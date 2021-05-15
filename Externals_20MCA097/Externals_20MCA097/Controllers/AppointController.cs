using Externals_20MCA097.Filters;
using Externals_20MCA097.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Externals_20MCA097.Controllers
{
    [RoutePrefix("Appointment")] //Giving Prefix for Ez Use
    public class AppointController : ApiController
    {
        AppointmentDBEntities context = new AppointmentDBEntities(); // Entity Context

        [HttpPost]
        [AuthFilter] //Applying AuthFilter
        [Route("InsertData")]
        [CusExceptionFilter] //Giving My Custom Exception Filter To The Method Where we Insert Data
        public Boolean POSTAppointment(Appointment_Master AM)
        {
            context.Appointment_Master.Add(AM);
            int f = context.SaveChanges();
            return (f > 0) ? true : false; // If Valid Add ata to table and returning true or false depending on the database/table
        }


        [HttpGet]
        [Route("ViewAll")]
        [AuthFilter] //Applying AuthFilter
        public HttpResponseMessage ViewAll()
        {
            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);
            res.Content = new ObjectContent(typeof(List<Appointment_Master>), context.Appointment_Master.ToList(), new JsonMediaTypeFormatter()); //directly saving entire table list in the content to return
            return res;
        }

        [HttpGet]
        [Route("GetByID")]
        [AuthFilter] //Applying AuthFilter
        public HttpResponseMessage GetAccountByAc(int ID)
        {
            Appointment_Master obj = context.Appointment_Master.FirstOrDefault(s => s.ap_id == ID); //using lamda just getting data of the account holder who matches the argument
            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);
            res.Content = new ObjectContent(typeof(Appointment_Master), obj, new JsonMediaTypeFormatter());
            return res;
        }
    }
}
