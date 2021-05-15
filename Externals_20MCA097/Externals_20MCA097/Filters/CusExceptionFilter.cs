using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Externals_20MCA097.Filters
{
    public class CusExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);

            //checking for dbupdateexception
            if (actionExecutedContext.Exception is DbUpdateException)
            {
                HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                response.Content = new StringContent("Error Occured (DbUpdateException)");
                actionExecutedContext.Response = response;
            }
            //if none of the above exception has occured returing common error....
            else
            {
                HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Error Occured (Information Not Available");
                actionExecutedContext.Response = response;
            }
        }
    }
}