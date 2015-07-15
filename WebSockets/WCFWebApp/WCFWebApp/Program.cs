using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json.Linq;

namespace WCFWebApp
{
    [ServiceContract]
    public interface IWCFWebApp01
    {
        [OperationContract, WebGet(UriTemplate = "/{resource}.{extension}")]
        Stream Files(string resource, string extension);

        [OperationContract, WebGet(UriTemplate = "/{path}/{resource}.{extension}")]
        Stream Links(string path, string resource, string extension);

        [OperationContract, WebInvoke(Method = "POST", UriTemplate = "/login",RequestFormat=WebMessageFormat.Json)]
        Stream Login(Stream request);

        [OperationContract, WebInvoke(Method = "POST", UriTemplate = "/logout", RequestFormat = WebMessageFormat.Json)]
        Stream Logout(Stream request);

        [OperationContract, WebGet(UriTemplate = "/states")]
        Stream States();

        [OperationContract, WebInvoke(Method = "POST", UriTemplate = "/state")]
        Stream State(Stream request);
    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,ConcurrencyMode=ConcurrencyMode.Multiple)]
    public class WebApp : IWCFWebApp01
    {
        JObject states;

        public WebApp()
        {
            if (states==null)
                states = JObject.Parse(File.ReadAllText("web\\data\\states.json"));
        }

        public bool Authenticate()
        {
            string userName = "user";
            string password = "pass";

            string basicAuthCode = Convert.ToBase64String (Encoding.ASCII.GetBytes (string. Format ("{0}: {1}", userName, password)));
            string token = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
            if (token.Contains(basicAuthCode))
            {
                return true;
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                return false;
            }
        }

        public Stream Links(string path, string resource, string extension)
        {
            int extensionIndex = extension.LastIndexOf('.');
            if (extensionIndex > 0)
            {
                resource += "." + extension.Substring(0, extensionIndex);
                extension = extension.Remove(0, extensionIndex + 1);
            }
            return Files(path + "\\" + resource, extension);
        }

        public Stream Files(string resource, string extension)
        {
            switch (extension)
            {
                //http://www.w3schools.com/media/media_mimeref.asp
                case "htm":
                    if ((String.Compare(resource, "index") != 0) && (String.Compare(resource, "login") != 0))
                    {
                        if (!Authenticate()) return null;
                    }
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                    break;
                case "js":
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/javascript";
                    break;
                case "css":
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/css";
                    break;
                case "png":
                    WebOperationContext.Current.OutgoingResponse.ContentType = "image/png";
                    break;
                case "ico":
                    WebOperationContext.Current.OutgoingResponse.ContentType = "image/x-icon";
                    break;
                default:
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.UnsupportedMediaType;
                    return new MemoryStream(Encoding.ASCII.GetBytes("File type not supported"), false);
            }
            string fileName = String.Format("web\\{0}.{1}", resource, extension);
            return new FileStream(fileName, FileMode.Open);
        }

        public Stream Login(Stream request)
        {
            if (!Authenticate()) return null;

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            long length = WebOperationContext.Current.IncomingRequest.ContentLength;
            StreamReader reader = new StreamReader(request);

            char [] buffer = new char[length];
            reader.Read(buffer, 0, (int)length);
            string data = new string(buffer);
            
            JObject o = JObject.Parse(data);
            string message = String.Format("User {0} loggedin", o["username"].Value<string>());
            
            return new MemoryStream(Encoding.ASCII.GetBytes( message), false);
        }

        public Stream Logout(Stream request)
        {
            if (!Authenticate()) return null;

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";

            long length = WebOperationContext.Current.IncomingRequest.ContentLength;
            StreamReader reader = new StreamReader(request);

            char[] buffer = new char[length];
            reader.Read(buffer, 0, (int)length);
            string data = new string(buffer);

            JObject o = JObject.Parse(data);
            string message = String.Format("User {0} logged out.", o["username"].Value<string>());

            return new MemoryStream(Encoding.ASCII.GetBytes(message), false);
        }

        public Stream States()
        {
            if (!Authenticate()) return null;

            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";
            return new MemoryStream(Encoding.ASCII.GetBytes(states.ToString()),false);
        }

        public Stream State(Stream request)
        {
            if (!Authenticate()) return null;

            long length = WebOperationContext.Current.IncomingRequest.ContentLength;
            StreamReader reader = new StreamReader(request);

            char[] buffer = new char[length];
            reader.Read(buffer, 0, (int)length);

            JObject data = JObject.Parse(new string(buffer));
            int id = ((int)data["id"]) -1;
            states["states"][id]["visited"] = true;

            return States();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {            
            string baseAddress = "http://" + Environment.MachineName + ":2011/";
            using (WebServiceHost host = new WebServiceHost(typeof(WebApp), new Uri(baseAddress)))
            {
                WebHttpBinding binding = new WebHttpBinding();

                host.AddServiceEndpoint(typeof(IWCFWebApp01), binding, "").Behaviors.Add(new WebHttpBehavior());
                host.Open();

                Console.WriteLine("Server running @ " + baseAddress);
                Console.Write("Press any key to terminate");
                Console.ReadLine();
            }
        }
    }
}
