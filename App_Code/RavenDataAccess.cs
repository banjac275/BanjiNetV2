using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using Raven.Client;
using Raven.Client.Document;

/// <summary>
/// Summary description for RavenDataAccess
/// </summary>
public class RavenDataAccess
{
    IDocumentStore _store;
    IDocumentSession _session;

    public RavenDataAccess()
    {
        _store = new DocumentStore
        {
            Url = "http://localhost:8080/",
            DefaultDatabase = "banjiNetdb"
        };
        _store.Initialize();

        //indeksi
        new WorkersR_byEmail().Execute(_store);
        new CompaniesR_byEmail().Execute(_store);
        new CompaniesR_byName().Execute(_store);

        _session = _store.OpenSession();
    }

    public WorkersR Create(WorkersR w)
    {
        _session.Store(w);
        _session.SaveChanges();
        return w;
    }

    public CompaniesR CreateCompany(CompaniesR c)
    {
        _session.Store(c);
        _session.SaveChanges();
        return c;
    }

    public List<WorkersR> getWorkerByEmail(string email)
    {
        var result = _session.Query<WorkersR, WorkersR_byEmail>().Search(x => x.Email, email).ToList();
        return result;
    }

    public List<CompaniesR> getCompanyByEmail(string email)
    {
        var result = _session.Query<CompaniesR, CompaniesR_byEmail>().Search(x => x.Email, email).ToList();
        return result;
    }

    public List<CompaniesR> getCompanyByName(string name)
    {
        var result = _session.Query<CompaniesR, CompaniesR_byName>().Search(x => x.CompanyName, name).ToList();
        return result;
    }
}