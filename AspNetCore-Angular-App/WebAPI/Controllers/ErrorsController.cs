using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : Controller
    {
        [Route("error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            return BadRequest(new { Message = exception.Message });
        }
    }
    public class MyErrorResponse
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public MyErrorResponse(Exception ex)
        {
            Message = ex.Message;
            StackTrace = ex.ToString();
        }
    }
}
