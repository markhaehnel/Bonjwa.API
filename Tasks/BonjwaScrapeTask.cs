using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bonjwa.API.Config;
using Bonjwa.API.Models;
using Bonjwa.API.Services;
using Bonjwa.API.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bonjwa.API.Tasks
{
    public class BonjwaScrapeTask : IHostedService, IDisposable
    {
        private readonly ILogger<BonjwaScrapeTask> _logger;
        private readonly IDataStore _dataStore;
        public BonjwaScrapeService _bonjwa { get; }

        private Timer _timer;

        public BonjwaScrapeTask(ILogger<BonjwaScrapeTask> logger, IDataStore dataStore, BonjwaScrapeService bonjwa)
        {
            _logger = logger;
            _dataStore = dataStore;
            _bonjwa = bonjwa;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{ClassName} running.", this.GetType().Name);
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(AppConfig.ScrapeInterval));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var task = _bonjwa.ScrapeEventsAndScheduleAsync();
            task.Wait();
            var (eventItems, scheduleItems) = task.Result;

            _dataStore.SetEvents(eventItems);
            _dataStore.SetSchedule(scheduleItems);
            _logger.LogDebug("Added {EventItemCount} events to {DataStoreType}", eventItems.Count, _dataStore.GetType().Name);
            _logger.LogDebug("Added {ScheduleItemCount} shows to {DataStoreType}", scheduleItems.Count, _dataStore.GetType().Name);

            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{ClassName} is stopping.", this.GetType().Name);
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
