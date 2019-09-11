using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Data.EntityContract
{
    public interface IMongoBaseEntity<TKey>
    {
        /// <summary>
        /// The Primary Key, which must be decorated with the [BsonId] attribute 
        /// if you want the MongoDb C# driver to consider it to be the document ID.
        /// </summary>
        [BsonId]
        TKey Id { get; set; }
    }
}
