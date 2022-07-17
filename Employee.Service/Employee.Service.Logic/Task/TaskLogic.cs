using Billing.Service.Client;
using Employee.Service.API.Common;
using Employee.Service.API.Task;
using Employee.Service.DAL;
using Employee.Service.DAL.Task;
using Employee.Service.Logic.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Service.Logic.Task
{
    public class TaskLogic
    {
        #region Private ReadOnly Properties
        private readonly EmployeeDbContext _employeeDbContext;
        private readonly IConfiguration _configuration;
        #endregion
        #region Constructor
        public TaskLogic(EmployeeDbContext employeeDbContext, IConfiguration configuration)
        {
            _employeeDbContext = employeeDbContext;
            _configuration = configuration;
        }
        #endregion
        #region Public Methods

        public CreateTaskResponse CreateTask(CreateTaskRequest request)
        {
            try
            {                
                Validation.RequiredParameter("Request", request);
                Validation.RequiredParameter("TaskInfo", request.TaskInfo);
                Validation.RequiredParameter("TaskTitle", request.TaskInfo.TitleOfTask);
                Validation.RequiredParameter("EstimatedHours", request.TaskInfo.EstimatedHours);
                Validation.RequiredDoubleParameter("EstimatedHours", request.TaskInfo.EstimatedHours);
                Validation.RequiredParameter("UserName", request.TaskInfo.UserName);
                Validation.RequiredParameter("Category", request.TaskInfo.Category);
               
                var userId = GetUserId(request.TaskInfo.UserName).GetValueOrDefault();
                if (userId == null)
                {
                    return new CreateTaskResponse()
                    {
                         ResponseMsg="UserId not found , task can not created",
                          ResponseResult=ResponseResultEnum.Warning.ToString()
                    };
                }

                var taskDetails = new TaskDetails
                {
                    TitleOfTask = request.TaskInfo.TitleOfTask,
                    EstimatedHours = request.TaskInfo.EstimatedHours,
                    Category = request.TaskInfo.Category.ToString(),
                    UserId = userId
                };

                _employeeDbContext.taskDetails.Add(taskDetails);
                _employeeDbContext.SaveChanges();
                return new CreateTaskResponse
                {                  
                    TaskId = GetTaskId(request.TaskInfo.TitleOfTask).GetValueOrDefault(),
                    ResponseMsg = "Task created successfully",
                    ResponseResult = ResponseResultEnum.Success.ToString()
                };
            }
            catch(Exception ex)
            {
                ex.Data.Add("Request", request);            
                return ExceptionHelpers.ProcessException<CreateTaskResponse>(request, ex);
            }
        }
        public DisplayTaskResponse DisplayTask(DisplayTaskRequest request)
        {
             Validation.RequiredParameter("Request", request);
            try
            { 

                var taskDetails = _employeeDbContext.taskDetails.ToList();
                if(taskDetails==null||taskDetails.Count==0)
                {
                    return new DisplayTaskResponse
                    {
                        ResponseMsg = "Tasks not available",
                        ResponseResult = ResponseResultEnum.Warning.ToString()
                    };
                }
                var _listinfo = JsonConvert.DeserializeObject<List<TaskInfo>>(JsonConvert.SerializeObject(taskDetails));
                return new DisplayTaskResponse
                {
                    TaskListInfo = _listinfo,
                    ResponseMsg= "Available tasks",
                    ResponseResult=ResponseResultEnum.Success.ToString()
                };
            }
            
            catch(Exception ex)
            {        
                return ExceptionHelpers.ProcessException<DisplayTaskResponse>(request, ex);
            }

        }

        public GetBilledTaskDetailsResponse GetBilledTaskDetails(TaskByTaskIdRequest request)
        {
            Validation.RequiredParameter("Request", request);
            Validation.RequiredParameter("TaskId", request.TaskId);
            Validation.RequiredIntParameter("TaskId", request.TaskId);
            try
            {
                var taskDetail = _employeeDbContext.taskDetails.SingleOrDefault(x => x.TaskId == request.TaskId);
                if (taskDetail != null)
                {
                    var baseURL = _configuration["Clients:BillingServiceUri"];
                    HttpClient _httpClient = new HttpClient();
                    var _client = new Client(baseURL, _httpClient);
                    var response = _client.BillingDetailsAsync(taskDetail.TaskId).GetAwaiter().GetResult();
                    if (response.ResponseResult == nameof(ResponseResultEnum.Success))
                    {
                        var info = JsonConvert.DeserializeObject<RecordResponse>(JsonConvert.SerializeObject(response));

                        return new GetBilledTaskDetailsResponse()
                        {
                            TitleOfTask = taskDetail.TitleOfTask,
                            EstimatedtedHours = taskDetail.EstimatedHours,
                            Category = taskDetail.Category,
                            BillingId = info.BillingId,
                            BilledOn = info.BilledOn,
                            TotalAmountCalculated = info.TotalAmountCalculated,
                            ResponseResult = ResponseResultEnum.Success.ToString(),
                            ResponseMsg = "Task details found successfully"

                        };

                        return new GetBilledTaskDetailsResponse()
                        {
                            ResponseResult = ResponseResultEnum.Warning.ToString(),
                            ResponseMsg = "Task details not found."

                        };

                    }

                }
                return new GetBilledTaskDetailsResponse
                {
                    ResponseResult = ResponseResultEnum.ServiceFault.ToString(),
                    ResponseMsg = "Client not available "

                };
            }
            catch (Exception ex)
            {
                ex.Data.Add("TaskId", request.TaskId);
                return ExceptionHelpers.ProcessException<GetBilledTaskDetailsResponse>(request, ex);
            }     

        }

        public DeleteTaskResponse DeleteTask(DeleteTaskRequest request)
        {
            Validation.RequiredParameter("Request", request);
            Validation.RequiredParameter("TaskId", request.TaskId);
            Validation.RequiredIntParameter("TaskId", request.TaskId);
            try
            {
                var taskId = _employeeDbContext.taskDetails.Find(request.TaskId);
                if (taskId != null)
                {
                    _employeeDbContext.taskDetails.Remove(taskId);
                    _employeeDbContext.SaveChanges();
                    return new DeleteTaskResponse
                    {
                        ResponseMsg = "Task deleted successfully",
                        ResponseResult = ResponseResultEnum.Success.ToString()
                    };
                }
                return new DeleteTaskResponse
                {
                    ResponseResult = ResponseResultEnum.Warning.ToString(),
                    ResponseMsg = "Deletion unsuccessful"
                };
            }
            catch(Exception ex)
            {
                ex.Data.Add("TaskId", request.TaskId);
                return ExceptionHelpers.ProcessException<DeleteTaskResponse>(request, ex);
            }
        }
        public UpdateTaskResponse UpdateTask(UpdateTaskRequest request)
        {
            try
            {
                Validation.RequiredParameter("Request", request);
                Validation.RequiredParameter("TaskId", request.TaskId);
                Validation.RequiredIntParameter("TaskId", request.TaskId);
                Validation.RequiredParameter("TaskTitle", request.TitleOfTask);
                Validation.RequiredParameter("CompletedHours", request.CompletedHours);
                Validation.RequiredDoubleParameter("CompletedHours", request.CompletedHours);
                Validation.RequiredParameter("Category", request.Category);
               
                var taskInfo = _employeeDbContext.taskDetails.FirstOrDefault(x => x.TaskId == request.TaskId);
                if (taskInfo == null)
                {
                    return new UpdateTaskResponse
                    {
                        ResponseMsg = "Can not update the task details",
                        ResponseResult = ResponseResultEnum.Warning.ToString()
                    };
                }

                taskInfo.TitleOfTask = request.TitleOfTask;
                taskInfo.Category = request.Category.ToString();
             
                _employeeDbContext.SaveChanges();
                return new UpdateTaskResponse
                {
                    ResponseMsg = "Task updated successfully",
                    ResponseResult = ResponseResultEnum.Success.ToString()
                };
            }
            catch(Exception ex)
            {
                ex.Data.Add("TaskId", request.TaskId);
                ex.Data.Add("TitleOdTask", request.TitleOfTask);
                return ExceptionHelpers.ProcessException<UpdateTaskResponse>(request, ex);
            }         
        }
        public CompleteTaskResponse CompleteTask(CompleteTaskRequest request)
        {
            try
            {
                Validation.RequiredParameter("Request", request);
                Validation.RequiredParameter("TaskId", request.TaskId);
                Validation.RequiredIntParameter("TaskId", request.TaskId);
                Validation.RequiredParameter("StartDuration", request.StartDuration);
                Validation.RequiredParameter("EndDuration", request.EndDuration);

                var taskInfo = _employeeDbContext.taskDetails.FirstOrDefault(x => x.TaskId == request.TaskId);
                if (taskInfo == null)
                {
                    return new CompleteTaskResponse
                    {
                        ResponseMsg = "Task details not found",
                        ResponseResult = ResponseResultEnum.Warning.ToString()
                    };
                }
                var CompletedHours = request.StartDuration.TimeOfDay.TotalHours - request.EndDuration.TimeOfDay.TotalHours;
                taskInfo.CompletedHours = CompletedHours;

                _employeeDbContext.SaveChanges();
                var baseURL = _configuration["Clients:BillingServiceUri"];
                HttpClient _httpClient = new HttpClient();
                var _client = new Client(baseURL, _httpClient);
                var response = _client.CalculateAmountAsync(taskInfo.TaskId, taskInfo.Category, taskInfo.CompletedHours, taskInfo.UserId).GetAwaiter().GetResult();
                if (response.ResponseResult == nameof(ResponseResultEnum.Success))
                {
                    var info = JsonConvert.DeserializeObject<CalculateAmountResponse>(JsonConvert.SerializeObject(response));
                    return new CompleteTaskResponse
                    {
                        CompletedHours = CompletedHours,
                        TotalAmountCalculated = info.TotalAmountCalculated,
                        UserId = taskInfo.UserId,
                        ResponseMsg = info.ResponseMessage,
                        ResponseResult = info.ResponseResult
                    };
                }

                return new CompleteTaskResponse
                {
                    ResponseResult = ResponseResultEnum.ServiceFault.ToString(),
                    ResponseMsg = "Client not available "

                };
            }
            catch (Exception ex)
            {
                ex.Data.Add("TaskId", request.TaskId);
                ex.Data.Add("StartDuration", request.StartDuration);
                return ExceptionHelpers.ProcessException<CompleteTaskResponse>(request, ex);
            }
        }
        public GetDetailsByUserIdResponse GetTaskDetailsByUserId(GetTaskDetailsByUserIdRequest request)
        {
            try
            {
                Validation.RequiredParameter("Request", request);
                Validation.RequiredParameter("UserId", request.UserId);
                Validation.RequiredIntParameter("UserId", request.UserId);

                var taskInfo = _employeeDbContext.taskDetails.Where(x => x.UserId == request.UserId).ToList();
                var userId = taskInfo.Select(x => x.UserId).FirstOrDefault();
                int taskId = taskInfo.Select(x => x.TaskId).FirstOrDefault();
                if (taskInfo == null || taskInfo.Count == 0)
                {
                    return new GetDetailsByUserIdResponse
                    {
                        ResponseMsg = "Task details not found",
                        ResponseResult = ResponseResultEnum.Warning.ToString()
                    };
                }
                else
                {
                    var baseURL = _configuration["Clients:BillingServiceUri"];
                    HttpClient _httpClient = new HttpClient();
                    var _client = new Client(baseURL, _httpClient);
                    var response = _client.BillingDetailsByUserIDAsync(userId).GetAwaiter().GetResult();
                    if (response.ResponseResult == nameof(ResponseResultEnum.Success))
                    {
                        var info = JsonConvert.DeserializeObject<List<TaskDetailsByUserID>>(JsonConvert.SerializeObject(taskInfo));
                        return new GetDetailsByUserIdResponse
                        {
                            ListInfo = info,
                            ResponseMsg = "Task details Found",
                            ResponseResult = ResponseResultEnum.Success.ToString()

                        };
                    }
                }
                return new GetDetailsByUserIdResponse
                {
                    ResponseResult = ResponseResultEnum.ServiceFault.ToString(),
                    ResponseMsg = "Client not available "

                };

            }
            catch (Exception ex)
            {
                ex.Data.Add("UserId", request.UserId);        
                return ExceptionHelpers.ProcessException<GetDetailsByUserIdResponse>(request, ex);
            }
        }
        #endregion
        #region Private Methods
        private int? GetUserId(string userName)
        {
            var id = _employeeDbContext.UserInfos.Where(x => x.Email == userName).Select(x => x.UserId);
            if (id != null)
                return id.FirstOrDefault();
            return null;
        }
        private int? GetTaskId(string UserName)
        {
            var Taskid = _employeeDbContext.taskDetails.Where(x => x.TitleOfTask == UserName).Select(x => x.TaskId);
            if (Taskid != null)
                return Taskid.FirstOrDefault();
            return null;
        }
        #endregion
    }
}
