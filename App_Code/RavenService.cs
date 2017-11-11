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

        Changes ch = new Changes()
        {
            Actor1 = ret.Id,
            Actor1Name = ret.FirstName + ' ' + ret.LastName,
            Actor1Collection = "WorkersR",
            Type = " is new person that has joined our network!",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var change = raven.addFriendChange(ch);

        if (ret != null && change != null)
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

        Changes ch = new Changes()
        {
            Actor1 = ret.Id,
            Actor1Name = ret.CompanyName,
            Actor1Collection = "CompaniesR",
            Type = " is new company that has joined our network!",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var change = raven.addFriendChange(ch);

        if (ret != null && change != null)
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
    public string updateWorkerInRDb(string id, string mail, string pass, string name, string last, string company, string previous, string skills)
    {
        WorkersR recvv = raven.getWorkerById(Guid.Parse(id));

        if (previous == "")
            previous = null;

        if (skills == "")
            skills = null;

        recvv.Email = mail;
        recvv.Password = pass;
        recvv.FirstName = name;
        recvv.LastName = last;

        if (previous != null)
        {
            List<PrevEmp> pre = new List<PrevEmp>();
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
            }
            else
                return fail;
            recvv.PreviousEmployment = pre;

        }
        else
            recvv.PreviousEmployment = null;

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

        }
        else
            recvv.Skills = null;

        var temp = recvv.CompanyName;

        var cId = raven.getCompanyByName(company);

        Changes changeFinal = null;

        if (cId.Count == 0)
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

        if (res != null && changeFinal != null)
        {
            HttpContext.Current.Session.Add("userR", res);
            return "Update successfull!";
        }
        return fail;
    }

    //apdejtovanje profila radnika
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string updateCompanyInRDb(string id, string mail, string pass, string name, string owner, string type, string loc)
    {
        CompaniesR recvv = raven.getCompanyById(Guid.Parse(id));

        recvv.Email = mail;
        recvv.Password = pass;
        //recvv.CompanyName = name;
        recvv.Owner = owner;
        recvv.Location = loc;

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


        Changes ch = new Changes()
        {
            Actor1 = res.Id,
            Actor1Name = res.CompanyName,
            Actor1Collection = "CompaniesR",
            Type = " has updated profile!",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        changeFinal = raven.addFriendChange(ch);


        if (res != null && changeFinal != null)
        {
            HttpContext.Current.Session.Add("companyR", res);
            return "Update successfull!";
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

        if(w != null)
        {
            if(w.Friends != null)
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
            }

            if(w.CompanyName != null)
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
            }
        }

        var res = raven.deleteWorker(w);

        Changes ch = new Changes()
        {
            Actor1 = w.Id,
            Actor1Name = w.FirstName + ' ' + w.LastName,
            Actor1Collection = "WorkersR",
            Type = " has deleted profile from the network!",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        Changes changeFinal = raven.addFriendChange(ch);

        if (res != null && changeFinal != null)
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

        if(c != null)
        {
            if(c.Employees != null)
            {
                for(int i = 0; i < c.Employees.Count; i++)
                {
                    var temp = raven.getWorkerById(c.Employees[i]);
                    temp.CompanyId = null;
                    temp.CompanyName = null;
                    raven.updateWorker(temp);
                }
            }
        }

        var res = raven.deleteCompany(c);

        Changes ch = new Changes()
        {
            Actor1 = c.Id,
            Actor1Name = c.CompanyName,
            Actor1Collection = "CompaniesR",
            Type = " has deleted profile from the network!",
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        Changes changeFinal = raven.addFriendChange(ch);

        if (res != null && changeFinal != null)
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
}
