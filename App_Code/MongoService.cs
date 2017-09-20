using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Threading;
using MongoDB.Bson;
using System.Web.UI;
using System.Web.UI.WebControls;


/// <summary>
/// Summary description for MongoService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class MongoService : System.Web.Services.WebService
{
    MongoDataAccess mongoDbase;
    string badp = "Bad password, please try again!";
    string badm = "No users were found with this email, please sign up!";
    string succ = "User database entry was a success!";
    string fail = "User database entry failed!";

    public MongoService()
    {
        mongoDbase = new MongoDataAccess();
    }

    //trazi radnika u bazi i proverava da li mu je dobra sifra
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string returnWorkerFromEmail(string mail, string pass)
    {
        //var obj = JObject.Parse(jsons);
        //var mail = (string)obj.SelectToken("Email");
        List<Workers> w = mongoDbase.getWorkerByEmail(mail);
        //JavaScriptSerializer jserial = new JavaScriptSerializer();
        if (w.Count != 0)
        {
            if (w[0].Password == pass)
            {
                HttpContext.Current.Session.Add("user", w[0]);
                HttpContext.Current.Session.Add("company", null);
                return JsonConvert.SerializeObject(w[0]);
            }
            else
                return badp;
        }
        else
            return badm;
        //return w.ToString();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string returnCompanyFromEmail(string mail, string pass)
    {
        List<Companies> w = mongoDbase.getCompanyByEmail(mail);
        
        if (w.Count != 0)
        {
            if (w[0].Password == pass)
            {
                HttpContext.Current.Session.Add("company", w[0]);
                HttpContext.Current.Session.Add("user", null);
                return JsonConvert.SerializeObject(w[0]);
            }
            else
                return badp;
        }
        else
            return badm;
    }

    [System.Web.Services.WebMethod]
    public string returnWorkerFromEmailNoPass(string mail)
    {
        List<Workers> w = mongoDbase.getWorkerByEmail(mail);
        
        if (w.Count != 0)
        {
             return JsonConvert.SerializeObject(w[0]);            
        }
        else
            return "Worker not found with mail!";
        
    }

    [System.Web.Services.WebMethod]
    public string returnCompanyFromEmailNoPass(string mail)
    {
        List<Companies> w = mongoDbase.getCompanyByEmail(mail);

        if (w.Count != 0)
        {
            return JsonConvert.SerializeObject(w[0]);   
        }
        else
            return "Company not found with mail!";
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string enterNewWorkerInDb(string mail, string pass, string name, string last, string check)
    {
        Workers w = new Workers();
        w.FirstName = name;
        w.LastName = last;
        w.Email = mail;
        w.Password = pass;
        w.Checkbox = check;

        var ret = mongoDbase.Create(w);

        if (ret != null)
        {
            HttpContext.Current.Session.Add("user", ret);
            return succ;
        }
        return fail;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string enterNewCompanyInDb(string company, string owner, string type, string location, string mail, string pass, string check)
    {
        Companies c = new Companies();
        c.CompanyName = company;
        c.Owner = owner;
        c.Type = type;
        c.Location = location;
        c.Email = mail;
        c.Password = pass;
        c.Checkbox = check;

        var ret = mongoDbase.CreateCompany(c);

        if (ret != null)
        {
            HttpContext.Current.Session.Add("company", ret);
            return succ;
        }
        return fail;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string updateWorkerInDb(string id, string mail, string pass, string name, string last, string check, string company)
    {
        List<Workers> recvv = mongoDbase.getWorkerById(ObjectId.Parse(id));


        recvv[0].Email = mail;
        recvv[0].Password = pass;
        recvv[0].FirstName = name;
        recvv[0].LastName = last;
        recvv[0].Checkbox = check;

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

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string updateCompanyInDb(string id, string mail, string pass, string name, string owner, string type, string loc, string check)
    {
        List<Companies> recvv = mongoDbase.getCompanyById(ObjectId.Parse(id));


        recvv[0].Email = mail;
        recvv[0].Password = pass;
        recvv[0].Owner = owner;
        recvv[0].Type = type;
        recvv[0].Location = loc;
        recvv[0].Checkbox = check;

        var temp = recvv[0].CompanyName;

        var cId = mongoDbase.getCompanyByName(name);

        Companies res = new Companies();
        string ret = "";

        if (temp != name)
        {
            if (cId.Count == 0)
            {
                recvv[0].CompanyName = name;
                if(recvv[0].Employees.Length != 0)
                {
                    for (int i = 0; i < recvv[0].Employees.Length; i++)
                    {
                        var tempW = mongoDbase.getWorkerById(recvv[0].Employees[i]);
                        tempW[0].CompanyName = name;
                        mongoDbase.updateWorker(tempW[0].Id, tempW[0]);
                    }
                }
                res = mongoDbase.updateCompany(recvv[0].Id, recvv[0]);
            }
            else
            {
                if (recvv[0].Employees.Length != 0)
                {
                    for (int i = 0; i < recvv[0].Employees.Length; i++)
                    {
                        var rets = mongoDbase.removeWorkerFromCompany(recvv[0].Employees[i], recvv[0]);
                        var com = mongoDbase.addWorkerToCompany(recvv[0].Employees[i], cId[0]);
                    }
                }
                ret = mongoDbase.removeCompany(recvv[0].Id);
            }
        }


        if (res != null && ret != "Company deleted!")
        {
            HttpContext.Current.Session.Add("company", res);
            return "Update successfull!";
        }
        return fail;
    }

    [System.Web.Services.WebMethod]
    public List<Workers> retAllWorkersFromCollection()
    {
        List<Workers> w = mongoDbase.GetWorkers();
        return w;
    }

    [System.Web.Services.WebMethod]
    public List<Companies> retAllCompaniesFromCollection()
    {
        List<Companies> c = mongoDbase.GetCompanies();
        return c;
    }

    [System.Web.Services.WebMethod]
    public string retCompanyFromId(ObjectId id)
    {
        List<Companies> c = mongoDbase.getCompanyById(id);

        if (c != null)
        {
            return JsonConvert.SerializeObject(c);
        }
        else
            return "Company with that ID doesn't exist in our registry!";
    }

    [System.Web.Services.WebMethod]
    public string retCompanyFromName(string name)
    {
        List<Companies> c = mongoDbase.getCompanyByName(name);

        if (c.Count != 0)
           return JsonConvert.SerializeObject(c);
        else
           return "Company with that name doesn't exist in our registry!";

    }

    [System.Web.Services.WebMethod]
    public string retWorkerFromName(string name)
    {
        List<Workers> w = mongoDbase.getWorkerByName(name);

        if(w.Count != 0)
           return JsonConvert.SerializeObject(w);
        else
           return "Worker with that name doesn't exist in our registry!";
            
    }

    [System.Web.Services.WebMethod]
    public string retWorkerFromLastName(string name)
    {
        List<Workers> w = mongoDbase.getWorkerByLastName(name);

        if(w.Count != 0)
           return JsonConvert.SerializeObject(w);
        else
           return "Worker with that last name doesn't exist in our registry!";

    }

    [System.Web.Services.WebMethod]
    public string retCompanyWorkers(string name)
    {
        List<Companies> c = mongoDbase.getCompanyByName(name);

        List<ObjectId> objects = new List<ObjectId>();

        for (int i = 0; i < c.Count; i++)
        {
            if (c[i] != null || c[i].CompanyName == name)
            {
                if (c[i].Employees != null)
                {
                    for (int j = 0; j < c[i].Employees.Length; j++)
                    {
                        objects.Add(c[i].Employees[j]);
                    }
                    return JsonConvert.SerializeObject(objects);
                }
            }
            else
                return "Company with that name doesn't exist in our registry!";
        }

        return null;

    }

    [System.Web.Services.WebMethod]
    public string retWorkerFromId(string id)
    {

        List<Workers> c = mongoDbase.getWorkerById(ObjectId.Parse(id));

    
        if (c[0] != null || c[0].Id == ObjectId.Parse(id))
        {
            return JsonConvert.SerializeObject(c);
        }
        else
            return "User with that name doesn't exist in our registry!";

    }

    [System.Web.Services.WebMethod]
    public string deleteWorkerWithId(string id)
    {
        string res = mongoDbase.removeWorker(ObjectId.Parse(id));
        return res;
    }

    [System.Web.Services.WebMethod]
    public string deleteCompanyWithId(string id)
    {
        string res = mongoDbase.removeCompany(ObjectId.Parse(id));
        return res;
    }
}
