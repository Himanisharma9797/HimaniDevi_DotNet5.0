using Employee.Service.API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Service.API.Account
{
    #region Registration Service Contracts
    public class RegistrationOfEmployeesRequest : RequestBase
    {
        public RegistrationInfo RegistrationInfo { get; set; } = new RegistrationInfo();
    }
    public class RegistrationOfEmployeesResponse : ResponseBase
    {
        
    }
    #endregion
    #region LogIn Service Contracts
    public class LogInOfEmployeesRequest : RequestBase
    {
        public LogInInfo LogInInfo { get; set; } = new LogInInfo();
    }

    public class LogInOfEmployeesResponse : ResponseBase
    {
       public string Token { get; set; }

    }
    #endregion
}
