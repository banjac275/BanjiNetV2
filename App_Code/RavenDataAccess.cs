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
        new WorkersR_byName().Execute(_store);
        new WorkersR_byLastName().Execute(_store);
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

    public WorkersR getWorkerByEmail(string email)
    {
        var result = _session.Query<WorkersR, WorkersR_byEmail>().Search(x => x.Email, email).ToList();
        if (result.Count != 0)
            return result[0];
        else
            return null;
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

    public WorkersR getWorkerById(Guid id)
    {
        var find = _session.Query<WorkersR>().ToList();
        if(find != null)
            for(int i = 0; i < find.Count; i++)
            {
                if(find[i].Id == id)
                {
                    return find[i];
                }
            }
        return null;
    }

    public CompaniesR getCompanyById(Guid id)
    {
        var find = _session.Query<CompaniesR>().ToList();
        if (find != null)
            for (int i = 0; i < find.Count; i++)
            {
                if (find[i].Id == id)
                {
                    return find[i];
                }
            }
        return null;
    }

    public string removeWorkerFromCompany(Guid id, CompaniesR c)
    {
        var check = false;
        List<Guid> temp = new List<Guid>();
        if (c.Employees != null)
        {
            for (int i = 0; i < c.Employees.Count; i++)
            {
                if (id != c.Employees[i])
                    temp.Add(c.Employees[i]);
                else
                    check = true;
            }
            c.Employees = temp;
            _session.Store(c);
            _session.SaveChanges();
            if (check == true)
                return "Worker removed from company!";
            else
                return "Such worker never existed in registry!";
        }
        else
            return "Such worker doesn't exist in registry!";        
    }

    public string addWorkerToCompany(Guid id, CompaniesR c)
    {
        if (c.Employees == null)
        {
            c.Employees = new List<Guid>();
            c.Employees.Add(id);
        }
        else
            c.Employees.Add(id);

        _session.Store(c);
        _session.SaveChanges();
        return "Worker added to the company!";
    }

    public WorkersR updateWorker(WorkersR w)
    {
        _session.Store(w);
        _session.SaveChanges();
        return w;
    }

    public CompaniesR updateCompany(CompaniesR c)
    {
        _session.Store(c);
        _session.SaveChanges();
        return c;
    }

    public List<WorkersR> GetWorkers()
    {
        var users = _session.Query<WorkersR>().ToList();
        return users;
    }

    public List<CompaniesR> GetCompanies()
    {
        var companies = _session.Query<CompaniesR>().ToList();
        return companies;
    }

    public List<WorkersR> getWorkerByName(string name)
    {
        //var result = _session.Query<WorkersR, WorkersR_byName>().Search(x => x.FirstName, name+"*").ToList();
        var result = _session.Advanced.DocumentQuery<WorkersR, WorkersR_byName>().WhereStartsWith(x => x.FirstName, name).ToList();
        return result;
    }

    public List<WorkersR> getWorkerByEmailS(string email)
    {
        var result = _session.Advanced.DocumentQuery<WorkersR, WorkersR_byEmail>().WhereStartsWith(x => x.Email, email).ToList();
        return result;
    }

    public List<WorkersR> getWorkerByLastNameS(string name)
    {
        var result = _session.Advanced.DocumentQuery<WorkersR, WorkersR_byLastName>().WhereStartsWith(x => x.LastName, name).ToList();
        return result;
    }

    public List<WorkersR> getWorkerWithSkillS(string name)
    {
        var result = _session.Query<WorkersR>().ToList();
        if(result.Count > 0)
        {
            List<WorkersR> temp = new List<WorkersR>();
            for (int i = 0; i<result.Count; i++)
            {
                if(result[i].Skills != null)
                {
                    for(int j = 0; j < result[i].Skills.Count; j++)
                    {
                        var tmp = result[i].Skills[j];
                        if (tmp.ToLower().Contains(name.ToLower()))
                            temp.Add(result[i]);
                    }
                }
            }
            if (temp.Count > 0)
                result = temp;
            else
                result = null;
        }
        return result;
    }

    public List<CompaniesR> getCompanyByEmailS(string email)
    {
        var result = _session.Advanced.DocumentQuery<CompaniesR, CompaniesR_byEmail>().WhereStartsWith(x => x.Email, email).ToList();
        return result;
    }

    public List<CompaniesR> getCompanyByNameS(string name)
    {
        var result = _session.Advanced.DocumentQuery<CompaniesR, CompaniesR_byName>().WhereStartsWith(x => x.CompanyName, name).ToList();
        return result;
    }

    public Changes addFriendChange(Changes c)
    {
        List<Changes> change = getChanges();
        if (change == null)
        {
            ChangeFinal cf = new ChangeFinal();
            cf.Change = new List<Changes>();
            cf.Change.Add(c);
            _session.Store(cf);
            _session.SaveChanges();
            return c;
        }
        else
        {
            var changee = _session.Query<ChangeFinal>().ToList();
            changee[0].Change.Add(c);
            _session.Store(changee[0]);
            _session.SaveChanges();
            return c;
        }
    }

    public List<Changes> getChanges()
    {
        var change = _session.Query<ChangeFinal>().ToList();
        if (change.Count != 0)
            return change[0].Change;
        else
            return null;
    }

    public string deleteWorker(WorkersR w)
    {
        _session.Delete<WorkersR>(w);
        _session.SaveChanges();
        return "Worker deleted!";
    }

    public string deleteCompany(CompaniesR c)
    {
        _session.Delete<CompaniesR>(c);
        _session.SaveChanges();
        return "Company deleted!";
    }

    public string setDB(DBCheck dbc)
    {
        List<DBCheck> change = getDBPref();
        if (change == null)
        {
            DBCheckFinal cf = new DBCheckFinal();
            cf.Check = new List<DBCheck>();
            cf.Check.Add(dbc);
            _session.Store(cf);
            _session.SaveChanges();
            return "DB Set";
        }
        else
        {
            var changee = _session.Query<DBCheckFinal>().ToList();
            List<DBCheck> temp = new List<DBCheck>();
            var temCh = false;
            for(int i = 0; i < change.Count; i++)
            {
                if(change[i].RavenId == dbc.RavenId && change[i].MongoId == dbc.MongoId)
                {
                    temCh = true;
                    change[i].Mail = dbc.Mail;
                    change[i].Password = dbc.Password;
                    change[i].DbName = dbc.DbName;
                    change[i].Collection = dbc.Collection;
                    changee[0].Check = change;
                    _session.Store(changee[0]);
                    _session.SaveChanges();
                }
            }

            if(temCh == false)
            {
                changee[0].Check.Add(dbc);
                _session.Store(changee[0]);
                _session.SaveChanges();
            }
            return "DB Set";
        }
    }

    public List<DBCheck> getDBPref()
    {
        var check = _session.Query<DBCheckFinal>().ToList();
        if (check.Count != 0)
            return check[0].Check;
        else
            return null;
    }
}