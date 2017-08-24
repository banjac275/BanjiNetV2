using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;


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

    public MongoService()
    {
        mongoDbase = new MongoDataAccess();
    }

    //trazi radnika u bazi i proverava da li mu je dobra sifra
    [System.Web.Services.WebMethod]
    public string returnWorkerFromEmail(string mail, string pass)
    {
        //var obj = JObject.Parse(jsons);
        //var mail = (string)obj.SelectToken("Email");
        List<Workers> w = mongoDbase.getWorkerByEmail(mail);
        JavaScriptSerializer jserial = new JavaScriptSerializer();
        if (w.Count != 0)
        {
            if (w[0].Password == pass)
                return JsonConvert.SerializeObject(w[0]);
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
        Workers w;
        w.FirstName = name;

    }


}
