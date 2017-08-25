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
  
    public int CompanyId { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
  
    public string Password { get; set; }
  
    public string Email { get; set; }

    public string Checkbox { get; set; }

    public Workers()
    {
        Random rnd = new Random();

        CompanyId = rnd.Next(10);
        FirstName = null;
        LastName = null;
        Password = null;
        Email = null;
        Checkbox = null;
    }
}