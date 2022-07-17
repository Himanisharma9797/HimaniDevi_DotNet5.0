using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Employee.Service.API.Common
{
    #region Response Result Enum
    public enum ResponseResultEnum
    {      
            Success,
            Warning,
            Exception,
           ServiceFault
        
    }
    #endregion
    #region Category Enum
    public enum CategoryEnum
    {
        easy,
        medium,
        hard
    }
    #endregion

}
