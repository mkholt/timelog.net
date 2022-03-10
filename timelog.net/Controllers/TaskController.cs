using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using timelog.net.Data;
using timelog.net.Models;

namespace timelog.net.Controllers
{
    [Route("[controller]")]
    public class TaskController : BaseController<ProjectTask>
    {
        protected override string EntityName => "Task";
        
        public TaskController(IRepository<ProjectTask> taskRepository) : base(taskRepository)
        {
        }
    }
}