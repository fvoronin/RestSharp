#if FRAMEWORK || PocketPC
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using RestSharp.Deserializers;

namespace RestSharp
{
	public partial class RestClient
	{


		/// <summary>
		/// Executes the specified request and downloads the response data
		/// </summary>
		/// <param name="request">Request to execute</param>
		/// <returns>Response data</returns>
		public byte[] DownloadData(IRestRequest request)
		{
			var response = Execute(request);
			return response.RawBytes;
		}

		/// <summary>
		/// Executes the request and returns a response, authenticating if needed
		/// </summary>
		/// <param name="request">Request to be executed</param>
		/// <returns>RestResponse</returns>
		public virtual IRestResponse Execute(IRestRequest request)
		{
#if !PocketPC
			var method = Enum.GetName(typeof(Method), request.Method);
			switch (request.Method)
			{
				case Method.POST:
				case Method.PUT:
				case Method.PATCH:
					return Execute(request, method, DoExecuteAsPost);
				default:
					return Execute(request, method, DoExecuteAsGet);
			}
#else
            switch (request.Method)
            {
                case Method.PATCH:
                    return Execute(request, "PATCH", DoExecuteAsPost);
                case Method.POST:
                    return Execute(request, "POST", DoExecuteAsPost);
                case Method.PUT:
                    return Execute(request, "PUT", DoExecuteAsPost);
                case Method.GET:
                    return Execute(request, "GET", DoExecuteAsGet);
                case Method.DELETE:
                    return Execute(request, "DELETE", DoExecuteAsGet);
                case Method.HEAD:
                    return Execute(request, "HEAD", DoExecuteAsGet);
                case Method.OPTIONS:
                    return Execute(request, "OPTIONS", DoExecuteAsGet);
                default:
                    return Execute(request, "", DoExecuteAsGet);
            }
#endif
        }

		public IRestResponse ExecuteAsGet(IRestRequest request, string httpMethod)
		{
			return Execute(request, httpMethod, DoExecuteAsGet);
		}

		public IRestResponse ExecuteAsPost(IRestRequest request, string httpMethod)
		{
			request.Method = Method.POST; // Required by RestClient.BuildUri... 
			return Execute(request, httpMethod, DoExecuteAsPost);
		}

		private IRestResponse Execute(IRestRequest request, string httpMethod, Func<IHttp, string, HttpResponse> getResponse)
		{
			AuthenticateIfNeeded(this, request);

			IRestResponse response = new RestResponse();
			try
			{
				var http = HttpFactory.Create();

				ConfigureHttp(request, http);

				response = ConvertToRestResponse(request, getResponse(http, httpMethod));
				response.Request = request;
				response.Request.IncreaseNumAttempts();

			}
			catch (Exception ex)
			{
				response.ResponseStatus = ResponseStatus.Error;
				response.ErrorMessage = ex.Message;
				response.ErrorException = ex;
			}

			return response;
		}

		private static HttpResponse DoExecuteAsGet(IHttp http, string method)
		{
			return http.AsGet(method);
		}

		private static HttpResponse DoExecuteAsPost(IHttp http, string method)
		{
			return http.AsPost(method);
		}

	    /// <summary>
		/// Executes the specified request and deserializes the response content using the appropriate content handler
		/// </summary>
		/// <typeparam name="T">Target deserialization type</typeparam>
		/// <param name="request">Request to execute</param>
		/// <returns>RestResponse[[T]] with deserialized data in Data property</returns>
		public virtual IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
		{
			return Deserialize<T>(request, Execute(request));
		}

		public IRestResponse<T> ExecuteAsGet<T>(IRestRequest request, string httpMethod) where T : new()
		{
			return Deserialize<T>(request, ExecuteAsGet(request, httpMethod));
		}

		public IRestResponse<T> ExecuteAsPost<T>(IRestRequest request, string httpMethod) where T : new()
		{
			return Deserialize<T>(request, ExecuteAsPost(request, httpMethod));
		}
	}
}
#endif