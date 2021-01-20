using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.Tests.Controllers
{
    public class ControllerTestBase
    {
        protected T ValidateOkResult<T>(IActionResult result)
        {
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okObject = result as OkObjectResult;
            Assert.IsInstanceOfType(okObject.Value, typeof(T));
            return (T)okObject.Value;
        }
    }
}
