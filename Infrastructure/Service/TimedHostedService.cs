using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class TimedHostedService : BackgroundService
    {
        private readonly ILogger<TimedHostedService> _logger;

        private int _executionCount;
        private readonly IServiceScopeFactory _serviceProvider;
        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceScopeFactory serviceProvider)
        {
            _logger = logger;

            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            // When the timer should have no due-time, then do the work once now.
            await DoWork();

            using PeriodicTimer timer = new(TimeSpan.FromMinutes(1));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWork();
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Timed Hosted Service is stopping.");
            }
        }

        // Could also be a async method, that can be awaited in ExecuteAsync above
        private async Task DoWork()
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            try
            {
                //await unitOfWork.ClassRepository.UpdateClassStatus();
                //_logger.LogInformation(@class.ClassName);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
