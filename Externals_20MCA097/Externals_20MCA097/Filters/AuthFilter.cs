using Externals_20MCA097.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Externals_20MCA097.Filters
{
    public class AuthFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext); //getting actioncontext for future use

            //checking if user has provided authorization or not
            if (actionContext.Request.Headers.Authorization == null)
            {
                HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.Forbidden);
                httpResponse.Content = new StringContent("Authorization Data Not Found!!!");
                actionContext.Response = httpResponse;
            }
            else
            {
                String encodedData = actionContext.Request.Headers.Authorization.Parameter;
                String decodeData = Encoding.UTF8.GetString(Convert.FromBase64String(encodedData)); //decoding the data sent by the auth
                String[] userdata = decodeData.Split(':'); //spliting using ':' which will separate username and password

                //saving for ease of use
                int id = Convert.ToInt32(userdata[0]);
                String password = userdata[1];

                AppointmentDBEntities context = new AppointmentDBEntities(); //creating entity object just to access data from the table

                //using lamda and saving the return value in um where both id and pass matches in admin table
                User_Master um = context.User_Master.Where(b => b.User_id == id && b.User_Password_.Equals(password)).FirstOrDefault();


                //if null it means no data found meaning id pass is wrong
                if (um != null)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity((um.User_id).ToString()), null); // saving identity for future use and calling thread (if needed)
                }
                else
                {
                    HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    httpResponse.Content = new StringContent("Authorization Data is Invalid");
                    actionContext.Response = httpResponse;
                }
            }
        }
    }
}