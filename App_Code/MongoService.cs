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
                return JsonConvert.SerializeObject(w[0]);
            }
            else
                return badp;
        }
        else
            return badm;
        //return w.ToString();
    }

    [System.Web.Services.WebMethod]
    public string enterNewWorkerInDb(string mail, string pass, string name, string last, string check)
    {
        Workers w = new Workers();
        w.FirstName = name;
        w.LastName = last;
        w.Email = mail;
        w.Password = pass;
        w.Checkbox = check;

        var ret = mongoDbase.Create(w);

        if(ret != null)
        {
            return succ;
        }
        return fail;
    }

    [System.Web.Services.WebMethod]
    public List<Workers> retAllWorkersFromCollection()
    {
        List<Workers> w = mongoDbase.GetWorkers();
        return w;
    }

}
