using Employee.Service.API.Common;
using Employee.Service.API.Account;
using Employee.Service.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Service.Logic.Common
{
    public class ExceptionHelpers
    {
        public async static Task<string> ProcessException(Exception ex)
        {

            var _addRegisterRequest = new Employee.Service.API.Account.RegistrationOfEmployeesRequest()
            {
                RegistrationInfo = new Employee.Service.API.Account.RegistrationInfo()

            };

            var _ClientErrorMessage = ex.Message;
            _addRegisterRequest.RegistrationInfo.FirstName = _ClientErrorMessage;
            try
            {
                _addRegisterRequest.RegistrationInfo.Email = ex.Message;
                _addRegisterRequest.RegistrationInfo.FirstName = ex.Message;
            }
            catch
            {
                Console.WriteLine("{0} Exception caught.", ex);

            }
            var _addLogInRequest = new Employee.Service.API.Account.LogInOfEmployeesRequest()
            {
                LogInInfo = new Employee.Service.API.Account.LogInInfo()

            };
            var ClientErrorMessage = ex.Message;
            _addLogInRequest.LogInInfo.UserName = ClientErrorMessage;
            try
            {
                _addLogInRequest.LogInInfo.UserName = ex.Message;
                _addLogInRequest.LogInInfo.Password = ex.Message;
            }
            catch
            {
                Console.WriteLine("{0} Exception caught.", ex);

            }


            return _ClientErrorMessage;
        }
        public static T ProcessException<T>(
          RequestBase Request, Exception ex) where T : ResponseBase
        {
            var _Response = Activator.CreateInstance<T>();

            if (ex is ValidationException)
                _Response.ResponseResult = nameof(ResponseResultEnum.Warning);
            else
                _Response.ResponseResult = nameof(ResponseResultEnum.Exception);

            var _errMsg = ProcessException(ex);

            _Response.ResponseMsg= _errMsg.Result;

            return _Response;
        }


    }
}

