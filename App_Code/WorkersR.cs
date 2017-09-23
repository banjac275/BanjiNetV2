using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RaptorDB;
using RaptorDB.Common;
using RaptorDB.Views;


/// <summary>
/// Summary description for WorkersR
/// </summary>

public class WorkersR
{
    public Guid Id { get; set; }

    public string CompanyId { get; set; }

    public string CompanyName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public string Checkbox { get; set; }

    public WorkersR()
    {
        Id = Guid.NewGuid();
    }
}

public abstract class RDBSchema : BindableFields
{
    public Guid docid;
}

public class RowShemaWorkers : RDBSchema
{
    public Guid Id { get; set; }

    public string CompanyId { get; set; }

    public string CompanyName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public string Checkbox { get; set; }

}

[RegisterView]
public class WorkersRView : View<WorkersR>
{
    public WorkersRView()
    {
        this.Name = "WorkersR";
        this.Description = "A primary view for WorkersR";
        this.isPrimaryList = true;
        this.isActive = true;
        this.BackgroundIndexing = true;
        this.DeleteBeforeInsert = true;
        this.Version = 1;

        this.Schema = typeof(RowShemaWorkers);

        this.FullTextColumns.Add("firstname");
        this.FullTextColumns.Add("lastname");
        this.FullTextColumns.Add("email");

        this.Mapper = (api, docid, doc) =>
        {
            this.Version += 1;
            api.EmitObject(docid, doc);
        };
    }
}
