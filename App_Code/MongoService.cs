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
                HttpContext.Current.Session.Add("database", "mongo");
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
                HttpContext.Current.Session.Add("database", "mongo");
                HttpContext.Current.Session.Add("user", null);
                return JsonConvert.SerializeObject(w);
            }
            else
                return badp;
        }
        else
            return badm;
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
                        emp.FormerEmployeeId = recvv.Id;
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
                HttpContext.Current.Session.Add("user", null);
                HttpContext.Current.Session.Add("database", "raven");
                return "Update successfull!";
            }
            else if (dbch == "mongo")
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Add("user", resm);
                HttpContext.Current.Session.Add("userR", null);
                HttpContext.Current.Session.Add("database", "mongo");
                return "Update successfull!";
            }
        }
        return fail;
    }

    [WebMethod(EnableSession = true)]
    public string updateCompanyInDb(string id, string mail, string pass, string name, string owner, string type, string loc, string dbch)
    {
        Companies recvm = mongoDbase.getCompanyById(ObjectId.Parse(id));
        CompaniesR recvv = raven.getCompanyByEmail(recvm.Email);        

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

        if (temp != name && temp != null)
        {
            var tempC = raven.getCompanyByName(temp);
            for (int i = 0; i < tempC[0].Employees.Count; i++)
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
            var tempC = mongoDbase.getCompanyByName(temp);
            for (int i = 0; i < tempC[0].Employees.Count; i++)
            {
                var tempE = mongoDbase.getWorkerById(tempC[0].Employees[i]);
                tempE.CompanyName = name;
                mongoDbase.updateWorker(tempE);
            }
            recvm.CompanyName = name;
        }
        else if (temp == null)
        {
            recvm.CompanyName = name;
        }

        var resm = mongoDbase.updateCompany(recvm);

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
            if (dbtemp == null)
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
                HttpContext.Current.Session.Clear();
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

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string retAllWorkersFromCollection()
    {
        List<Workers> w = mongoDbase.GetWorkers();
        List<string> temp = new List<string>();
        if(w.Count != 0)
            return JsonConvert.SerializeObject(w);
        else
            return null;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string retAllCompaniesFromCollection()
    {
        List<Companies> c = mongoDbase.GetCompanies();
        List<string> temp = new List<string>();
        if (c.Count != 0)
            return JsonConvert.SerializeObject(c);
        else
            return null;
    }

    [System.Web.Services.WebMethod]
    public string retCompanyFromId(string id)
    {
        Companies c = mongoDbase.getCompanyById(ObjectId.Parse(id));
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
        List<Workers> w = mongoDbase.getWorkerByNameS(name);
        if (w.Count != 0)
           return JsonConvert.SerializeObject(w);
        else
           return "Worker with that name doesn't exist in our registry!";
            
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
        Workers wm = mongoDbase.getWorkerById(ObjectId.Parse(id));
        WorkersR w = raven.getWorkerByEmail(wm.Email);        

        if (w != null && wm != null)
        {
            if (w.Friends != null && wm.Friends != null)
            {
                for (int i = 0; i < w.Friends.Count; i++)
                {
                    var temp = raven.getWorkerById(w.Friends[i]);
                    List<Guid> tpr = new List<Guid>();
                    for (int j = 0; j < temp.Friends.Count; j++)
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
                    var temp = mongoDbase.getWorkerById(wm.Friends[i]);
                    List<ObjectId> tpr = new List<ObjectId>();
                    for (int j = 0; j < temp.Friends.Count; j++)
                    {
                        if (ObjectId.Parse(id) != temp.Friends[j])
                            tpr.Add(temp.Friends[j]);
                    }
                    temp.Friends = tpr;
                    mongoDbase.updateWorker(temp);
                }
            }

            if (w.CompanyName != null && wm.CompanyName != null)
            {
                CompaniesR c = raven.getCompanyById(Guid.Parse(w.CompanyId));
                List<Guid> tpr = new List<Guid>();
                for (int j = 0; j < c.Employees.Count; j++)
                {
                    if (Guid.Parse(id) != c.Employees[j])
                        tpr.Add(c.Employees[j]);
                }
                c.Employees = tpr;
                raven.updateCompany(c);

                //mongo
                Companies cm = mongoDbase.getCompanyById(ObjectId.Parse(wm.CompanyId));
                List<ObjectId> tpm = new List<ObjectId>();
                for (int j = 0; j < cm.Employees.Count; j++)
                {
                    if (ObjectId.Parse(id) != cm.Employees[j])
                        tpm.Add(cm.Employees[j]);
                }
                cm.Employees = tpm;
                mongoDbase.updateCompany(cm);
            }
        }

        var res = raven.deleteWorker(w);
        var resm = mongoDbase.removeWorker(wm.Id);

        var resdb = raven.deleteDBprefEntry(w.Id.ToString(), w.Email, w.Password);

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
        Companies cm = mongoDbase.getCompanyById(ObjectId.Parse(id));
        CompaniesR c = raven.getCompanyByEmail(cm.Email);        

        if (c != null && cm != null)
        {
            if (c.Employees != null && cm.Employees != null)
            {
                for (int i = 0; i < c.Employees.Count; i++)
                {
                    var temp = raven.getWorkerById(c.Employees[i]);
                    temp.CompanyId = null;
                    temp.CompanyName = null;
                    raven.updateWorker(temp);
                }

                //mongo
                for (int i = 0; i < cm.Employees.Count; i++)
                {
                    var temp = mongoDbase.getWorkerById(cm.Employees[i]);
                    temp.CompanyId = null;
                    temp.CompanyName = null;
                    mongoDbase.updateWorker(temp);
                }
            }
        }

        var res = raven.deleteCompany(c);
        var resm = mongoDbase.removeCompany(cm.Id);

        var resdb = raven.deleteDBprefEntry(c.Id.ToString(), c.Email, c.Password);

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

    //dodavanje prijatelja
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string addFriend(string id1, string id2)
    {
        //mongo
        Workers friendm = mongoDbase.getWorkerById(ObjectId.Parse(id1));
        Workers userm = mongoDbase.getWorkerById(ObjectId.Parse(id2));

        Workers tempWM1, tempWM2;

        WorkersR friend = raven.getWorkerByEmail(friendm.Email);
        WorkersR user = raven.getWorkerByEmail(userm.Email);

        WorkersR tempW1, tempW2;        

        if (friend.Friends == null)
        {
            friend.Friends = new List<Guid>();
            friend.Friends.Add(user.Id);
            tempW1 = raven.updateWorker(friend);
        }
        else
        {
            friend.Friends.Add(user.Id);
            tempW1 = raven.updateWorker(friend);
        }

        if (user.Friends == null)
        {
            user.Friends = new List<Guid>();
            user.Friends.Add(friend.Id);
            tempW2 = raven.updateWorker(user);
        }
        else
        {
            user.Friends.Add(friend.Id);
            tempW2 = raven.updateWorker(user);
        }

        //mongo
        if (friendm.Friends == null)
        {
            friendm.Friends = new List<ObjectId>();
            friendm.Friends.Add(ObjectId.Parse(id2));
            tempWM1 = mongoDbase.updateWorker(friendm);
        }
        else
        {
            friendm.Friends.Add(ObjectId.Parse(id2));
            tempWM1 = mongoDbase.updateWorker(friendm);
        }

        if (userm.Friends == null)
        {
            userm.Friends = new List<ObjectId>();
            userm.Friends.Add(ObjectId.Parse(id1));
            tempWM2 = mongoDbase.updateWorker(userm);
        }
        else
        {
            userm.Friends.Add(ObjectId.Parse(id1));
            tempWM2 = mongoDbase.updateWorker(userm);
        }

        Changes ch = new Changes()
        {
            Actor1 = friend.Id,
            Actor1Name = friend.FirstName + ' ' + friend.LastName,
            Actor1Collection = "WorkersR",
            Actor2 = user.Id,
            Actor2Name = user.FirstName + ' ' + user.LastName,
            Actor2Collection = "WorkersR",
            Type = " is friends with ",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var change = raven.addFriendChange(ch);

        if (tempW2 != null && change != null)
        {
            HttpContext.Current.Session.Add("worker", id1);
            HttpContext.Current.Session.Add("user", tempWM2);
            return "Update successfull!";
        }
        return fail;
    }

    //brisanje prijatelja
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string removeFriend(string id1, string id2)
    {
        //mongo
        Workers friendm = mongoDbase.getWorkerById(ObjectId.Parse(id1));
        Workers userm = mongoDbase.getWorkerById(ObjectId.Parse(id2));

        Workers tempWM1, tempWM2;

        WorkersR friend = raven.getWorkerByEmail(friendm.Email);
        WorkersR user = raven.getWorkerByEmail(userm.Email);

        WorkersR tempW1, tempW2;

        if (friend.Friends != null)
        {
            List<Guid> temp = new List<Guid>();
            for (int i = 0; i < friend.Friends.Count; i++)
            {
                if (friend.Friends[i] != user.Id)
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
                if (user.Friends[i] != friend.Id)
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
            tempWM1 = mongoDbase.updateWorker(friendm);
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
            tempWM2 = mongoDbase.updateWorker(userm);
        }
        else
        {
            return "You are making an invalid action!";
        }

        Changes ch = new Changes()
        {
            Actor1 = friend.Id,
            Actor1Name = friend.FirstName + ' ' + friend.LastName,
            Actor1Collection = "WorkersR",
            Actor2 = user.Id,
            Actor2Name = user.FirstName + ' ' + user.LastName,
            Actor2Collection = "WorkersR",
            Type = " is no longer friends with ",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var change = raven.addFriendChange(ch);

        if (tempW2 != null && change != null)
        {
            HttpContext.Current.Session.Add("worker", id1);
            HttpContext.Current.Session.Add("user", tempWM2);
            return "Update successfull!";
        }
        return fail;
    }

    //za search
    [System.Web.Services.WebMethod]
    public string retWorkerWithSkill(string name)
    {
        List<Workers> w = mongoDbase.getWorkerWithSkillS(name);

        if (w != null)
            return JsonConvert.SerializeObject(w);
        else
            return "Worker not found!";

    }

    [System.Web.Services.WebMethod]
    public string retCompanyFromNameSrc(string name)
    {
        List<Companies> c = mongoDbase.getCompanyByNameS(name);

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
    public string retWorkerFromNameSrc(string name)
    {
        List<Workers> w = mongoDbase.getWorkerByNameS(name);

        if (w.Count != 0)
            return JsonConvert.SerializeObject(w);
        else
            return "Worker not found!";

    }

    [System.Web.Services.WebMethod]
    public string retWorkerFromLastNameSrc(string name)
    {
        List<Workers> w = mongoDbase.getWorkerByLastNameS(name);
        
        if (w.Count != 0)
            return JsonConvert.SerializeObject(w);
        else
            return "Worker not found!";

    }

    [System.Web.Services.WebMethod]
    public string returnWorkerFromEmailNoPassSrc(string name)
    {
        List<Workers> w = mongoDbase.getWorkerByEmailS(name);

        if (w.Count != 0)
        {
            return JsonConvert.SerializeObject(w);
        }
        else
            return "Worker not found!";

    }

    [System.Web.Services.WebMethod]
    public string returnCompanyFromEmailNoPassSrc(string name)
    {
        List<Companies> c = mongoDbase.getCompanyByEmailS(name);

        if (c.Count != 0)
        {
            return JsonConvert.SerializeObject(c);
        }
        else
            return "Company not found!";
    }
}
