using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.DBModel;
using Services.Users.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Controllers
{
    [ApiController]
    public class JobAidController : ControllerBase
    {
        private readonly JobsContext JobsContext;
        private readonly AuthenticationController AuthenticationController;

        public JobAidController(JobsContext JobsContext, AuthenticationController AuthenticationController)
        {
            this.JobsContext = JobsContext;
            this.AuthenticationController = AuthenticationController;
        }

        [Route("/jobs/list"), HttpGet]
        public async Task<ActionResult> ListJobs(ulong token)
        {
            var authcheck = await AuthenticationController.Check(token);
            if (authcheck is OkObjectResult okres)
            {
                dynamic values = okres.Value;
                return Ok(new
                {
                    values.username,
                    jobs = await JobsContext.Jobs.ToArrayAsync(),
                });
            }

            return Unauthorized();
        }
    }
}
