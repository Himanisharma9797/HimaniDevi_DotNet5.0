using Employee.Service.API.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Employee.Service.API.Task
{
    #region TaskInfo
    public class TaskInfo
    {    
        public string TitleOfTask { get; set; }

        public double EstimatedHours { get; set; }
        
        public CategoryEnum Category { get; set; }

        public string UserName { get; set; }

    }
    #endregion
    #region TaskDetailsByUserId
    public class TaskDetailsByUserID
    {
        public int TaskId { get; set; }
        public string TitleOfTask { get; set; }
        public double TotalBillCalculated { get; set; }
        public DateTime BilledOn { get; set; }
    }
    #endregion

}
