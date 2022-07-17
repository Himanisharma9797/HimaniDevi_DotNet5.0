using Employee.Service.API.Task;
using Employee.Service.DAL;
using Employee.Service.Logic.Task;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Service.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]


    public class Task : ControllerBase
    {
        #region Private readonly property
        private readonly EmployeeDbContext _employeeDbContext;
        private IConfiguration _configuration;
        #endregion
        #region Constructor
        public Task(EmployeeDbContext employeeDbContext, IConfiguration configuration)
        {
            _employeeDbContext = employeeDbContext;
            _configuration = configuration;
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Create task
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("CreateTask")]
        public ActionResult<CreateTaskResponse> CreateTask(CreateTaskRequest request)
        {
            try
            {
                var taskLogic = new TaskLogic(_employeeDbContext,_configuration);
                return taskLogic.CreateTask(request);

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);

            }
        }
        /// <summary>
        /// Mark task as complete
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("CompleteTask")]
        public ActionResult<CompleteTaskResponse> CompleteTask(CompleteTaskRequest request)
        {
            try
            {
                var taskLogic = new TaskLogic(_employeeDbContext,_configuration);
                return taskLogic.CompleteTask(request);

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);

            }
        }
        /// <summary>
        /// Display task details
        /// </summary>
        /// <returns></returns>
        [HttpGet("DisplayTaskDetails")]
        public ActionResult<DisplayTaskResponse> DisplayTask()
        {
            try
            {
                var request = new DisplayTaskRequest();
                var taskLogic = new TaskLogic(_employeeDbContext,_configuration);
                return taskLogic.DisplayTask(request);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);

            }
        }
        /// <summary>
        /// Delete task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpDelete("{taskId}/DeleteTask")] 
        public ActionResult<DeleteTaskResponse> DeleteTask(int taskId)
        {
            try
            {
                var Request = new DeleteTaskRequest() { TaskId = taskId };
                var taskLogic = new TaskLogic(_employeeDbContext,_configuration);
                return taskLogic.DeleteTask(Request);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);

            }
        }
        /// <summary>
        /// Update task
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateTask")]
        public ActionResult<UpdateTaskResponse> UpdateTask(UpdateTaskRequest request)
        {
            try
            {

                var taskLogic = new TaskLogic(_employeeDbContext,_configuration);
                return taskLogic.UpdateTask(request);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);

            }
        }
        /// <summary>
        /// Getting details of task that is already billed
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        [HttpGet("GetBilledTaskDetails")]
        public ActionResult<GetBilledTaskDetailsResponse> GetBilledTaskDetails(int taskID)
        {
            try
            {
                var taskLogic = new TaskLogic(_employeeDbContext,_configuration);
                return taskLogic.GetBilledTaskDetails(new TaskByTaskIdRequest { TaskId = taskID });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        /// <summary>
        /// Getting task details by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("{userId}/GetDetailsByUserId")]
        public ActionResult<GetDetailsByUserIdResponse> GetDetailsByUserId(int userId)
        {
            try
            {
                var Request = new GetTaskDetailsByUserIdRequest() { UserId = userId };
                var taskLogic = new TaskLogic(_employeeDbContext, _configuration);
                return taskLogic.GetTaskDetailsByUserId(Request);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);

            }
        }
        #endregion
    }
}
