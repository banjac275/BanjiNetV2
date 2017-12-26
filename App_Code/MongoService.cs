using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
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
    RavenDataAccess raven;
    string badp = "Bad password, please try again!";
    string badm = "No users were found with this email, please sign up!";
    string succ = "User database entry was a success!";
    string fail = "User database entry failed!";

    public MongoService()
    {
        mongoDbase = new MongoDataAccess();
        raven = new RavenDataAccess();
    }

    //trazi radnika u bazi i proverava da li mu je dobra sifra
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string returnWorkerFromEmail(string mail, string pass)
    {
        Workers w = mongoDbase.getWorkerByEmail(mail);
        //JavaScriptSerializer jserial = new JavaScriptSerializer();
        if (w != null)
        {
            if (w.Password == pass)
            {
                HttpContext.Current.Session.Add("user", w);
                HttpContext.Current.Session.Add("company", null);
                return JsonConvert.SerializeObject(w);
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
        Companies w = mongoDbase.getCompanyByEmail(mail);
        
        if (w != null)
        {
            if (w.Password == pass)
            {
                HttpContext.Current.Session.Add("company", w);
                HttpContext.Current.Session.Add("user", null);
                return JsonConvert.SerializeObject(w);
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
        Workers w = mongoDbase.getWorkerByEmail(mail);
        
        if (w != null)
        {
             return JsonConvert.SerializeObject(w);            
        }
        else
            return "Worker not found with mail!";
        
    }

    [System.Web.Services.WebMethod]
    public string returnCompanyFromEmailNoPass(string mail)
    {
        Companies w = mongoDbase.getCompanyByEmail(mail);

        if (w != null)
        {
            return JsonConvert.SerializeObject(w);   
        }
        else
            return "Company not found with mail!";
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string enterNewWorkerInDb(string mail, string pass, string name, string last)
    {
        Workers w = new Workers();
        w.FirstName = name;
        w.LastName = last;
        w.Email = mail;
        w.Password = pass;

        var ret = mongoDbase.Create(w);

        if (ret != null)
        {
            HttpContext.Current.Session.Add("user", ret);
            return succ;
        }
        return fail;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string enterNewCompanyInDb(string company, string owner, string type, string location, string mail, string pass)
    {
        Companies c = new Companies();
        c.CompanyName = company;
        c.Owner = owner;
        c.Type = type;
        c.Location = location;
        c.Email = mail;
        c.Password = pass;

        var ret = mongoDbase.CreateCompany(c);

        if (ret != null)
        {
            HttpContext.Current.Session.Add("company", ret);
            return succ;
        }
        return fail;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string updateWorkerInDb(string id, string mail, string pass, string name, string last, string company, string previous, string skills, string dbch)
    {
        Workers recvm = mongoDbase.getWorkerById(ObjectId.Parse(id));
        WorkersR recvv = raven.getWorkerByEmail(recvm.Email);

        if (recvm == null)
        {
            Workers wm = new Workers()
            {
                FirstName = name,
                LastName = last,
                Email = mail,
                Password = pass,
                CompanyId = "5a3c3546a2bfccaa6c6a90e1",
                CompanyName = "unemployed"
            };

            var retM = mongoDbase.Create(wm);
            var compmon = mongoDbase.addWorkerToCompany(retM.Id, mongoDbase.getCompanyById(ObjectId.Parse("5a3c3546a2bfccaa6c6a90e1")));
            recvm = retM;
        }

        if (previous == "")
            previous = null;

        if (skills == "")
            skills = null;

        recvv.Email = recvm.Email = mail;
        recvv.Password = recvm.Password = pass;
        recvv.FirstName = recvm.FirstName = name;
        recvv.LastName = recvm.LastName = last;

        if (previous != null)
        {
            List<PrevEmp> pre = new List<PrevEmp>();
            List<PrevEmpM> prem = new List<PrevEmpM>();
            JArray tempp = JArray.Parse(previous);
            if (tempp != null)
            {
                for (int i = 0; i < tempp.Count; i++)
                {
                    PrevEmp emp = new PrevEmp();
                    JToken token = tempp[i];
                    string sol = (string)token["firm"];
                    List<CompaniesR> comp = raven.getCompanyByName(sol);
                    if (comp != null)
                    {
                        emp.FirmName = comp[0].CompanyName;
                        emp.FirmId = comp[0].Id;
                        emp.FormerEmployeeId = Guid.Parse(id);
                        emp.StartTime = (string)token["dates"];
                        emp.EndTime = (string)token["datee"];
                    }

                    if (emp != null)
                        pre.Add(emp);
                    else
                        return fail;

                }

                for (int i = 0; i < tempp.Count; i++)
                {
                    PrevEmpM empm = new PrevEmpM();
                    JToken token = tempp[i];
                    string sol = (string)token["firm"];
                    List<Companies> compm = mongoDbase.getCompanyByName(sol);
                    if (compm != null)
                    {
                        empm.FirmName = compm[0].CompanyName;
                        empm.FirmId = compm[0].Id;
                        empm.FormerEmployeeId = recvm.Id;
                        empm.StartTime = (string)token["dates"];
                        empm.EndTime = (string)token["datee"];
                    }

                    if (empm != null)
                        prem.Add(empm);
                    else
                        return fail;

                }
            }
            else
                return fail;
            recvv.PreviousEmployment = pre;
            recvm.PreviousEmployment = prem;
        }
        else
        {
            recvv.PreviousEmployment = null;
            recvm.PreviousEmployment = null;
        }


        if (skills != null)
        {
            List<string> ski = new List<string>();
            JArray tempps = JArray.Parse(skills);
            if (tempps != null)
            {
                for (int i = 0; i < tempps.Count; i++)
                {
                    JToken token = tempps[i];
                    string sol = (string)token;

                    if (sol != null)
                        ski.Add(sol);
                    else
                        return fail;
                }
            }
            else
                return fail;
            recvv.Skills = ski;
            recvm.Skills = ski;
        }
        else
        {
            recvv.Skills = null;
            recvm.Skills = null;
        }


        var temp = recvv.CompanyName;

        var cId = raven.getCompanyByName(company);
        var cmId = mongoDbase.getCompanyByName(company);

        Changes changeFinal = null;

        if (cId.Count == 0 || cmId.Count == 0)
        {
            return "There is no such company!";
        }

        if (temp != company && temp != null)
        {
            var tempC = raven.getCompanyByName(temp);
            var ret = raven.removeWorkerFromCompany(recvv.Id, tempC[0]);
            var com = raven.addWorkerToCompany(recvv.Id, cId[0]);
            recvv.CompanyId = cId[0].Id.ToString();
            recvv.CompanyName = cId[0].CompanyName;
        }
        else if (temp == null)
        {
            var com = raven.addWorkerToCompany(recvv.Id, cId[0]);
            recvv.CompanyId = cId[0].Id.ToString();
            recvv.CompanyName = cId[0].CompanyName;
        }

        var res = raven.updateWorker(recvv);

        //mongo
        if (temp != company && temp != null)
        {
            var tempC = mongoDbase.getCompanyByName(temp);
            var ret = mongoDbase.removeWorkerFromCompany(recvm.Id, tempC[0]);
            var com = mongoDbase.addWorkerToCompany(recvm.Id, cmId[0]);
            recvm.CompanyId = cmId[0].Id.ToString();
            recvm.CompanyName = cmId[0].CompanyName;
        }
        else if (temp == null)
        {
            var com = mongoDbase.addWorkerToCompany(recvm.Id, cmId[0]);
            recvm.CompanyId = cmId[0].Id.ToString();
            recvm.CompanyName = cmId[0].CompanyName;
        }

        var resm = mongoDbase.updateWorker(recvm);

        if (temp != company || temp == null)
        {
            Changes ch = new Changes()
            {
                Actor1 = res.Id,
                Actor1Collection = "WorkersR",
                Actor1Name = res.FirstName + ' ' + res.LastName,
                Type = " has added as employer ",
                Actor2 = Guid.Parse(res.CompanyId),
                Actor2Name = res.CompanyName,
                Actor2Collection = "CompaniesR",
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            changeFinal = raven.addFriendChange(ch);
        }
        else
        {
            Changes ch = new Changes()
            {
                Actor1 = res.Id,
                Actor1Name = res.FirstName + ' ' + res.LastName,
                Actor1Collection = "WorkersR",
                Type = " has updated profile!",
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            changeFinal = raven.addFriendChange(ch);
        }

        var dbTemplist = raven.getDBPref();

        string dbtemp = null;

        if (dbTemplist.Count != 0)
        {
            for (int i = 0; i < dbTemplist.Count; i++)
            {
                if (dbTemplist[i].MongoId == recvm.Id.ToString() && dbTemplist[i].RavenId == recvv.Id.ToString())
                    dbtemp = dbTemplist[i].DbName;
            }
            if (dbtemp == null)
            {
                DBCheck dbc = new DBCheck()
                {
                    Collection = "worker",
                    DbName = "raven",
                    Mail = mail,
                    Password = pass,
                    MongoId = recvm.Id.ToString(),
                    RavenId = recvv.Id.ToString()
                };

                var dbcRet = raven.setDB(dbc);
            }
        }

        if (dbtemp != null && dbtemp != dbch)
        {
            DBCheck dbc = new DBCheck()
            {
                Collection = "worker",
                DbName = dbch,
                Mail = mail,
                Password = pass,
                MongoId = recvm.Id.ToString(),
                RavenId = recvv.Id.ToString()
            };

            var dbcRet = raven.setDB(dbc);
        }

        if (res != null && resm != null && changeFinal != null)
        {
            if (dbch == "raven")
            {
                HttpContext.Current.Session.Add("userR", res);
                HttpContext.Current.Session.Add("database", "raven");
                return "Update successfull!";
            }
            else if (dbch == "mongo")
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Add("user", resm);
                HttpContext.Current.Session.Add("database", "mongo");
                return "Update successfull!";
            }
        }
        return fail;
    }

    //[WebMethod(EnableSession = true)]
    //public string updateCompanyInDb(string id, string mail, string pass, string name, string owner, string type, string loc)
    //{
    //    Companies recvv = mongoDbase.getCompanyById(ObjectId.Parse(id));


    //    recvv.Email = mail;
    //    recvv.Password = pass;
    //    recvv.Owner = owner;
    //    recvv.Type = type;
    //    recvv.Location = loc;

    //    var temp = recvv.CompanyName;

    //    var cId = mongoDbase.getCompanyByName(name);

    //    Companies res = new Companies();
    //    string ret = "";

    //    if (temp != name)
    //    {
    //        if (cId.Count == 0)
    //        {
    //            recvv.CompanyName = name;
    //            if(recvv.Employees.Count != 0)
    //            {
    //                for (int i = 0; i < recvv.Employees.Count; i++)
    //                {
    //                    var tempW = mongoDbase.getWorkerById(recvv.Employees[i]);
    //                    tempW[0].CompanyName = name;
    //                    mongoDbase.updateWorker(tempW[0].Id, tempW[0]);
    //                }
    //            }
    //            res = mongoDbase.updateCompany(recvv.Id, recvv);
    //        }
    //        else
    //        {
    //            if (recvv.Employees.Count != 0)
    //            {
    //                for (int i = 0; i < recvv.Employees.Count; i++)
    //                {
    //                    var rets = mongoDbase.removeWorkerFromCompany(recvv.Employees[i], recvv);
    //                    var com = mongoDbase.addWorkerToCompany(recvv.Employees[i], cId[0]);
    //                }
    //            }
    //            ret = mongoDbase.removeCompany(recvv.Id);
    //        }
    //    }


    //    if (res != null && ret != "Company deleted!")
    //    {
    //        HttpContext.Current.Session.Add("company", res);
    //        return "Update successfull!";
    //    }
    //    return fail;
    //}

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
        Companies c = mongoDbase.getCompanyById(id);

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
            return "Company not found!";

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
                    for (int j = 0; j < c[i].Employees.Count; j++)
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

        Workers c = mongoDbase.getWorkerById(ObjectId.Parse(id));

    
        if (c != null || c.Id == ObjectId.Parse(id))
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
