using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model;

namespace Business.Empleado
{
    public class EmpleadoImplements
    {
        PagosEntities iContext;

        public EmpleadoImplements()
        {
            iContext = new PagosEntities();
        }

        public SEL_EMPLEADO_SP_Result obtieneEmpleado(int idempleado)
        {
            return iContext.SEL_EMPLEADO_SP(idempleado).FirstOrDefault();
        }
    }
}
