using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Business.Empleado;
using DataAccess.Model;

namespace WebApp.Controllers
{
    public class EmpleadoApiController : ApiController
    {
        EmpleadoImplements iEmpleado;

        public EmpleadoApiController()
        {
            iEmpleado = new  EmpleadoImplements();
        }

        public HttpResponseMessage Get(string id)
        {
            var parameters = id.Split('|');
            switch (parameters[0])
            {
                case "1":
                    var empleado = iEmpleado.obtieneEmpleado(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<SEL_EMPLEADO_SP_Result>(HttpStatusCode.OK, empleado);
                case "2":
                    return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}