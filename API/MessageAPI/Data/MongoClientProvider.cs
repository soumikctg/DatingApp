﻿using System.Collections.Concurrent;
using MessageAPI.Interfaces;
using MongoDB.Driver;

namespace MessageAPI.Data;

public class MongoClientProvider : IMongoClientProvider
{
    private readonly ConcurrentDictionary<string, MongoClient> _clients;

    public MongoClientProvider()
    {
        _clients = new();
    }

    public MongoClient GetClient(string connectionString)
    {
        if (_clients.TryGetValue(connectionString, out MongoClient client))
        {
            return client;
        }

        var newClient = new MongoClient(connectionString);

        _clients.TryAdd(connectionString, newClient);

        return newClient;
    }
}