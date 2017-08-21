using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RaptorDB;
using RaptorDB.Common;

/// <summary>
/// Summary description for Worker
/// </summary>
public class Workers
{
    public ObjectId Id { get; set; }
    [BsonElement("WorkerId")]
    public int WorkerId { get; set; }
    [BsonElement("FirstName")]
    public string FirstName { get; set; }
    [BsonElement("LastName")]
    public string LastName { get; set; }
    [BsonElement("Password")]
    public string Password { get; set; }
    [BsonElement("Email")]
    public string Email { get; set; }
}