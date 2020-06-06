using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Options;
using Mocoding.AspNetCore.ODataApi.EasyDocDb.Helpers;
using Mocoding.EasyDocDb;

namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    public class DocumentRepositoryFactory
    {
        private readonly IEntityKeyAccessor _keyAccessor;
        private readonly string _conn;
        private readonly IRepository _repository;
        private readonly IDocumentSerializer _serializer;
        private readonly IDictionary<Type, object> _crudRepositories;

        public DocumentRepositoryFactory(IEntityKeyAccessor keyAccessor, IOptions<ODataEasyDocDbOptions> options, IRepository repository, IDocumentSerializer serializer)
        {
            _keyAccessor = keyAccessor;
            _conn = options.Value.Connection;
            _repository = repository;
            _serializer = serializer;
            _crudRepositories = new ConcurrentDictionary<Type, object>();
        }

        public ICrudRepository<TEntity, TKey> Create<TEntity, TKey>()
            where TEntity : class, new()
        {
            var t = typeof(TEntity);
            if (_crudRepositories.ContainsKey(t))
                return _crudRepositories[t] as ICrudRepository<TEntity, TKey>;

            var name = t.Name.ToLower();
            var attribute = t.GetCustomAttribute(typeof(ReadOptimizedAttribute));
            var repo = attribute != null
                ? new DocumentCrudRepository<TEntity, TKey>(_repository.Init<List<TEntity>>(Path.Combine(_conn, $"{name}.{_serializer.Type}")).Result, _keyAccessor)
                : new DocumentCollectionCrudRepository<TEntity, TKey>(_repository.InitCollection<TEntity>(Path.Combine(_conn, name)).Result, _keyAccessor) as ICrudRepository<TEntity, TKey>;

            _crudRepositories.Add(t, repo);

            return repo;
        }
    }
}