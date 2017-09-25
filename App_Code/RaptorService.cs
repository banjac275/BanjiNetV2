using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using RaptorDB;
using RaptorDB.Common;
using RaptorDB.Views;
using System.Threading.Tasks;

/// <summary>
/// Summary description for RaptorService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
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

        if (w.Count != 0)
        {
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

    [System.Web.Services.WebMethod]
    public string retWorkerFromIdR(string id)
    {

        WorkersR c = raptor.getWorkerById(Guid.Parse(id));


        if (c != null || c.Id == Guid.Parse(id))
        {
            return JsonConvert.SerializeObject(c);
        }
        else
            return "User with that name doesn't exist in our registry!";

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string updateWorkerInRDb(string id, string mail, string pass, string name, string last, string check, string company)
    {
        WorkersR recvv = raptor.getWorkerById(Guid.Parse(id));

        //recvv.CompanyName = company;
        recvv.FirstName = name;
        recvv.LastName = last;
        recvv.Email = mail;
        recvv.Password = pass;
        recvv.Checkbox = check;

        var temp = recvv.CompanyName;

        var cId = raptor.getCompanyByName(company);
        Guid compRet;

        if (cId == null)
        {
            return "There is no such company!";
        }
        else
            compRet = cId.Id;

        if (temp != company && temp != null)
        {
            //nalazi kompaniju sa sve radnicima preko id
            var tempC = raptor.getCompanyByName(temp);
            var tempCId = raptor.getCompanyById(tempC.Id);
            List<string> radnici = new List<string>();
            var broken = tempCId.Employees.Split(',');
            for (int i = 0; i < broken.Length; i++)
            {
                if (Guid.Parse(broken[i]) != recvv.Id)
                    radnici.Add(broken[i]);
            }
            var rez = radnici.ToArray();
            tempCId.Employees = string.Join(",", rez);

            //kompanija se apdejtuje sa jednim radnikom manje
            raptor.updateCompany(tempCId.Id, tempCId);

            //nalazi firmu u koju radnik treba da se upise preko id-a
            var compRetId = raptor.getCompanyById(compRet);

            if (compRetId.Employees.Length != 0)
            {
                compRetId.Employees = recvv.Id.ToString();
            }
            else
            {
                compRetId.Employees = compRetId.Employees + recvv.Id.ToString();
            }
            raptor.updateCompany(compRetId.Id, compRetId);
            recvv.CompanyId = compRet.ToString();
            recvv.CompanyName = compRetId.CompanyName;
        }
        else
        {
            var compRetId = raptor.getCompanyById(compRet);
            if (compRetId.Employees.Length != 0)
            {
                compRetId.Employees = recvv.Id.ToString();
            }
            else
            {
                compRetId.Employees = compRetId.Employees + recvv.Id.ToString();
            }
            raptor.updateCompany(compRetId.Id, compRetId);
            recvv.CompanyId = compRet.ToString();
            recvv.CompanyName = compRetId.CompanyName;
        }

        var res = raptor.updateWorker(recvv.Id, recvv);

        if (res != null)
        {
            HttpContext.Current.Session.Add("userR", res);
            return "Update successfull!";
        }
        return fail;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string enterNewCompanyInRDb(string company, string owner, string type, string location, string mail, string pass, string check)
    {
        CompaniesR c = new CompaniesR()
        {
            CompanyName = company,
            Owner = owner,
            Type = type,
            Location = location,
            Email = mail,
            Password = pass,
            Checkbox = check
        };

        var ret = raptor.CreateCompany(c);

        if (ret != null)
        {
            HttpContext.Current.Session.Add("companyR", ret);
            return succ;
        }
        return fail;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string returnCompanyFromEmailR(string mail, string pass)
    {

        RowShemaCompanies c = raptor.getCompanyByEmail(mail);

        if (c != null)
        {

            CompaniesR com = new CompaniesR()
            {
                Id = c.docid,
                CompanyName = c.CompanyName,
                Owner = c.Owner,
                Type = c.Type,
                Location = c.Location,
                Employees = c.Employees,
                Email = mail,
                Password = pass,
                Checkbox = c.Checkbox
            };

            if (c.Password == pass)
            {
                HttpContext.Current.Session.Add("companyR", com);
                HttpContext.Current.Session.Add("userR", null);
                return JsonConvert.SerializeObject(com);
            }
            else
                return badp;
        }
        else
            return badm;

    }

    [System.Web.Services.WebMethod]
    public List<RowShemaCompanies> retAllCompaniesFromCollectionR()
    {
        List<RowShemaCompanies> c = raptor.GetCompanies(); 
        return c;
    }
}
