using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

/// <summary>
/// Summary description for Companies
/// </summary>
public class Companies
{
    public ObjectId Id { get; set; }

    public string CompanyName { get; set; }

    public string Owner { get; set; }

    public string Type { get; set; }

    public string Location { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Checkbox { get; set; }

    [BsonIgnoreIfNullAttribute]
    public ObjectId[] Employees { get; set; }

    public Companies()
    {
    }
}