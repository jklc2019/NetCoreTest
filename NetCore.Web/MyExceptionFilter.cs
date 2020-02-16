using Microsoft.AspNetCore.Mvc.Filters;
using NetCore.MyService;

namespace NetCore.Web
{
    public class MyExceptionFilter : IExceptionFilter
    {
        private ILogService logservice;
        public MyExceptionFilter(ILogService logservice)
        {
            this.logservice = logservice;
        }
        public void OnException(ExceptionContext context)
        {
            logservice.AddLog(context.Exception.Message);
        }
    }
}
