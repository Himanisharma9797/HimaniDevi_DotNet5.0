using Employee.Service.API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Service.API.Task
{
    #region CreateTask 
    public class CreateTaskRequest : RequestBase
    {
        public TaskInfo TaskInfo { get; set; } = new TaskInfo();
    }
    public class CreateTaskResponse : ResponseBase
    {
        public int TaskId { get; set; }
    }
    #endregion
    #region DisplayTask 
    public class DisplayTaskRequest : RequestBase
    {

    }
    public class DisplayTaskResponse : ResponseBase
    {
        public List<TaskInfo> TaskListInfo { get;set; }     

    }
    #endregion
    #region DeleteTask 
    public class DeleteTaskRequest : RequestBase
    {
        public int TaskId { get; set; }
    }
    public class DeleteTaskResponse : ResponseBase
    {

    }
    #endregion
    #region UpdateTask 
    public class UpdateTaskRequest : RequestBase
    {
        public int TaskId { get; set; }
        public string TitleOfTask { get; set; }
        public double CompletedHours { get; set; }
        
        public CategoryEnum Category { get; set; }
   
    }
    public class UpdateTaskResponse : ResponseBase
    {

    }
    #endregion
    #region GetTaskByTaskId 
    public class TaskByTaskIdRequest : RequestBase
    {
        public int TaskId { get; set; }
    }
    public class GetBilledTaskDetailsResponse : ResponseBase
    {
        public string TitleOfTask { get; set; }
       
        public double EstimatedtedHours { get; set; }
        public string Category { get; set; }
        public int BillingId { get; set; }
        public double TotalAmountCalculated { get; set; }
        public DateTimeOffset BilledOn { get; set; }
       
    }
    #endregion
    #region CompleteTask 
    public class CompleteTaskRequest:RequestBase
    {
        public int TaskId { get; set; }
        public DateTime StartDuration { get; set; }
        public DateTime EndDuration { get; set; }
    }
    public class CompleteTaskResponse:ResponseBase
    {
        public int UserId { get; set; }
        public double CompletedHours { get; set; }
        public double TotalAmountCalculated { get; set; }
    }
    #endregion
    #region GetTaskDetailsByUserId 
    public class GetTaskDetailsByUserIdRequest : RequestBase
    {
        public int UserId { get; set; }
    }
    public class GetDetailsByUserIdResponse : ResponseBase
    {
        public List<TaskDetailsByUserID> ListInfo{ get; set;}
        
    }
    #endregion
}
