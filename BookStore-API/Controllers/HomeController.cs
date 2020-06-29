using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore_API.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore_API.Controllers
{
    /// <summary>
    /// THIS IS A TEST CONTROLLER! 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private readonly ILoggerServices _logger;

        public HomeController(ILoggerServices logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Action 
        /// </summary>
        /// <returns></returns>
        // GET: api/<HomeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInfo("AccesedHomeController");
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<HomeController>/5
        [HttpGet("{id}", Name ="Get")]
        public string Get(int id)
        {
            _logger.LogDebug("Got A value!");
            return "value";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        // POST api/<HomeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _logger.LogError("This Is An Error");
        }

        // PUT api/<HomeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HomeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogWarn("This Is An Warrning");
        }
    }
}
