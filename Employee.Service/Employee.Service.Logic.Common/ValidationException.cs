using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Service.Logic.Common
{    public class ValidationException : Exception
    {
        public ValidationException(string Message) : base(Message)
        {
        }
    }
    public class ParameterValidationException : Exception
    {
        private string _ParmName;
        private object _ActualValue;

        public ParameterValidationException(string ParmName, object ActualValue)
        {
            _ParmName = ParmName;
            _ActualValue = ActualValue;
        }

        public override string Message
        {
            get
            {
                string _Message = string.Format("Parameter: {0}, Actual Value: {1} ", _ParmName, _ActualValue);
                return _Message + base.Message;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
