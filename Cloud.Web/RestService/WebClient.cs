using System;
using System.Configuration;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Web.Helpers;
using Cloud.Common.Resources;
using Cloud.Web.Resources;
using RestSharp;

namespace Cloud.Web.RestService
{
    public class WebApiClient
    {
        #region Private filelds

        private readonly string _serverUrl;

        private readonly ObjectCache _cache = MemoryCache.Default;

        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30.00)
        };

        #endregion Private filelds

        public WebApiClient()
        {
            _serverUrl = ConfigurationManager.AppSettings[ServerUrls.Domain];
        }

        #region Public filelds

        public dynamic CallApi(string reqPath, Method reqVerb, dynamic reqPostData = null, string token = null)
		{
			var cacheKey = new StringBuilder();
			cacheKey.Append("APIReq:").Append(reqPath).Append(":").Append(reqVerb).Append(":").Append((token != null));
            if ((_cache[cacheKey.ToString()]) == null)
			{
                lock (_cache)
				{
					dynamic response = MakeRequest(reqPath, reqVerb, reqPostData, false, token);
                    _cache.Add(cacheKey.ToString(), response, _policy);
					return response.StatusCode != HttpStatusCode.OK ? string.Empty : response.Content;
				}
			}

            var itemFromCache = _cache[cacheKey.ToString()] as IRestResponse;				

			return itemFromCache != null ? itemFromCache.Content : string.Empty;

		}

        public dynamic CallApiWithoutCache(string url, Method verb, dynamic postData = null, string token = null)
		{
            var response = MakeRequest(url, verb, postData, false, token);
            return response.StatusCode != HttpStatusCode.OK ? response.Content : string.Empty;
		}

		public dynamic LoginApi(dynamic reqPostData)
		{
			dynamic response = MakeRequest(ServerUrls.AuthorizationToken, Method.POST, reqPostData, true, null);
            return Json.Decode<dynamic>(response.Content);
		}

        #endregion Public filelds

        #region Private filelds

        private IRestResponse MakeRequest(string uri, Method verb, dynamic postData = null,
            bool isFormData = false, string authorizationToken = null)
        {
            var client = new RestClient(_serverUrl);
            var request = new RestRequest(uri, verb);

            if (!string.IsNullOrWhiteSpace(authorizationToken))
            {
                request.AddHeader(HttpHeaders.Authorization, authorizationToken);
            }

            if (isFormData && postData != null)
            {
                foreach (var pair in postData)
                {
                    request.AddParameter(pair.Key, pair.Value);
                }
            }
            else
            {
                request.RequestFormat = DataFormat.Json;

                if (postData != null)
                    request.AddBody(postData);
            }

            return client.Execute(request);
        }

        #endregion Private filelds
    }
}
 