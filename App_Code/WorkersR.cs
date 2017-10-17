﻿using System;
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

    public string StartTime { get; set; }

    public string EndTime { get; set; }
}

public class Changes
{
    public string Type { get; set; }

    public Guid Actor1 { get; set; }

    public Guid Actor2 { get; set; }

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