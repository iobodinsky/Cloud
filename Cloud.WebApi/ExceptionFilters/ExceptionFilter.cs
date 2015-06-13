using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;
using Cloud.WebApi.Models;

namespace Cloud.WebApi.ExceptionFilters {
	public class ExceptionFilter : ExceptionFilterAttribute {
		public override void OnException( HttpActionExecutedContext actionExecutedContext ) {
			var model = new ServerError {
				Message = actionExecutedContext.Exception.Message,
				StackTrace = actionExecutedContext.Exception.StackTrace
			};
			if (actionExecutedContext.Exception.InnerException != null) {
				model.InnerServerError = new ServerError {
					Message = actionExecutedContext.Exception.InnerException.Message,
					StackTrace = actionExecutedContext.Exception.InnerException.StackTrace
				};
			}

			actionExecutedContext.Response = new HttpResponseMessage {
				StatusCode = HttpStatusCode.InternalServerError,
				Content = new ObjectContent<ServerError>(model, new JsonMediaTypeFormatter())
			};
		}
	}
}