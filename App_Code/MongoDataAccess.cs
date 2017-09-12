using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Core;
using System.Web.Script.Services;

/// <summary>
/// Summary description for MongoDataAccess
/// </summary>
public class MongoDataAccess
{
    IMongoClient _client;
    IMongoDatabase _dbase;

    public MongoDataAccess()
    {
        _client = new MongoClient("mongodb://localhost:27017");
        _dbase = _client.GetDatabase("userdb");
    }

    public List<Workers> GetWorkers()
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        var users = collection.Find(Builders<Workers>.Filter.Empty).ToList();
        return users;
    }

    public Workers getWorker(ObjectId id)
    {
        var res = _dbase.GetCollection<Workers>("workers");
        var filter = Builders<Workers>.Filter.Eq("_id", id);
        var result = res.Find(filter);
        return (Workers) result;
    }

    public List<Workers> getWorkerByEmail(string email)
    {
        var res = _dbase.GetCollection<Workers>("workers");
        //var filter = Builders<Workers>.Filter.Eq("Email", email);
        var result = res.Find(w => w.Email == email).ToList();
        return result;
    }

    public Workers Create(Workers w)
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        collection.InsertOne(w);
        return w;
    }

    public Workers updateWorker(ObjectId id, Workers w)
    {
        w.Id = id;
        var collection = _dbase.GetCollection<Workers>("workers");
        var query_id = Builders<Workers>.Filter.Eq("_id", w.Id);
        var operation = collection.ReplaceOne(query_id, w);
        return w;
    }

    public string removeWorker(ObjectId id)
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        var query_id = Builders<Workers>.Filter.Eq("_id", id);
        var remove = collection.DeleteOne(query_id);
        return "Worker deleted!";
    }

}