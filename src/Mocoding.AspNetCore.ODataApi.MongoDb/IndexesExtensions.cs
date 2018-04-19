using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Internal;
using MongoDB.Driver;

namespace Mocoding.AspNetCore.ODataApi.MongoDb
{
    public class IndexedAttribute : Attribute
    {
        
    }

    static class IndexesExtensions
    {
        private const string Prefix = "__IndexedAttribute__";

        public static void EnsureIndexes<T>(this IMongoCollection<T> mongoCollection)
        {
            var t = typeof(T);
            var indexProperties = t.GetProperties().Where(_ => Attribute.IsDefined(_, typeof(IndexedAttribute)))
                .Select(_ => _.Name).ToArray();
            var indexes = mongoCollection.Indexes.List();
            indexes.MoveNext();
            var existingIndexes = indexes.Current.Select(_ => _["name"].AsString)
                .Where(_ => _.StartsWith(Prefix))
                .Select(_ => _.Remove(0, Prefix.Length)).ToArray();

            var newIndexes = indexProperties.Except(existingIndexes);
            var deletedIndexes = existingIndexes.Except(indexProperties);

            foreach (var deletedIndex in deletedIndexes)
                mongoCollection.Indexes.DropOne(Prefix + deletedIndex);

            foreach (var nexIndex in newIndexes)
            {
                var builder = new IndexKeysDefinitionBuilder<T>();
                mongoCollection.Indexes.CreateOne(builder.Ascending(nexIndex), new CreateIndexOptions() { Name = Prefix + nexIndex });
            }

        }
    }
}
