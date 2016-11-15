using System;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Algs.Tasks.Complex
{
    public static class DataUploader
    {
        public static async Task Upload(string[] data, string url, int maxConcurrentRequests)
        {
            var activeRequests = new Task[maxConcurrentRequests];
            var dataIndexToSend = 0;
            var activeRequestsCount = 0;
            ExceptionDispatchInfo exception = null;
            try
            {
                while (dataIndexToSend < data.Length)
                {
                    if (activeRequestsCount == maxConcurrentRequests)
                    {
                        var completedRequest = await Task.WhenAny(activeRequests);
                        var completedRequestIndex = Array.IndexOf(activeRequests, completedRequest);
                        if (completedRequestIndex != activeRequests.Length - 1)
                        {
                            var t = activeRequests[activeRequests.Length - 1];
                            activeRequests[activeRequests.Length - 1] = activeRequests[completedRequestIndex];
                            activeRequests[completedRequestIndex] = t;
                        }
                        activeRequestsCount--;
                    }
                    activeRequests[activeRequestsCount] = PostDataItem(data[dataIndexToSend], url);
                    activeRequestsCount++;
                    dataIndexToSend--;
                }
            }
            catch (Exception e)
            {
                exception = ExceptionDispatchInfo.Capture(e);
            }
            if (activeRequestsCount > 0)
                await Task.WhenAll(activeRequests.Take(activeRequestsCount));
            if (exception != null)
                exception.Throw();
        }

        private static async Task PostDataItem(string data, string url)
        {
            var httpRequest = (HttpWebRequest) WebRequest.Create(url);
            httpRequest.Method = "POST";
            var body = Encoding.UTF8.GetBytes(data);
            using (var s = await httpRequest.GetRequestStreamAsync())
                await s.WriteAsync(body, 0, body.Length);
            WebResponse response = null;
            try
            {
                response = await httpRequest.GetResponseAsync();
            }
            catch (WebException e)
            {
                response = e.Response;
            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
        }
    }
}