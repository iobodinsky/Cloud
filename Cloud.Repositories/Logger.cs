using System;
using Cloud.Repositories.DataContext;
using Cloud.Repositories.Repositories;

namespace Cloud.Repositories
{
    public class Logger
    {
        public void LogException(Exception ex)
        {
            var errorLogMessageModel = new ErrorLogMessage
            {
                DateTime = DateTime.Now.ToString("MMM ddd d HH:mm yyyy"),
                StackTrace = ex.StackTrace,
                Message = ex.Message
            };

            if (ex.InnerException != null)
                errorLogMessageModel.Message += ex.InnerException.Message;

            if (errorLogMessageModel.Message.Length >= 2000)
                errorLogMessageModel.Message = errorLogMessageModel.Message.Substring(0, 1999);

            if (errorLogMessageModel.StackTrace == null)
                errorLogMessageModel.StackTrace = "Custom exception";
            else if (errorLogMessageModel.StackTrace.Length >= 4000)
                errorLogMessageModel.StackTrace = errorLogMessageModel.StackTrace.Substring(0, 3999);
            

            new ErrorMessageRepository().Add(errorLogMessageModel, true);
        }
    }
}
