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

    public MongoService()
    {
        mongoDbase = new MongoDataAccess();
    }

    [System.Web.Services.WebMethod]
    public string returnWorkerFromEmail(string mail)
    {
        //var obj = JObject.Parse(jsons);
        //var mail = (string)obj.SelectToken("Email");
        List<Workers> w = mongoDbase.getWorkerByEmail(mail);
        //JavaScriptSerializer jserial = new JavaScriptSerializer();
        return JsonConvert.SerializeObject(w[0]);
        //return w.ToString();
    }

}
