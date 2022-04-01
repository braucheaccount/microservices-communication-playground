using MassTransit;
using SharedLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService
{
    public class MyBackgroundService : BackgroundService, IConsumer<Fault<ExceptionRequest>>
    {
        private readonly ILogger<MyBackgroundService> _logger;

        public MyBackgroundService(ILogger<MyBackgroundService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // when the app stops
            while(!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking Exception Queue..");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping the BackgroundService now..");
            return base.StopAsync(cancellationToken);
        }

        public Task Consume(ConsumeContext<Fault<ExceptionRequest>> context)
        {
            var data = context.Message;
            throw new NotImplementedException();
        }
    }
}
