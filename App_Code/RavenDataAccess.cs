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
}