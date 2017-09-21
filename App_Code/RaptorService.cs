using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using RaptorDB;
using RaptorDB.Common;
using RaptorDB.Views;

/// <summary>
/// Summary description for RaptorService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class RaptorService : System.Web.Services.WebService
{
    RaptorDataAccess raptor;
    string badp = "Bad password, please try again!";
    string badm = "No users were found with this email, please sign up!";
    string succ = "User database entry was a success!";
    string fail = "User database entry failed!";

    public RaptorService()
    {
        raptor = new RaptorDataAccess();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string enterNewWorkerInRDb(string mail, string pass, string name, string last, string check)
    {
        WorkersR w = new WorkersR()
        {
            FirstName = name,
            LastName = last,
            Email = mail,
            Password = pass,
            Checkbox = check
        };
        

        var ret = raptor.Create(w);

        if (ret != null)
        {
            HttpContext.Current.Session.Add("userR", ret);
            return succ;
        }
        return fail;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string returnWorkerFromEmailR(string mail, string pass)
    {
        
        Result<RowShemaWorkers> w = raptor.getWorkerByEmail(mail);

        var temp = w.Rows[0];

        WorkersR wor = new WorkersR()
        {
            Id = temp.docid,
            CompanyId = temp.CompanyId,
            CompanyName = temp.CompanyName,
            FirstName = temp.FirstName,
            LastName = temp.LastName,
            Email = temp.Email,
            Password = temp.Password,
            Checkbox = temp.Checkbox
        };

        if (w.Count != 0)
        {
            if (wor.Password == pass)
            {
                HttpContext.Current.Session.Add("userR", wor);
                HttpContext.Current.Session.Add("companyR", null);
                return JsonConvert.SerializeObject(wor);
            }
            else
                return badp;
        }
        else
            return badm;
        
    }
}
