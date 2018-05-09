using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace SGDemoFunctions.Infrastructure
{
    public static class MongoClientFactory
    {
        public static MongoClient Create()
        {
            string connectionString =
     @"mongodb://sgdemocosmodb2:zE5HVibUKyX4wvm7jEDcpEugNsrJehuTk4U9VroTjfcdKvZrJ2dJmzjLz7aT6fJ8ojP27hX3Y3oCTijChkTfHw==@sgdemocosmodb2.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            return new MongoClient(settings);

        }
    }
}
