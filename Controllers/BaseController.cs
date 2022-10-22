using Microsoft.AspNetCore.Mvc;
using Utilities.Enums;
using Utilities.Exceptions;

namespace TicketSystem.Controllers
{
    public class BaseController : Controller
    {

        public BaseController()
        {
        }
        protected ActionResult TryCatchLog(Func<ActionResult> function)
        {
            try
            {
                return function.Invoke();
            }
            catch (CustomException ex)
            {
                return StatusCode(500, ex.Message);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(500, ValidationKeysEnum.ServerError);
            }
        }

        protected async Task<IActionResult> TryCatchLogAscync(Func<Task<IActionResult>> function)
        {
            try
            {
                return await function.Invoke();
            }
            catch (CustomException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (AggregateException aggEx)
            {
                Exception? ex = aggEx.Flatten()?.InnerExceptions?.FirstOrDefault();
                logger.LogError(ex, ex?.Message);
                return StatusCode(500, ex?.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(500, ValidationKeysEnum.ServerError);
            }
        }

    }
}
