using Employee.Service.API.Account;
using Employee.Service.API.Common;
using Employee.Service.DAL;
using Employee.Service.DAL.Account;
using Employee.Service.Logic.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Employee.Service.Logic
{
 
    public class AmountLogic
    {
        #region Private Readonly Properties
        private readonly EmployeeDbContext _employeeDbcontext;
        private readonly IConfiguration _configuration;
        #endregion
        #region Constructor
        public AmountLogic(EmployeeDbContext employeeDbContext, IConfiguration configuration)
        {
            _employeeDbcontext = employeeDbContext;
            _configuration = configuration; 
        }
        #endregion
    
        #region Private Methods
        private void CreateHashPassword(string password, out byte[] PasswordHash, out byte[] PasswprdSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                PasswprdSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        
        private bool VerifyPasswordHash(string password, byte[] passwordHash , byte[] passwordSalt)
        {
            using(var hmac= new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0;i<computedHash.Length;i++)
                {
                    if(computedHash[i]!=passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
     
        private string CreateToken(UserInfo userInfo)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserId.ToString()),
                new Claim(ClaimTypes.Name, userInfo.Email)

            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokendDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokendDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
        #region Public Methods
        public RegistrationOfEmployeesResponse GetRegistration(RegistrationOfEmployeesRequest request)
        {
            try
            {
                Validation.RequiredParameter("Request", request);
                Validation.RequiredParameter("Request", request.RegistrationInfo);
                Validation.RequiredParameter("FirstName", request.RegistrationInfo.FirstName);
                Validation.isAlphabets("FirstName", request.RegistrationInfo.FirstName);
                Validation.RequiredParameter("LastName", request.RegistrationInfo.LastName);
                Validation.isAlphabets("LastName", request.RegistrationInfo.LastName);
                Validation.RequiredParameter("Email", request.RegistrationInfo.Email);
                Validation.isValidEmail("email", request.RegistrationInfo.Email);
                Validation.RequiredParameter("Password", request.RegistrationInfo.Password);
                Validation.ComparePassword("Password", "ConfirmPassword", request.RegistrationInfo.Password, request.RegistrationInfo.ConfirmPassword);

                if (_employeeDbcontext.UserInfos.Any(x => x.Email.ToLower().Equals(request.RegistrationInfo.Email.ToLower())))
                {
                    return new RegistrationOfEmployeesResponse
                    {
                        ResponseResult = ResponseResultEnum.Warning.ToString(),
                        ResponseMsg = "User already exists"
                    };
                }
                CreateHashPassword(request.RegistrationInfo.Password, out byte[] hashPassword, out byte[] passwordSalt);
                var user = new UserInfo
                {
                    PasswordHash = hashPassword,
                    PasswordSalt = passwordSalt,
                    FirstName = request.RegistrationInfo.FirstName,
                    LastName = request.RegistrationInfo.LastName,
                    Email = request.RegistrationInfo.Email

                };
                _employeeDbcontext.UserInfos.Add(user);

                var noOfRows = _employeeDbcontext.SaveChanges();
                if (noOfRows != 0)
                {
                    return new RegistrationOfEmployeesResponse()
                    {
                        ResponseMsg = "User can not be added",
                        ResponseResult = ResponseResultEnum.Warning.ToString()
                    };
                }

                return new RegistrationOfEmployeesResponse()
                {
                    ResponseMsg = "User successfully added",
                    ResponseResult = ResponseResultEnum.Success.ToString()
                };
               
            }
           
            catch (Exception ex)
            {
                ex.Data.Add("Request", request);
                return ExceptionHelpers.ProcessException<RegistrationOfEmployeesResponse>(request, ex);
            }
        }
      
        public LogInOfEmployeesResponse GetLogIn(LogInOfEmployeesRequest request)
        {
            try
            {
                Validation.RequiredParameter("LogInRequest", request);
                Validation.RequiredParameter("LogInRequest", request.LogInInfo);
                Validation.RequiredParameter("UserName", request.LogInInfo.UserName);
                Validation.RequiredParameter("Password", request.LogInInfo.Password);

                var user = _employeeDbcontext.UserInfos.FirstOrDefault(x => x.Email.ToLower().Equals(request.LogInInfo.UserName.ToLower()));
                if (user == null)
                {
                    return new LogInOfEmployeesResponse()
                    {
                        ResponseMsg = "User not found",
                        ResponseResult = ResponseResultEnum.Warning.ToString(),

                    };
                }
                else if (!VerifyPasswordHash(request.LogInInfo.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return new LogInOfEmployeesResponse()
                    {
                        ResponseMsg = "Wrong Password",
                        ResponseResult = ResponseResultEnum.Warning.ToString(),


                    };
                }
                else
                {
                    return new LogInOfEmployeesResponse()
                    {
                        ResponseMsg = "User login sucessfully",
                        ResponseResult = ResponseResultEnum.Success.ToString(),
                        Token = CreateToken(user)

                    };
                }
            }
            catch(Exception ex)
            {
                ex.Data.Add("Request", request);
                return ExceptionHelpers.ProcessException<LogInOfEmployeesResponse>(request, ex);
            }
                       
        }
        #endregion
    }
}
