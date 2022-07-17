using Employee.Service.API.Common;
using Employee.Service.DAL.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Service.DAL.Task
{
    #region TaskDetails
    public class TaskDetails
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public string TitleOfTask { get; set; }
        [Required]
        public double CompletedHours { get; set; }
        [Required]
        public double EstimatedHours { get; set; }
        [Required(ErrorMessage = "0-easy , 1-hard, 2-medium")]
        public string Category { get; set; }
        [Required]
        public DateTime StartDuration { get; set; }
        [Required]
        public DateTime EndDuration { get; set; }
        public int UserId { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
#endregion