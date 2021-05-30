using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApi.Helpers
{
    public class DatabaseHelper
    {
        private readonly IMongoDatabase _db;

        /// <summary>
        /// Instanciates the DatabaseHelper and sets up the client and collection
        /// </summary>
        public DatabaseHelper(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDB");
            var database = configuration["MongoDatabase"];

            var client = new MongoClient(connectionString);
            _db = client.GetDatabase(database);
        }

        /// <summary>
        /// Gets a list of documents from specified collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public IEnumerable<T> GetDocuments<T>(string collection)
        {
            var dbCollection = _db.GetCollection<T>(collection);
            return dbCollection.Find(new BsonDocument()).ToEnumerable();
        }

        /// <summary>
        /// Gets a specific document from specified collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetDocumentById<T>(string collection, ObjectId id)
        {
            var dbCollection = _db.GetCollection<T>(collection);
            var filter = Builders<T>.Filter.Eq("Id", id);
            return dbCollection.Find(filter).First();
        }

        public IEnumerable<T> GetDocumentsByFilter<T>(string collection, FilterDefinition<T> filter)
        {
            var dbCollection = _db.GetCollection<T>(collection);
            return dbCollection.Find(filter).ToEnumerable();
        }

        /// <summary>
        /// Insert document in specified collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="document"></param>
        public void InsertDocument<T>(string collection, T document)
        {
            var dbCollection = _db.GetCollection<T>(collection);
            dbCollection.InsertOne(document);
        }

        /// <summary>
        /// Replaces or insert a specific document in the specified collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="id"></param>
        /// <param name="document"></param>
        public void UpsertDocument<T>(string collection, ObjectId id, T document)
        {
            var dbCollection = _db.GetCollection<T>(collection);
            var filter = Builders<T>.Filter.Eq("Id", id);
            var options = new ReplaceOptions { IsUpsert = true };
            _ = dbCollection.ReplaceOne(filter, document, options);
        }

        /// <summary>
        /// Replaces a specific document in the specified collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="id"></param>
        /// <param name="document"></param>
        public void ReplaceDocument<T>(string collection, ObjectId id, T document)
        {
            var dbCollection = _db.GetCollection<T>(collection);
            var filter = Builders<T>.Filter.Eq("Id", id);
            _ = dbCollection.ReplaceOne(filter, document);
        }

        /// <summary>
        /// Deletes a specific document from the specified collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="id"></param>
        public void DeleteDocument<T>(string collection, ObjectId id)
        {
            var dbCollection = _db.GetCollection<T>(collection);
            var filter = Builders<T>.Filter.Eq("Id", id);
            _ = dbCollection.DeleteOne(filter);
        }
    }
}
