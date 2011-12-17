using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Fotoideen.Helper
{
    public class Proxy
    {
        public void LoadData()
        {
            var webRequest = (HttpWebRequest) WebRequest.Create(new Uri(Constants.DataUrl, UriKind.Absolute));
            webRequest.BeginGetResponse(FinishedLoadData, webRequest);
        }

        private void FinishedLoadData(IAsyncResult asyncRsult)
        {
            var webRequest = asyncRsult.AsyncState as HttpWebRequest;
            try
            {
                var webResponse = (HttpWebResponse)webRequest.EndGetResponse(asyncRsult);
                var sr = new StreamReader(webResponse.GetResponseStream());
                var statusCode = webResponse.StatusCode;
                var response = sr.ReadToEnd();
                var normalizedResponse = JsonNormalize.DoNormalize(response);
                InvokeProxyEvent(new ProxyEventArgs(normalizedResponse, statusCode));
                sr.Close();
                webResponse.Close();
            }
            catch(WebException ex)
            {
                var webResponse = (HttpWebResponse)ex.Response;
                var statusCode = webResponse.StatusCode;
                InvokeProxyEvent(new ProxyEventArgs(string.Empty, statusCode));
                webResponse.Close();
            }
            catch(Exception ex)
            {
                InvokeProxyEvent(new ProxyEventArgs(string.Empty, HttpStatusCode.Unused));
            }
            finally
            {
                webRequest.Abort();
            }
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
