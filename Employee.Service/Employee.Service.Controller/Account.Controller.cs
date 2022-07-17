using Employee.Service.API.Account;
using Employee.Service.DAL;
using Employee.Service.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Service.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account : ControllerBase
    {
        #region Private Readonly peoperties
        private readonly EmployeeDbContext _employeeDbContext;
        private readonly IConfiguration _configuration;
        #endregion
        #region Constructor
        public Account(EmployeeDbContext employeeDbContext, IConfiguration configuration)
        {
            _employeeDbContext = employeeDbContext;
            _configuration = configuration;
        }
        #endregion
        #region Public Methods
        /// <summary>
        ///   Registration of employees     
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("RegistrationOfEmployees")]
        public ActionResult<RegistrationOfEmployeesResponse> RegistrationOfEmployees(RegistrationOfEmployeesRequest request)
        {
            try
            {
                var accountLogic = new AmountLogic(_employeeDbContext, _configuration);
                return accountLogic.GetRegistration(request);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);

            }
        }
        /// <summary>
        /// Login of employees
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("LogInOfEmployees")]
        public ActionResult<LogInOfEmployeesResponse> LogInOfEmployees(LogInOfEmployeesRequest request)
        {
            try
            {
                var accountLogic = new AmountLogic(_employeeDbContext, _configuration);
                return accountLogic.GetLogIn(request);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);

            }
        }
        #endregion
    }
}
