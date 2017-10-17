using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raven.Client;

/// <summary>
/// Summary description for RavenService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class RavenService : System.Web.Services.WebService
{
    RavenDataAccess raven;
    string badp = "Bad password, please try again!";
    string badm = "No users were found with this email, please sign up!";
    string succ = "User database entry was a success!";
    string fail = "User database entry failed!";

    public RavenService()
    {
        raven = new RavenDataAccess(); 
    }

    //upis
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string enterNewWorkerInRDb(string mail, string pass, string name, string last)
    {
        WorkersR w = new WorkersR()
        {
            FirstName = name,
            LastName = last,
            Email = mail,
            Password = pass
        };

        var ret = raven.Create(w);

        if (ret != null)
        {
            HttpContext.Current.Session.Add("userR", ret);
            return succ;
        }
        return fail;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string enterNewCompanyInRDb(string company, string owner, string type, string location, string mail, string pass)
    {
        CompaniesR c = new CompaniesR()
        {
            CompanyName = company,
            Owner = owner,
            Type = type,
            Location = location,
            Email = mail,
            Password = pass
        };

        var ret = raven.CreateCompany(c);

        if (ret != null)
        {
            HttpContext.Current.Session.Add("companyR", ret);
            return succ;
        }
        return fail;
    }

    //trazenje radnika po mejlu
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string returnWorkerFromEmailR(string mail, string pass)
    {
        
        List<WorkersR> w = raven.getWorkerByEmail(mail);
       
        if (w.Count != 0)
        {
            for (int i = 0; i < w.Count; i++)
            {
                if (w[i].Password == pass && w[i].Email == mail)
                {
                    HttpContext.Current.Session.Add("userR", w[0]);
                    HttpContext.Current.Session.Add("companyR", null);
                    return JsonConvert.SerializeObject(w[i]);
                }                  
            }
            return badp;
        }
        else
            return badm;
        
    }

    //trazenje firme po mailu
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string returnCompanyFromEmailR(string mail, string pass)
    {
        List<CompaniesR> w = raven.getCompanyByEmail(mail);

        if (w.Count != 0)
        {
            for (int i = 0; i < w.Count; i++)
            {
                if (w[0].Password == pass && w[i].Email == mail)
                {
                    HttpContext.Current.Session.Add("companyR", w[0]);
                    HttpContext.Current.Session.Add("userR", null);
                    return JsonConvert.SerializeObject(w[0]);
                }
            }
            return badp;
        }
        else
            return badm;
    }

    //trazenje firme po imenu
    [System.Web.Services.WebMethod]
    public string retCompanyFromNameR(string name)
    {
        List<CompaniesR> c = raven.getCompanyByName(name);

        if (c.Count != 0)
        {
            for (int i = 0; i < c.Count; i++)
            {
                if (c[0].CompanyName == name)
                {
                    return JsonConvert.SerializeObject(c[i]);
                }
            }
            return "Company not found!";
        }
        else
            return "Company with that name doesn't exist in our registry!";

    }

    //apdejtovanje profila radnika
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string updateWorkerInDb(string id, string mail, string pass, string name, string last, string company)
    {
        List<WorkersR> recvv = raven.getWorkerById(Guid.Parse(id));


        recvv[0].Email = mail;
        recvv[0].Password = pass;
        recvv[0].FirstName = name;
        recvv[0].LastName = last;

        var temp = recvv[0].CompanyName;

        var cId = mongoDbase.getCompanyByName(company);

        if (cId.Count == 0)
        {
            return "There is no such company!";
        }

        if (temp != company && temp != null)
        {
            var tempC = mongoDbase.getCompanyByName(temp);
            var ret = mongoDbase.removeWorkerFromCompany(recvv[0].Id, tempC[0]);
            var com = mongoDbase.addWorkerToCompany(recvv[0].Id, cId[0]);
            recvv[0].CompanyId = cId[0].Id;
            recvv[0].CompanyName = cId[0].CompanyName;
        }
        else
        {
            var com = mongoDbase.addWorkerToCompany(recvv[0].Id, cId[0]);
            recvv[0].CompanyId = cId[0].Id;
            recvv[0].CompanyName = cId[0].CompanyName;
        }

        var res = mongoDbase.updateWorker(recvv[0].Id, recvv[0]);

        if (res != null)
        {
            HttpContext.Current.Session.Add("user", res);
            return "Update successfull!";
        }
        return fail;
    }

}
