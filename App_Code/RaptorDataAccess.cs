using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RaptorDB;
using RaptorDB.Common;
using RaptorDB.Views;

/// <summary>
/// Summary description for RaptorDataAccess
/// </summary>
public class RaptorDataAccess
{
    IRaptorDB rap;

	public RaptorDataAccess()
	{
        var p = RaptorDB.RaptorDB.Open("C:\\Users\\nikol\\Documents\\GitHub\\BanjiNetV2\\data");
        p.RegisterView(new WorkersRView());
        p.RegisterView(new CompaniesRView());
        rap = p;
	}

    public WorkersR Create(WorkersR w)
    {
        rap.Save(w.Id, w);
        rap.Shutdown();
        return w;
    }

    public Result<RowShemaWorkers> getWorkerByEmail(string email)
    {
        var result = rap.Query<RowShemaWorkers>(x => x.Email == email);
        //rap.Shutdown();       
        return result;
    }
}