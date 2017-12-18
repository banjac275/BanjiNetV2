using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Abstractions.Indexing;

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

    public string Email { get; set; }

    public string Password { get; set; }

    public List<Guid> Employees { get; set; }

    public CompaniesR()
    {
        Id = Guid.NewGuid();
    }
}

//fje za brzu pretragu
//po mejlu
public class CompaniesR_byEmail: AbstractIndexCreationTask<CompaniesR>
{
    public CompaniesR_byEmail()
    {
        Map = companies => from company in companies
                         select new
                         {
                             company.Email
                         };

        Indexes.Add(x => x.Email, FieldIndexing.Analyzed);
    }
}

//po imenu
public class CompaniesR_byName : AbstractIndexCreationTask<CompaniesR>
{
    public CompaniesR_byName()
    {
        Map = companies => from company in companies
                           select new
                           {
                               company.CompanyName
                           };

        Indexes.Add(x => x.CompanyName, FieldIndexing.Analyzed);
    }
}