using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RaptorDB;
using RaptorDB.Common;
using RaptorDB.Views;
using System.Threading;

/// <summary>
/// Summary description for RaptorDataAccess
/// </summary>
public class RaptorDataAccess
{
    public RaptorDB.RaptorDB rap;
    //public Thread t;

    public RaptorDataAccess()
	{
        rap = RaptorDB.RaptorDB.Open("C:\\Users\\nikol\\Documents\\GitHub\\BanjiNetV2\\RaptorServerr\\bin\\Debug\\data");
        //Thread.Sleep(3000);
        //t.Start();
        rap.RegisterView(new WorkersRView());
        rap.RegisterView(new CompaniesRView());
        //t.Suspend();
    }

    public WorkersR Create(WorkersR w)
    {
        rap.Save<WorkersR>(w.Id, w);
        rap.Shutdown();
        return w;
    }

    public Result<RowShemaWorkers> getWorkerByEmail(string email)
    {
        var result = rap.Query<RowShemaWorkers>(x => x.Email == email);
        rap.Shutdown();       
        return result;
    }

    public WorkersR getWorkerById(Guid id)
    {
        var result = rap.Fetch<WorkersR>(id);
        rap.Shutdown();
        return result;
    }

    public Result<RowShemaWorkers> getWorkerByName(string name)
    {
        var result = rap.Query<RowShemaWorkers>(x => x.FirstName == name);
        rap.Shutdown();
        return result;
    }

    public Result<RowShemaWorkers> getWorkerByLastName(string name)
    {
        var result = rap.Query<RowShemaWorkers>(x => x.LastName == name);
        rap.Shutdown();
        return result;
    }

    public WorkersR updateWorker(Guid id, WorkersR w)
    {
        if (rap.Save<WorkersR>(w.Id, w))
        {
            rap.Shutdown();
            return w;
        }
        else
        {
            rap.Shutdown();
            return null;
        }
    }

    public string removeWorker(Guid id)
    {
        if (rap.Delete(id))
        {
            rap.Shutdown();
            return "Worker deleted!";
        }            
        else
        {
            rap.Shutdown();
            return "Worker not deleted!";
        }
            
    }

    public CompaniesR CreateCompany(CompaniesR c)
    {
        if (rap.Save<CompaniesR>(c.Id, c))
        {
            rap.Shutdown();
            return c;
        }            
        else
        {
            rap.Shutdown();
            return null;
        }            
    }

    public CompaniesR getCompanyById(Guid id)
    {
        //var result = rap.Fetch(id);
        var result = rap.Query("CompaniesR");
        rap.Shutdown();
        if (result.Count != 0)
        {
            for (int i = 0; i < result.Count; i++)
            {
                var temp = result.Rows[i] as CompaniesR;
                if (temp.Id == id)
                    return temp;
            }
        }
        return null;
    }

    public CompaniesR getCompanyByName(string name)
    {
        var result = rap.Query("CompaniesR");
        rap.Shutdown();
        if (result.Count != 0)
        {
            for (int i = 0; i < result.Count; i++)
            {
                var temp = result.Rows[i] as CompaniesR;
                if (temp.CompanyName == name)
                    return temp;
            }
        }
        return null;
    }

    public RowShemaCompanies getCompanyByEmail(string email)
    {
        //var result = rap.Query<RowShemaCompanies>(x => x.Email == email);
        //rap.Shutdown();
        //return result;
        var result = rap.Query("CompaniesR");
        rap.Shutdown();
        if (result.Count != 0)
        {
            for (int i = 0; i < result.Count; i++)
            {
                var temp = result.Rows[i] as RowShemaCompanies;
                if (temp.Email == email)
                    return temp;
            }
        }
        return null;
    }

    public CompaniesR updateCompany(Guid id, CompaniesR c)
    {
        if (rap.Save<CompaniesR>(c.Id, c))
        {
            rap.Shutdown();
            return c;
        }
        else
        {
            rap.Shutdown();
            return null;
        }
    }

    public List<RowShemaCompanies> GetCompanies()
    {
        var result = rap.Query("CompaniesR");
        rap.Shutdown();
        if (result.Count != 0)
        {
            List<RowShemaCompanies> retL = new List<RowShemaCompanies>();
            for(int i = 0; i< result.Count; i++)
            {
                retL.Add(result.Rows[i] as RowShemaCompanies);
            }
            return retL;
        }
        return null;
    }
}