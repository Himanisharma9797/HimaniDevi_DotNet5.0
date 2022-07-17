using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Service.API.Account
{
    #region Registration Info
    public class RegistrationInfo
    {      
        public string FirstName { get; set; }     
        public string LastName { get; set; }   
        public string Email { get; set; }
        public string Password { get; set; } 
        public string ConfirmPassword { get; set; }
    }
    #endregion
    #region LogIn Info
    public class LogInInfo
    {    
        public string UserName { get; set; } 
        public string Password { get; set; }
       
    }
    #endregion
}
