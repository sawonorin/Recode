using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Recode.Data.EntityContract;

namespace Recode.Data.AppEntity
{
    public class ClientRequestLog: IMongoBaseEntity<String>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("UserId")]
        public long? UserId { get; set; }
        [BsonElement("ServiceName")]
        public string ServiceName { get; set; }
        [BsonElement("MethodName")]
        public string MethodName { get; set; }

        [BsonElement("ResponseBody")]
        public string ResponseBody { get; set; }

        [BsonElement("Parameters")]
        public string Parameters { get; set; }
        [BsonDateTimeOptions(DateOnly = false, Kind = DateTimeKind.Local)]
        public DateTime ExecutionTime { get; set; }
        [BsonElement("ExecutionDuration")]
        public int ExecutionDuration { get; set; }
        [BsonElement("ClientIpAddress")]
        public string ClientIpAddress { get; set; }
        [BsonElement("BrowserInfo")]
        public string BrowserInfo { get; set; }
        [BsonElement("ClientName")]
        public string ClientName { get; set; }

        public string Exception { get; set; }
        public override string ToString()
        {
            var loggedUserId = UserId.HasValue
                                   ? "user " + UserId.Value
                                   : "an anonymous user";

            var exceptionOrSuccessMessage = Exception != null
                ? "exception: " + Exception
                : "succeed";

            return $"AUDIT LOG: {ServiceName}.{MethodName} is executed by {loggedUserId} in {ExecutionDuration} ms from {ClientIpAddress} IP address with {exceptionOrSuccessMessage}.";
        }
    }
}
