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
    
    public Guid[] Employees { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Checkbox { get; set; }

    public CompaniesR()
    {
        Id = Guid.NewGuid();
    }
}

public class RowShemaCompanies : RDBSchema
{
    public string CompanyName;

    public string Owner;

    public string Type;

    public string Location;

    public string Email;

    public string Password;

    public string Checkbox;
}

[RegisterView]
public class CompaniesRView : View<CompaniesR>
{
    public CompaniesRView()
    {
        this.Name = "CompaniesR";
        this.Description = "A primary view for Companies";
        this.isPrimaryList = false;
        this.isActive = true;
        this.BackgroundIndexing = true;
        this.Version = 2;

        this.Schema = typeof(RowShemaCompanies);

        this.FullTextColumns.Add("companyname");
        this.FullTextColumns.Add("owner");
        this.FullTextColumns.Add("type");
        this.FullTextColumns.Add("location");
        this.FullTextColumns.Add("email");

        this.Mapper = (api, docid, doc) =>
        {
            api.EmitObject(docid, doc);
        };
    }
}