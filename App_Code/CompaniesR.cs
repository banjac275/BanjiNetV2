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

    public string Employees { get; set; }

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
    public Guid Id { get; set; }

    public string CompanyName { get; set; }

    public string Owner { get; set; }

    public string Type { get; set; }

    public string Location { get; set; }

    public string Employees { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Checkbox { get; set; }
}

[RegisterView]
public class CompaniesRView : View<CompaniesR>
{
    public CompaniesRView()
    {
        this.Name = "CompaniesR";
        this.Description = "A primary view for Companies";
        this.isPrimaryList = true;
        this.isActive = true;
        this.BackgroundIndexing = true;
        this.DeleteBeforeInsert = true;
        //this.ConsistentSaveToThisView = true;
        this.Version = 1;

        this.Schema = typeof(RowShemaCompanies);

        this.FullTextColumns.Add("companyname");
        this.FullTextColumns.Add("owner");
        this.FullTextColumns.Add("type");
        this.FullTextColumns.Add("location");
        this.FullTextColumns.Add("email");

        this.Mapper = (api, docid, doc) =>
        {
            this.Version += 1;
            api.EmitObject(docid, doc);
        };
    }
}

