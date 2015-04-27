using System;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Web.Helpers;
using Newtonsoft.Json;
using RestSharp;

namespace Cloud.Web.RestService
{
    public class WebApiClient
    {
        private const string URL = "http://localhost:3994";

        private static readonly ObjectCache Cache = MemoryCache.Default;

        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30.00)
        };


		private static IRestResponse MakeRequest(string uri, Method verb, dynamic postData = null,
            bool isFormData = false, string authorizationToken = null)
		{
			var client = new RestClient(URL);
            var request = new RestRequest(uri, verb);

            if (!string.IsNullOrWhiteSpace(authorizationToken))
			{
                request.AddHeader("Authorization", authorizationToken);
			}

			if (isFormData)
			{
                foreach (dynamic pair in postData)
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
			var tmp = request;
			return client.Execute(request);
		}

		public dynamic CallAPI<T>(string reqPath, Method reqVerb, dynamic reqPostData = null, string token = null)
		{
			var cacheKey = new StringBuilder();
			cacheKey.Append("APIReq:").Append(reqPath).Append(":").Append(reqVerb).Append(":").Append((token != null));

			try
			{
				if ((Cache[cacheKey.ToString()]) == null)
				{
					lock (Cache)
					{
						dynamic response = MakeRequest(reqPath, reqVerb, reqPostData, false, token);
						Cache.Add(cacheKey.ToString(), response, _policy);
						return response.StatusCode != HttpStatusCode.OK
							? JsonConvert.DeserializeObject<T>("")
							: JsonConvert.DeserializeObject<T>(response.Content);
					}
				}

				var itemFromCache = Cache[cacheKey.ToString()] as IRestResponse;				

				return Json.Decode<T>(itemFromCache != null ? itemFromCache.Content : "");
			}
			catch (Exception exception)
			{
				return Json.Decode<T>("");
			}
		}

		public dynamic WithoutCacheCallAPI<T>(string reqPath, Method reqVerb, dynamic reqPostData = null, string token = null)
		{
			dynamic response = MakeRequest(reqPath, reqVerb, reqPostData, false, token);
			
			return response.StatusCode != HttpStatusCode.OK
				? JsonConvert.DeserializeObject<T>("")
				: JsonConvert.DeserializeObject<T>(response.Content);
		}

		public dynamic LoginAPI(dynamic reqPostData)
		{
			dynamic response = MakeRequest("/token", Method.POST, reqPostData, true, null);

			return Json.Decode<dynamic>(response.Content);
		}
	}
}
 