using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raven.Client;
using MongoDB.Bson;

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
    MongoDataAccess mongor;
    string badp = "Bad password, please try again!";
    string badm = "No users were found with this email, please sign up!";
    string succ = "User database entry was a success!";
    string fail = "User database entry failed!";

    public RavenService()
    {
        raven = new RavenDataAccess();
        mongor = new MongoDataAccess();
    }

    //upis
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string enterNewWorkerInRDb(string mail, string pass, string name, string last)
    {
        //upis novog radnika u raven bazu
        WorkersR w = new WorkersR()
        {
            FirstName = name,
            LastName = last,
            Email = mail,
            Password = pass,
            CompanyId = "579ac770-de1b-4837-b6eb-1ff8bca0ec20",
            CompanyName = "unemployed"
        };

        var ret = raven.Create(w);
        //dodavanje novoupisanog radnike medju redove nezaposlenih
        var comp = raven.addWorkerToCompany(w.Id, raven.getCompanyById(Guid.Parse("579ac770-de1b-4837-b6eb-1ff8bca0ec20")));

        //upis u mongo za nove radnike i uzimanje object id novoupisanog radnika
        Workers wm = new Workers()
        {
            FirstName = name,
            LastName = last,
            Email = mail,
            Password = pass,
            CompanyId = "5a3c3546a2bfccaa6c6a90e1",
            CompanyName = "unemployed"
        };

        var retM = mongor.Create(wm);
        var compmon = mongor.addWorkerToCompany(retM.Id, mongor.getCompanyById(ObjectId.Parse("5a3c3546a2bfccaa6c6a90e1")));

        DBCheck dbc = new DBCheck()
        {
            Collection = "worker",
            DbName = "raven",
            Mail = mail,
            Password = pass,
            MongoId = retM.Id.ToString(),
            RavenId = w.Id.ToString()
        };

        var dbcRet = raven.setDB(dbc);

        Changes ch = new Changes()
        {
            Actor1 = ret.Id,
            Actor1Name = ret.FirstName + ' ' + ret.LastName,
            Actor1Collection = "WorkersR",
            Type = " is new person that has joined our network!",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var change = raven.addFriendChange(ch);

        if (ret != null && change != null && comp != null && dbcRet != null)
        {
            HttpContext.Current.Session.Add("userR", ret);
            HttpContext.Current.Session.Add("database", dbc.DbName);
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

        //mongo
        Companies cm = new Companies()
        {
            CompanyName = company,
            Owner = owner,
            Type = type,
            Location = location,
            Email = mail,
            Password = pass
        };

        var retm = mongor.CreateCompany(cm);

        DBCheck dbc = new DBCheck()
        {
            Collection = "company",
            DbName = "raven",
            Mail = mail,
            Password = pass,
            MongoId = retm.Id.ToString(),
            RavenId = c.Id.ToString()
        };

        var dbcRet = raven.setDB(dbc);

        Changes ch = new Changes()
        {
            Actor1 = ret.Id,
            Actor1Name = ret.CompanyName,
            Actor1Collection = "CompaniesR",
            Type = " is new company that has joined our network!",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var change = raven.addFriendChange(ch);

        if (ret != null && change != null && retm != null && dbcRet != null)
        {
            HttpContext.Current.Session.Add("companyR", ret);
            HttpContext.Current.Session.Add("database", dbc.DbName);
            return succ;
        }
        return fail;
    }

    //trazenje radnika po mejlu
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string returnWorkerFromEmailR(string mail, string pass)
    {
        
        WorkersR w = raven.getWorkerByEmail(mail);
       
        if (w != null)
        {
            if (w.Password == pass && w.Email == mail)
            {
                HttpContext.Current.Session.Add("userR", w);
                HttpContext.Current.Session.Add("database", "raven");
                HttpContext.Current.Session.Add("companyR", null);
                return JsonConvert.SerializeObject(w);
            }
            else
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
                    HttpContext.Current.Session.Add("database", "raven");
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
            return "Company not found!";

    }

    //trazenje firme po imenu za search samo
    [System.Web.Services.WebMethod]
    public string retCompanyFromName(string name)
    {
        List<CompaniesR> c = raven.getCompanyByNameS(name);

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

    //apdejtovanje profila radnika
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string updateWorkerInRDb(string id, string mail, string pass, string name, string last, string company, string previous, string skills, string dbch)
    {
        WorkersR recvv = raven.getWorkerById(Guid.Parse(id));
        Workers recvm = mongor.getWorkerByEmail(recvv.Email);

        if(recvm == null)
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

            var retM = mongor.Create(wm);
            var compmon = mongor.addWorkerToCompany(retM.Id, mongor.getCompanyById(ObjectId.Parse("5a3c3546a2bfccaa6c6a90e1")));
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
                    List<Companies> compm = mongor.getCompanyByName(sol);
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
        var cmId = mongor.getCompanyByName(company);

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
        else if(temp == null)
        {
            var com = raven.addWorkerToCompany(recvv.Id, cId[0]);
            recvv.CompanyId = cId[0].Id.ToString();
            recvv.CompanyName = cId[0].CompanyName;
        }

        var res = raven.updateWorker(recvv);

        //mongo
        if (temp != company && temp != null)
        {
            var tempC = mongor.getCompanyByName(temp);
            var ret = mongor.removeWorkerFromCompany(recvm.Id, tempC[0]);
            var com = mongor.addWorkerToCompany(recvm.Id, cmId[0]);
            recvm.CompanyId = cmId[0].Id.ToString();
            recvm.CompanyName = cmId[0].CompanyName;
        }
        else if (temp == null)
        {
            var com = mongor.addWorkerToCompany(recvm.Id, cmId[0]);
            recvm.CompanyId = cmId[0].Id.ToString();
            recvm.CompanyName = cmId[0].CompanyName;
        }

        var resm = mongor.updateWorker(recvm);

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

        if(dbTemplist.Count != 0)
        {
            for(int i = 0; i < dbTemplist.Count; i++)
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
            else if(dbch == "mongo")
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Add("user", resm);
                HttpContext.Current.Session.Add("database", "mongo");
                return "Update successfull!";
            }
        }
        return fail;
    }

    //apdejtovanje profila kompanije
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string updateCompanyInRDb(string id, string mail, string pass, string name, string owner, string type, string loc, string dbch)
    {
        CompaniesR recvv = raven.getCompanyById(Guid.Parse(id));
        Companies recvm = mongor.getCompanyByEmail(mail);

        if(recvm == null)
        {
            Companies cm = new Companies()
            {
                CompanyName = name,
                Owner = owner,
                Type = type,
                Location = loc,
                Email = mail,
                Password = pass
            };

            var retm = mongor.CreateCompany(cm);
            recvm = retm;
        }

        recvv.Email = recvm.Email = mail;
        recvv.Password = recvm.Password = pass;
        recvv.Owner = recvm.Owner = owner;
        recvv.Location = recvm.Location = loc;

        var temp = recvv.CompanyName;

        var cId = raven.getCompanyByName(name);

        Changes changeFinal = null;

        if (cId.Count == 0)
        {
            return "There is no such company!";
        }

        if(cId.Count != 0 && cId[0].CompanyName == temp && Guid.Parse(id) == cId[0].Id)
        {
            return "Company is a duplicate!";
        }

        if (temp != name && temp != null)
        {
            var tempC = raven.getCompanyByName(temp);
            for(int i = 0; i<tempC[0].Employees.Count; i++)
            {
                var tempE = raven.getWorkerById(tempC[0].Employees[i]);
                tempE.CompanyName = name;
                raven.updateWorker(tempE);
            }
            recvv.CompanyName = name;
        }
        else if (temp == null)
        {
            recvv.CompanyName = name;
        }

        var res = raven.updateCompany(recvv);

        //mongo
        if (temp != name && temp != null)
        {
            var tempC = mongor.getCompanyByName(temp);
            for (int i = 0; i < tempC[0].Employees.Count; i++)
            {
                var tempE = mongor.getWorkerById(tempC[0].Employees[i]);
                tempE.CompanyName = name;
                mongor.updateWorker(tempE);
            }
            recvm.CompanyName = name;
        }
        else if (temp == null)
        {
            recvm.CompanyName = name;
        }

        var resm = raven.updateCompany(recvv);

        Changes ch = new Changes()
        {
            Actor1 = res.Id,
            Actor1Name = res.CompanyName,
            Actor1Collection = "CompaniesR",
            Type = " has updated profile!",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        changeFinal = raven.addFriendChange(ch);

        var dbTemplist = raven.getDBPref();

        string dbtemp = null;

        if (dbTemplist.Count != 0)
        {
            for (int i = 0; i < dbTemplist.Count; i++)
            {
                if (dbTemplist[i].MongoId == recvm.Id.ToString() && dbTemplist[i].RavenId == recvv.Id.ToString())
                    dbtemp = dbTemplist[i].DbName;
            }
            if(dbtemp == null)
            {
                DBCheck dbc = new DBCheck()
                {
                    Collection = "company",
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
                Collection = "company",
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
                HttpContext.Current.Session.Add("companyR", res);
                HttpContext.Current.Session.Add("database", "raven");
                return "Update successfull!";
            }
            else if (dbch == "mongo")
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Add("company", resm);
                HttpContext.Current.Session.Add("database", "mongo");
                return "Update successfull!";
            }
        }
        return fail;
    }

    [System.Web.Services.WebMethod]
    public string retWorkerFromIdR(string id)
    {

        WorkersR c = raven.getWorkerById(Guid.Parse(id));


        if (c != null)
        {
            return JsonConvert.SerializeObject(c);
        }
        else
            return "Worker not found!";

    }

    [System.Web.Services.WebMethod]
    public string deleteWorkerWithId(string id)
    {
        WorkersR w = raven.getWorkerById(Guid.Parse(id));
        Workers wm = mongor.getWorkerByEmail(w.Email);

        if(w != null && wm != null)
        {
            if(w.Friends != null && wm.Friends != null)
            {
                for(int i = 0; i < w.Friends.Count; i++)
                {
                    var temp = raven.getWorkerById(w.Friends[i]);
                    List<Guid> tpr = new List<Guid>();
                    for(int j = 0; j < temp.Friends.Count; j++)
                    {
                        if (Guid.Parse(id) != temp.Friends[j])
                            tpr.Add(temp.Friends[j]);
                    }
                    temp.Friends = tpr;
                    raven.updateWorker(temp);
                }

                //mongo
                for (int i = 0; i < wm.Friends.Count; i++)
                {
                    var temp = mongor.getWorkerById(wm.Friends[i]);
                    List<ObjectId> tpr = new List<ObjectId>();
                    for (int j = 0; j < temp.Friends.Count; j++)
                    {
                        if (ObjectId.Parse(id) != temp.Friends[j])
                            tpr.Add(temp.Friends[j]);
                    }
                    temp.Friends = tpr;
                    mongor.updateWorker(temp);
                }
            }

            if(w.CompanyName != null && wm.CompanyName != null)
            {
                CompaniesR c = raven.getCompanyById(Guid.Parse(w.CompanyId));
                List<Guid> tpr = new List<Guid>();
                for(int j = 0; j < c.Employees.Count; j++)
                {
                    if (Guid.Parse(id) != c.Employees[j])
                        tpr.Add(c.Employees[j]);
                }
                c.Employees = tpr;
                raven.updateCompany(c);

                //mongo
                Companies cm = mongor.getCompanyById(ObjectId.Parse(wm.CompanyId));
                List<ObjectId> tpm = new List<ObjectId>();
                for (int j = 0; j < cm.Employees.Count; j++)
                {
                    if (ObjectId.Parse(id) != cm.Employees[j])
                        tpm.Add(cm.Employees[j]);
                }
                cm.Employees = tpm;
                mongor.updateCompany(cm);
            }
        }

        var res = raven.deleteWorker(w);
        var resm = mongor.removeWorker(wm.Id);

        Changes ch = new Changes()
        {
            Actor1 = w.Id,
            Actor1Name = w.FirstName + ' ' + w.LastName,
            Actor1Collection = "WorkersR",
            Type = " has deleted profile from the network!",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        Changes changeFinal = raven.addFriendChange(ch);

        if (res != null && resm != null && changeFinal != null)
        {
            return "Worker deleted!";
        }
        else
            return "Worker not found!";

    }

    [System.Web.Services.WebMethod]
    public string deleteCompanyWithId(string id)
    {
        CompaniesR c = raven.getCompanyById(Guid.Parse(id));
        Companies cm = mongor.getCompanyByEmail(c.Email);

        if (c != null && cm != null)
        {
            if(c.Employees != null && cm.Employees != null)
            {
                for(int i = 0; i < c.Employees.Count; i++)
                {
                    var temp = raven.getWorkerById(c.Employees[i]);
                    temp.CompanyId = null;
                    temp.CompanyName = null;
                    raven.updateWorker(temp);
                }

                //mongo
                for (int i = 0; i < cm.Employees.Count; i++)
                {
                    var temp = mongor.getWorkerById(cm.Employees[i]);
                    temp.CompanyId = null;
                    temp.CompanyName = null;
                    mongor.updateWorker(temp);
                }
            }
        }

        var res = raven.deleteCompany(c);
        var resm = mongor.removeCompany(cm.Id);

        Changes ch = new Changes()
        {
            Actor1 = c.Id,
            Actor1Name = c.CompanyName,
            Actor1Collection = "CompaniesR",
            Type = " has deleted profile from the network!",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        Changes changeFinal = raven.addFriendChange(ch);

        if (res != null && resm != null && changeFinal != null)
        {
            return "Company deleted!";
        }
        else
            return "Company not found!";

    }

    [System.Web.Services.WebMethod]
    public string retCompanyFromIdR(string id)
    {

        CompaniesR c = raven.getCompanyById(Guid.Parse(id));


        if (c != null)
        {
            return JsonConvert.SerializeObject(c);
        }
        else
            return "Company not found!";

    }

    [System.Web.Services.WebMethod]
    public List<WorkersR> retAllWorkersFromCollectionR()
    {
        List<WorkersR> w = raven.GetWorkers();
        return w;
    }

    [System.Web.Services.WebMethod]
    public List<CompaniesR> retAllCompaniesFromCollectionR()
    {
        List<CompaniesR> c = raven.GetCompanies();
        return c;
    }

    //dodavanje prijatelja
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string addFriendR(string id1, string id2)
    {
        WorkersR friend = raven.getWorkerById(Guid.Parse(id1));
        WorkersR user = raven.getWorkerById(Guid.Parse(id2));

        WorkersR tempW1, tempW2;

        //mongo
        Workers friendm = mongor.getWorkerByEmail(friend.Email);
        Workers userm = mongor.getWorkerByEmail(user.Email);

        Workers tempWM1, tempWM2;

        if(friend.Friends == null)
        {
            friend.Friends = new List<Guid>();
            friend.Friends.Add(Guid.Parse(id2));
            tempW1 = raven.updateWorker(friend);
        }
        else
        {
            friend.Friends.Add(Guid.Parse(id2));
            tempW1 = raven.updateWorker(friend);
        }

        if (user.Friends == null)
        {
            user.Friends = new List<Guid>();
            user.Friends.Add(Guid.Parse(id1));
            tempW2 = raven.updateWorker(user);
        }
        else
        {
            user.Friends.Add(Guid.Parse(id1));
            tempW2 = raven.updateWorker(user);
        }

        //mongo
        if (friendm.Friends == null)
        {
            friendm.Friends = new List<ObjectId>();
            friendm.Friends.Add(userm.Id);
            tempWM1 = mongor.updateWorker(friendm);
        }
        else
        {
            friendm.Friends.Add(userm.Id);
            tempWM1 = mongor.updateWorker(friendm);
        }

        if (userm.Friends == null)
        {
            userm.Friends = new List<ObjectId>();
            userm.Friends.Add(friendm.Id);
            tempWM2 = mongor.updateWorker(userm);
        }
        else
        {
            userm.Friends.Add(friendm.Id);
            tempWM2 = mongor.updateWorker(userm);
        }

        Changes ch = new Changes()
        {
            Actor1 = Guid.Parse(id1),
            Actor1Name = friend.FirstName + ' ' + friend.LastName,
            Actor1Collection = "WorkersR",
            Actor2 = Guid.Parse(id2),
            Actor2Name = user.FirstName + ' ' + user.LastName,
            Actor2Collection = "WorkersR",
            Type = " is friends with ",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var change = raven.addFriendChange(ch);

        if (tempW2 != null && change != null)
        {
            HttpContext.Current.Session.Add("workerR", id1);
            HttpContext.Current.Session.Add("userR", tempW2);
            return "Update successfull!";
        }
        return fail;
    }

    //brisanje prijatelja
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string removeFriendR(string id1, string id2)
    {
        WorkersR friend = raven.getWorkerById(Guid.Parse(id1));
        WorkersR user = raven.getWorkerById(Guid.Parse(id2));

        WorkersR tempW1, tempW2;

        //mongo
        Workers friendm = mongor.getWorkerByEmail(friend.Email);
        Workers userm = mongor.getWorkerByEmail(user.Email);

        Workers tempWM1, tempWM2;

        if (friend.Friends != null)
        {
            List<Guid> temp = new List<Guid>();
            for(int i = 0; i < friend.Friends.Count; i++)
            {
                if (friend.Friends[i] != Guid.Parse(id2))
                    temp.Add(friend.Friends[i]);
            }
            friend.Friends = temp;
            tempW1 = raven.updateWorker(friend);
        }
        else
        {
            return "You are making an invalid action!";
        }

        if (user.Friends != null)
        {
            List<Guid> temp = new List<Guid>();
            for (int i = 0; i < user.Friends.Count; i++)
            {
                if (user.Friends[i] != Guid.Parse(id1))
                    temp.Add(user.Friends[i]);
            }
            user.Friends = temp;
            tempW2 = raven.updateWorker(user);
        }
        else
        {
            return "You are making an invalid action!";
        }

        //mongo
        if (friendm.Friends != null)
        {
            List<ObjectId> temp = new List<ObjectId>();
            for (int i = 0; i < friendm.Friends.Count; i++)
            {
                if (friendm.Friends[i] != userm.Id)
                    temp.Add(friendm.Friends[i]);
            }
            friendm.Friends = temp;
            tempWM1 = mongor.updateWorker(friendm);
        }
        else
        {
            return "You are making an invalid action!";
        }

        if (userm.Friends != null)
        {
            List<ObjectId> temp = new List<ObjectId>();
            for (int i = 0; i < userm.Friends.Count; i++)
            {
                if (userm.Friends[i] != friendm.Id)
                    temp.Add(userm.Friends[i]);
            }
            userm.Friends = temp;
            tempWM2 = mongor.updateWorker(userm);
        }
        else
        {
            return "You are making an invalid action!";
        }

        Changes ch = new Changes()
        {
            Actor1 = Guid.Parse(id1),
            Actor1Name = friend.FirstName + ' ' + friend.LastName,
            Actor1Collection = "WorkersR",
            Actor2 = Guid.Parse(id2),
            Actor2Name = user.FirstName + ' ' + user.LastName,
            Actor2Collection = "WorkersR",
            Type = " is no longer friends with ",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var change = raven.addFriendChange(ch);

        if (tempW2 != null && change != null)
        {
            HttpContext.Current.Session.Add("workerR", id1);
            HttpContext.Current.Session.Add("userR", tempW2);
            return "Update successfull!";
        }
        return fail;
    }

    [System.Web.Services.WebMethod]
    public string retWorkerFromName(string name)
    {
        List<WorkersR> w = raven.getWorkerByName(name);

        if (w.Count != 0)
            return JsonConvert.SerializeObject(w);
        else
            return "Worker not found!";

    }

    [System.Web.Services.WebMethod]
    public string returnWorkerFromEmailNoPass(string name)
    {
        List<WorkersR> w = raven.getWorkerByEmailS(name);

        if (w.Count != 0)
        {
            return JsonConvert.SerializeObject(w);
        }
        else
            return "Worker not found!";

    }

    [System.Web.Services.WebMethod]
    public string retWorkerFromLastName(string name)
    {
        List<WorkersR> w = raven.getWorkerByLastNameS(name);

        if (w.Count != 0)
            return JsonConvert.SerializeObject(w);
        else
            return "Worker not found!";

    }

    [System.Web.Services.WebMethod]
    public string retWorkerWithSkill(string name)
    {
        List<WorkersR> w = raven.getWorkerWithSkillS(name);

        if (w != null)
            return JsonConvert.SerializeObject(w);
        else
            return "Worker not found!";

    }

    [System.Web.Services.WebMethod]
    public string returnCompanyFromEmailNoPass(string name)
    {
        List<CompaniesR> c = raven.getCompanyByEmailS(name);

        if (c.Count != 0)
        {
            return JsonConvert.SerializeObject(c);
        }
        else
            return "Company not found!";
    }

    [System.Web.Services.WebMethod]
    public List<Changes> retAllChanges()
    {
        List<Changes> c = raven.getChanges();
        return c;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string checkDBs(string mail, string pass)
    {
        List<DBCheck> dbc = raven.getDBPref();
        if(dbc.Count != 0)
        {
            for(int i = 0; i < dbc.Count; i++)
            {
                if (dbc[i].Mail == mail && dbc[i].Password == pass)
                {
                    HttpContext.Current.Session.Add("database", dbc[i].DbName);
                    return dbc[i].DbName;
                }
            }
        }
        return "raven";
    }

}
