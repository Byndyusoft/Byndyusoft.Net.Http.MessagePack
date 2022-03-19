using System.Net.Http.Tests.Models;
using Microsoft.AspNetCore.Mvc;

namespace System.Net.Http.Tests.Functional
{
    [Controller]
    [Route("msgpack-formatter")]
    public class MessagePackFormatterController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] SimpleType model)
        {
            return Ok(model);
        }

        [HttpPut]
        public IActionResult Put([FromBody] SimpleType model)
        {
            return Ok(model);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(SimpleType.Create());
        }
    }
}