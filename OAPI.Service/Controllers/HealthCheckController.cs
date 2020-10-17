using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace OAPI.Service.Controllers
{
    [ApiController]
    [Route("")]
    public class HealthCheckController : ControllerBase
    {
       [HttpGet]
        public IDictionary<string, object> Get()
        {
            return new Dictionary<string, object>()
            {
                            ["version"] = "v1.0", 
                            ["healthy"] = true, 
                            ["message"] = "Up and running", 
            };
        }
    }
}
