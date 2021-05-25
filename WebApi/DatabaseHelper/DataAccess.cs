﻿using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.DatabaseHelper
{
    public class DataAccess
    {
        private IMongoDatabase _db;
        private string _connectionString = "mongodb://localhost:27017";
        private string _collection = "EducationSystem";

        /// <summary>
        /// Instanciates the DataAccess and sets up the client and collection
        /// </summary>
        public DataAccess()
        {
            var client = new MongoClient(_connectionString);
            _db = client.GetDatabase(_collection);
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