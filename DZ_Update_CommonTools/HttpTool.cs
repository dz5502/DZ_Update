using DZ_Update.CommonTools.Base;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DZ_Update_CommonTools
{
    public class HttpTool
    {

        /// <summary>
        /// 连接服务
        /// </summary>
        /// <param name="method">连接方式</param>
        /// <param name="resource"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static IRestResponse ClientConnect(string url, Method method, object parameter = null, String token="")
        {
            IRestResponse result = null;

            RestClient client = new RestClient(url);
            RestRequest req = new RestRequest();
            req.Method = method;

            if (!String.IsNullOrEmpty(token))
            {
                req.AddHeader("Authorization", "Basic " + token);
            }

            if (parameter != null)
            {
                var json = JsonConvert.SerializeObject(parameter);
                req.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            }


            if (method == Method.GET)
            {
                result = client.Get(req);
                //result = client.Get(req);
            }
            else if (method == Method.POST)
            {
                result = client.Post(req);
            }
            else if (method == Method.PUT)
            {
                result = client.Put(req);
            }

            return result;
        }

        public static void DownloadFile(string url, String saveFile, Action<Int64> progressAction, string token="")
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            if (!String.IsNullOrEmpty(token))
            {
                request.AddHeader("Authorization", "Basic " + token);
            }


            using (var writer = new DZFileStream(saveFile))
            {
                writer.Progress += (w, e) => {
                    //Console.WriteLine(string.Format("\rProgress: {0} / {1:P2}", writer.CurrentSize, ((double)writer.CurrentSize) / fileSize));

                    progressAction?.Invoke(writer.CurrentSize);
                };
                request.ResponseWriter = (responseStream) => responseStream.CopyTo(writer);
                var response = client.DownloadData(request);

                return;
            }
        }

        public static IRestResponse SendFormConnect(string url, Method method, List<Tuple<String, object>> keyvalues, Tuple<String, String> cookie = null, String token = "", int timeOut = 10 * 1000)
        {
            IRestResponse result = null;

            RestClient client = new RestClient(url);

            RestRequest req = new RestRequest();
            req.Timeout = timeOut;
            req.Method = method;

            req.AlwaysMultipartFormData = true;

            foreach (var item in keyvalues)
            {
                if (item.Item2 == null)
                    continue;

                req.AddParameter(item.Item1, item.Item2);
            }            

            result = client.Execute(req);
            return result;
        }

        /// <summary>  
        /// 获取网络日期时间  
        /// </summary>  
        /// <returns></returns>  
        public static string GetNetDateTime()
        {
            WebRequest request = null;
            WebResponse response = null;
            WebHeaderCollection headerCollection = null;
            string datetime = string.Empty;

            try
            {
                request = WebRequest.Create("https://www.baidu.com");
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = (WebResponse)request.GetResponse();
                headerCollection = response.Headers;

                foreach (var h in headerCollection.AllKeys)
                {
                    if (h == "Date")
                    {
                        datetime = headerCollection[h];
                    }
                }

                return datetime;
            }
            catch (Exception)
            {
                return datetime;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }

                if (response != null)
                {
                    response.Close();
                }

                if (headerCollection != null)
                {
                    headerCollection.Clear();
                }
            }
        }
        public static bool OnNetWork()
        {
            String txt = HttpTool.ClientConnect("http://www.baidu.com", RestSharp.Method.GET).Content;
            return !String.IsNullOrEmpty(txt) && (txt.Length > 100);
        }
    }
}
