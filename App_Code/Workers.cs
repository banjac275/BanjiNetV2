using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

/// <summary>
/// Summary description for Worker
/// </summary>
public class Workers
{
    public ObjectId Id { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
  
    public string Password { get; set; }
  
    public string Email { get; set; }

    public string CompanyId { get; set; }

    public string CompanyName { get; set; }

    public List<string> Skills { get; set; }

    public List<PrevEmpM> PreviousEmployment { get; set; }

    public List<ObjectId> Friends { get; set; }
}

public class PrevEmpM
{
    public ObjectId FirmId { get; set; }

    public ObjectId FormerEmployeeId { get; set; }

    public string FirmName { get; set; }

    public string StartTime { get; set; }

    public string EndTime { get; set; }
}