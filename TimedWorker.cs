using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DemoWorkerService
{
    public class TimedWorker : IHostedService, IDisposable
    {
        private readonly ILogger<TimedWorker> _logger;
        private Timer _timer;
        private static object _lock = new object();
        private int _counter = 0;

        public TimedWorker(ILogger<TimedWorker> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(3));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void DoWork(object state)
        {
            _logger.LogDebug($"Try to execute iteration {_counter + 1} of MyJob ");
            if (Monitor.TryEnter(_lock))
            {
                try
                {
                    _logger.LogDebug($"Running MyJob iteration {_counter}");
                    Thread.Sleep(8000);
                }
                finally
                {
                    _counter++;
                    Monitor.Exit(_lock);
                }
            }
        }
    }
}