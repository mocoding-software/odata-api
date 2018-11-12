using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Mocoding.AspNetCore.ODataApi.EasyDocDb.Helpers;
using Mocoding.EasyDocDb;
using Mocoding.EasyDocDb.FileSystem;
using Mocoding.EasyDocDb.Json;

namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    public class DocumentRepositoryFactory
    {
        private readonly string _conn;
        private readonly IRepository _repository;
        private readonly IDictionary<Type, object> _crudRepositories;

        public DocumentRepositoryFactory(string conn)
        {
            _conn = conn;
            _repository = new EmbeddedRepository(new JsonSerializer());
            _crudRepositories = new ConcurrentDictionary<Type, object>();
        }

        public DocumentRepositoryFactory(string conn, IDocumentStorage storage)
        {
            _conn = conn;
            _repository = new EmbeddedRepository(new JsonSerializer(), storage);
            _crudRepositories = new ConcurrentDictionary<Type, object>();
        }

        public ICrudRepository<TEntity, TKey> Get<TEntity, TKey>(IEntityKeyAccossor keyAccossor)
            where TEntity : class, new()
        {
            var t = typeof(TEntity);
            if (_crudRepositories.ContainsKey(t))
                return _crudRepositories[t] as ICrudRepository<TEntity, TKey>;

            var name = t.Name.ToLower();
            var attribute = t.GetCustomAttribute(typeof(ReadOptimizedAttribute));
            var repo = attribute != null
                ? new DocumentCrudRepository<TEntity, TKey>(_repository.Init<List<TEntity>>(Path.Combine(_conn, name + ".json")).Result, keyAccossor)
                : new DocumentCollectionCrudRepository<TEntity, TKey>(_repository.InitCollection<TEntity>(Path.Combine(_conn, name)).Result, keyAccossor) as ICrudRepository<TEntity, TKey>;

            _crudRepositories.Add(t, repo);

            return repo;
        }
    }
}