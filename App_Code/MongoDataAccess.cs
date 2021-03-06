﻿using System;
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
        _dbase = _client.GetDatabase("banjiNet");
    }

    public List<Workers> GetWorkers()
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        var users = collection.Find(Builders<Workers>.Filter.Empty).ToList();
        return users;
    }

    public List<Companies> GetCompanies()
    {
        var collection = _dbase.GetCollection<Companies>("companies");
        var companies = collection.Find(Builders<Companies>.Filter.Empty).ToList();
        return companies;
    }

    public Workers getWorkerById(ObjectId id)
    {
        var res = _dbase.GetCollection<Workers>("workers");
        //var filter = Builders<Workers>.Filter.Eq("_id", id);
        var result = res.Find(w => w.Id == id).ToList();

        if (result.Count != 0)
            return result[0];
        else
            return null;
    }

    public Companies getCompanyById(ObjectId id)
    {
        var res = _dbase.GetCollection<Companies>("companies");
        //var filter = Builders<Companies>.Filter.Eq("_id", id);
        var result = res.Find(c => c.Id == id).ToList();

        if (result.Count != 0)
            return result[0];
        else
            return null;
    }

    public List<Companies> getCompanyByName(string name)
    {
        var res = _dbase.GetCollection<Companies>("companies");
        var result = res.Find(n => n.CompanyName == name).ToList();
        return result;
    }

    public Workers getWorkerByEmail(string email)
    {
        var res = _dbase.GetCollection<Workers>("workers");
        //var filter = Builders<Workers>.Filter.Eq("Email", email);
        var result = res.Find(w => w.Email == email).ToList();

        if (result.Count != 0)
            return result[0];
        else
            return null;
    }

    public Companies getCompanyByEmail(string email)
    {
        var res = _dbase.GetCollection<Companies>("companies");
        var result = res.Find(w => w.Email == email).ToList();

        if (result.Count != 0)
            return result[0];
        else
            return null;
    }

    public Workers Create(Workers w)
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        collection.InsertOne(w);
        return w;
    }

    public Companies CreateCompany(Companies c)
    {
        var collection = _dbase.GetCollection<Companies>("companies");
        collection.InsertOne(c);
        return c;
    }

    public Workers updateWorker(Workers w)
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        var query_id = Builders<Workers>.Filter.Eq("_id", w.Id);
        var operation = collection.ReplaceOne(query_id, w);
        return w;
    }

    public Companies updateCompany(Companies c)
    {
        var collection = _dbase.GetCollection<Companies>("companies");
        var query_id = Builders<Companies>.Filter.Eq("_id", c.Id);
        var operation = collection.ReplaceOne(query_id, c);
        return c;
    }

    public string removeWorkerFromCompany(ObjectId id, Companies c)
    {
        if (c.Employees != null)
        {
            var collection = _dbase.GetCollection<Companies>("companies");
            var filter = new BsonDocument("_id", c.Id);
            var pull = Builders<Companies>.Update.Pull("Employees", id);
            var res = collection.FindOneAndUpdate(filter, pull);
            return "Worker removed from company!";
        }
        else
            return "Worker never existed!";
    }

    public string addWorkerToCompany(ObjectId id, Companies c)
    {
        var collection = _dbase.GetCollection<Companies>("companies");
        if(c.Employees == null)
        {
            c.Employees = new List<ObjectId>();
            c.Employees.Add(id);
            var query_id = Builders<Companies>.Filter.Eq("_id", c.Id);
            var operation = collection.ReplaceOne(query_id, c);
        }
        else
        {
            var filter = new BsonDocument("_id", c.Id);
            var push = Builders<Companies>.Update.Push("Employees", id);
            var res = collection.FindOneAndUpdate(filter, push);
        }
        return "Worker added to company!";
    }

    public string removeWorker(ObjectId id)
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        var query_id = Builders<Workers>.Filter.Eq("_id", id);
        var remove = collection.DeleteOne(query_id);
        return "Worker deleted!";
    }

    public string removeCompany(ObjectId id)
    {
        var collection = _dbase.GetCollection<Companies>("companies");
        var query_id = Builders<Companies>.Filter.Eq("_id", id);
        var remove = collection.DeleteOne(query_id);
        return "Company deleted!";
    }

    //search
    public List<Workers> getWorkerByNameS(string name)
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        var filter = new BsonDocument { { "FirstName", new BsonDocument { { "$regex", name }, { "$options", "i" } } } };
        var result = collection.Find(filter).ToList();
        return result;
        
    }

    public List<Workers> getWorkerByEmailS(string email)
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        var filter = new BsonDocument { { "Email", new BsonDocument { { "$regex", email }, { "$options", "i" } } } };
        var result = collection.Find(filter).ToList();
        return result;
    }

    public List<Workers> getWorkerByLastNameS(string name)
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        var filter = new BsonDocument { { "LastName", new BsonDocument { { "$regex", name }, { "$options", "i" } } } };
        var result = collection.Find(filter).ToList();
        return result;
    }

    public List<Workers> getWorkerWithSkillS(string name)
    {
        var collection = _dbase.GetCollection<Workers>("workers");
        var filter = new BsonDocument { { "Skills", new BsonDocument { { "$regex", name }, { "$options", "i" } } } };
        var result = collection.Find(filter).ToList();
        return result;
    }

    public List<Companies> getCompanyByEmailS(string email)
    {
        var collection = _dbase.GetCollection<Companies>("companies");
        var filter = new BsonDocument { { "Email", new BsonDocument { { "$regex", email }, { "$options", "i" } } } };
        var result = collection.Find(filter).ToList();
        return result;
    }

    public List<Companies> getCompanyByNameS(string name)
    {
        var collection = _dbase.GetCollection<Companies>("companies");
        var filter = new BsonDocument { { "CompanyName", new BsonDocument { { "$regex", name }, { "$options", "i" } } } };
        var result = collection.Find(filter).ToList();
        return result;
    }

}