using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Abstractions.Indexing;

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

    public List<string> Skills { get; set; }

    public List<PrevEmp> PreviousEmployment { get; set; }

    public List<Guid> Friends { get; set; }

    public WorkersR()
    {
        Id = Guid.NewGuid();
    }
}

public class PrevEmp
{
    public Guid FirmId { get; set; }

    public Guid FormerEmployeeId { get; set; }

    public string FirmName { get; set; }

    public string StartTime { get; set; }

    public string EndTime { get; set; }
}

public class Changes
{
    public string Type { get; set; }

    public Guid Actor1 { get; set; }

    public string Actor1Name { get; set; }

    public string Actor1Collection { get; set; }

    public Guid Actor2 { get; set; }

    public string Actor2Name { get; set; }

    public string Actor2Collection { get; set; }

    public string Time { get; set; }
}

//za brze pretrage
//po mejlu
public class WorkersR_byEmail: AbstractIndexCreationTask<WorkersR>
{
    public WorkersR_byEmail()
    {
        Map = workers => from worker in workers
                         select new
                         {
                             worker.Email
                         };

        Indexes.Add(x => x.Email, FieldIndexing.Analyzed);
    }
}

//po imenu
public class WorkersR_byName : AbstractIndexCreationTask<WorkersR>
{
    public WorkersR_byName()
    {
        Map = workers => from worker in workers
                         select new
                         {
                             worker.FirstName
                         };

        Indexes.Add(x => x.FirstName, FieldIndexing.Analyzed);
    }
}

//po prezimenu
public class WorkersR_byLastName : AbstractIndexCreationTask<WorkersR>
{
    public WorkersR_byLastName()
    {
        Map = workers => from worker in workers
                         select new
                         {
                             worker.LastName
                         };

        Indexes.Add(x => x.LastName, FieldIndexing.Analyzed);
    }
}