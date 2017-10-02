using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RaptorDB;
using RaptorDB.Common;
using MongoDB.Bson.Serialization.Attributes;

/// <summary>
/// Summary description for CompaniesR
/// </summary>

public class CompaniesR
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; }

    public string Owner { get; set; }

    public string Type { get; set; }

    public string Location { get; set; }

    public List<string> Employees { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Checkbox { get; set; }

    public CompaniesR()
    {
        Id = Guid.NewGuid();
    }
}