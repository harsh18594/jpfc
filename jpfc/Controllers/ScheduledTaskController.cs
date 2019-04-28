using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace jpfc.Controllers
{
    public class ScheduledTaskController : JpfcController
    {
        private readonly ILogger _logger;
        private readonly IScheduledTaskService _scheduledTaskService;

        public ScheduledTaskController(ILogger<ScheduledTaskController> logger,
            IScheduledTaskService scheduledTaskService)
        {
            _logger = logger;
            _scheduledTaskService = scheduledTaskService;
        }

        public async Task<IActionResult> CopyPricesToToday()
        {
            _logger.LogInformation(GetLogDetails());

            await _scheduledTaskService.CopyPricesToTodayAsync();

            return Ok();
        }
    }
}
