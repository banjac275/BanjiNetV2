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

    public async System.Threading.Tasks.Task<IEnumerable<Workers>> GetWorkersAsync()
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        var users = await collection.Find(Builders<Workers>.Filter.Empty).ToListAsync();
        return users;
    }

    public Workers getWorker(ObjectId id)
    {
        var res = _dbase.GetCollection<Workers>("workers");
        var filter = Builders<Workers>.Filter.Eq("_id", id);
        var result = res.Find(filter);
        return (Workers) result;
    }

    public Workers getWorkerByEmail(string email)
    {
        var res = _dbase.GetCollection<Workers>("workers");
        var filter = Builders<Workers>.Filter.Eq("Email", email);
        var result = res.Find(filter);
        return (Workers)result;
    }

    public async System.Threading.Tasks.Task<IEnumerable<Workers>> Create(Workers w)
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        await collection.InsertOneAsync(w);
        IEnumerable<Workers> users = (IEnumerable < Workers >)w;
        return users;
    }

    public async System.Threading.Tasks.Task updateWorkerAsync(ObjectId id, Workers w)
    {
        w.Id = id;
        var collection = _dbase.GetCollection<Workers>("workers");
        var query_id = Builders<Workers>.Filter.Eq("_id", w.Id);
        var operation = await collection.ReplaceOneAsync(query_id, w);
    }

    public async System.Threading.Tasks.Task removeWorkerAsync(ObjectId id)
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        var query_id = Builders<Workers>.Filter.Eq("_id", id);
        var remove = await collection.DeleteOneAsync(query_id);
    }
}