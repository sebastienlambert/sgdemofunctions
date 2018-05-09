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
                @"mongodb://sgdemocosmodb:xgVR8phzKbf4q5q69OYFqI6OVfPmuNEEbmAojXVxsdGYfkln8T55jyrVTZCCc7saUWgeQNiHhhpb2lI2AnbiqA==@sgdemocosmodb.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            return new MongoClient(settings);

        }
    }
}
