using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace LowStockApp.Classes
{
    public class AppSession
    {
        public string Email = "";
        public string LinnworksApiSessionToken = "";
        public string Token = "";
        public Guid LinnworksUserId = Guid.Empty;
        public string Server = "";
        public string Locality = "";


        public static AppSession LoadAppSession(string token)
        {
            string fileName = System.IO.Path.Combine(AppSettings.TokenStorage, token);
            string fileContent = "";
            if (System.IO.File.Exists(fileName))
            {
                using (System.IO.StreamReader r = new System.IO.StreamReader(fileName))
                {
                    fileContent = r.ReadToEnd();
                    r.Close();
                }
                return JsonConvert.DeserializeObject<AppSession>(fileContent);
            }
            else {
                AppSession newSession = new AppSession()
                {
                    Token = token
                };
                newSession.AuthSession();
                AppSession.SaveAppSession(token, newSession);
                return newSession;
            }
        }




        public static void SaveAppSession(string token, AppSession appsession)
        {
            string fileName = System.IO.Path.Combine(AppSettings.TokenStorage, token);

            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(fileName))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, appsession);
            }
        }

        public T GetObject<T>(string controller, string method, string data)
        {
            object output = null;
            string URL = string.Concat(Server, "/api/", controller, "/", method);
            string outputString = "";
            try
            {
                outputString = GetAPIData(URL, data, LinnworksApiSessionToken);
            }
            catch (WebException ex)
            {
                HttpStatusCode code = ((HttpWebResponse)ex.Response).StatusCode;
                if (code == HttpStatusCode.Unauthorized)
                {
                    AuthSession();
                    try
                    {
                        outputString = GetAPIData(URL, data, LinnworksApiSessionToken);
                    }
                    catch (WebException ex2)
                    {
                        if (ex2.Response != null)
                        {
                            var responseStream = ex2.Response.GetResponseStream();
                            string responseText = "";
                            if (responseStream != null)
                            {
                                using (var reader = new StreamReader(responseStream))
                                {
                                    responseText = reader.ReadToEnd();
                                }
                            }
                            throw new Exception(responseText);
                        }
                    }
                }
                else {
                    if (ex.Response != null)
                    {
                        var responseStream = ex.Response.GetResponseStream();
                        string responseText = "";
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseText = reader.ReadToEnd();
                            }
                        }
                        throw new Exception(responseText);
                    }
                }
            }

            output = JsonConvert.DeserializeObject<T>(outputString);
            return (T)output;
        }

        public void AuthSession()
        {
            LinnworksSession currentSession = null;
            //first check if the session is active
            if (this.LinnworksApiSessionToken != "" && this.LinnworksApiSessionToken != null)
            {
                try
                {
                    string output = GetAPIData(AppSettings.LinnworksAPIUrl + "/Auth/GetSession", "token=" + this.LinnworksApiSessionToken, this.LinnworksApiSessionToken);
                    currentSession = JsonConvert.DeserializeObject<LinnworksSession>(output);
                }
                catch (Exception ex)
                {

                }
            }
            if (currentSession == null)
            {
                try
                {
                    string myParameters = string.Format("applicationId={0}&applicationSecret={1}&token={2}", AppSettings.ApplicationId, AppSettings.ApplicationSecret, this.Token);
                    string output = GetAPIData(AppSettings.LinnworksAPIUrl + "/Auth/AuthorizeByApplication", myParameters, null);
                    currentSession = JsonConvert.DeserializeObject<LinnworksSession>(output);
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            this.Locality = currentSession.Locality;
            this.Server = currentSession.Server;
            this.LinnworksUserId = new Guid(currentSession.Id);
            this.LinnworksApiSessionToken = currentSession.Token;
        }


        static string GetAPIData(string URL, string data, string sessionId)
        {
            string HtmlResult = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Accept] = "application/json, text/javascript, */*; q=0.01";
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded; charset=UTF-8";
                if (sessionId != null)
                {
                    wc.Headers[HttpRequestHeader.Authorization] = sessionId;
                }
                HtmlResult = wc.UploadString(URL, data);
            }
            return HtmlResult;
        }
    }
}