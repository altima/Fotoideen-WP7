using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SharpGIS.ZLib;

namespace Fotoideen.Helper
{
    public class Proxy
    {
        private bool _enableGZip = false;

        public void LoadData(bool enableGzip = false)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(new Uri(Constants.DataUrl, UriKind.Absolute));
            if (enableGzip)
            {
                webRequest.Headers["Accept-Encoding"] = "gzip";
            }
            webRequest.BeginGetResponse(FinishedLoadData, webRequest);
        }

        private void FinishedLoadData(IAsyncResult asyncRsult)
        {
            var webRequest = asyncRsult.AsyncState as HttpWebRequest;
            try
            {
                var webResponse = (HttpWebResponse)webRequest.EndGetResponse(asyncRsult);
                var statusCode = webResponse.StatusCode;
                string response = string.Empty;

                using (var responseStream = webResponse.GetResponseStream())
                {
                    if (webResponse.Headers["Content-Encoding"] == "gzip")
                    {
                        response = UnzipResponse(responseStream);
                    }
                    else
                    {
                        using (var sr = new StreamReader(responseStream))
                        {
                            response = sr.ReadToEnd();
                        }
                    }
                }
                var normalizedResponse = JsonNormalize.DoNormalize(response);
                InvokeProxyEvent(new ProxyEventArgs(normalizedResponse, statusCode));
                webResponse.Close();
            }
            catch (WebException ex)
            {
                var webResponse = (HttpWebResponse)ex.Response;
                var statusCode = webResponse.StatusCode;
                InvokeProxyEvent(new ProxyEventArgs(string.Empty, statusCode));
                webResponse.Close();
            }
            catch (Exception ex)
            {
                InvokeProxyEvent(new ProxyEventArgs(string.Empty, HttpStatusCode.Unused));
            }
            finally
            {
                webRequest.Abort();
            }
        }

        private string UnzipResponse(Stream input)
        {
            var sb = new StringBuilder();
            using (var zippedStream = new GZipStream(input))
            {
                using (var reader = new StreamReader(zippedStream))
                {
                    while (!reader.EndOfStream)
                    {
                        sb.AppendLine(reader.ReadLine());
                    }
                }
            }
            return sb.ToString();
        }


        public event EventHandler ProxyEvent;

        private void InvokeProxyEvent(ProxyEventArgs e)
        {
            EventHandler handler = ProxyEvent;
            if (handler != null) handler(this, e);
        }
    }

    public class ProxyEventArgs : EventArgs
    {
        public string Response { get; set; }
        public HttpStatusCode Code { get; set; }

        public ProxyEventArgs(string response, HttpStatusCode code)
        {
            Response = response;
            Code = code;
        }
    }
}
