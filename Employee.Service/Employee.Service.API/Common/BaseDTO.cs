using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Service.API.Common
{
    #region Request Base
    public class RequestBase
    {
    }
    #endregion
    #region Response Base
    public class ResponseBase
    {
        public string ResponseResult { get; set; }
        public string ResponseMsg { get; set; }
    }
    #endregion
}
