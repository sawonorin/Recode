using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Recode.Data.EntityContract;

namespace Recode.Data.AppEntity
{
    public enum AuditActionType
    {
        Create = 1,
        Edit,
        Delete
    }

    public class AuditEntityProperties
    {
        public long AuditLogId { get; set; }
        public string PropertyName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
    public class AuditLog : IMongoBaseEntity<String>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("UserId")]
        public long UserId { get; set; }
        [BsonDateTimeOptions(DateOnly=false,Kind = DateTimeKind.Local)]
        public DateTime EventDate { get; set; }
        [BsonElement("EventType")]
        public int EventType { get; set; }
        [BsonElement("TableName")]
        public string TableName { get; set; }
        [BsonElement("ColumnName")]
        public string ColumnName { get; set; }
        [BsonElement("OldValue")]
        public string OldValue { get; set; }
        [BsonElement("NewValue")]
        public string NewValue { get; set; }
        [BsonElement("ModifiedValue")]
        public string ModifiedValue { get; set; }
        [BsonElement("IPAddress")]
        public string IPAddress { get; set; }
    }
}
