using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bonjwa.API.Config;
using Bonjwa.API.Services;
using Bonjwa.API.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bonjwa.API.Tasks
{
    public partial class BonjwaScrapeTask : IHostedService, IDisposable
    {
        private readonly ILogger<BonjwaScrapeTask> _logger;
        private readonly IDataStore _dataStore;
        private readonly BonjwaScrapeService _bonjwa;

        private Timer _timer;

        [LoggerMessage(0, LogLevel.Information, "{ClassName} running")]
        partial void LogClassRunning(string className);

        [LoggerMessage(1, LogLevel.Information, "{ClassName} is stopping")]
        partial void LogClassStopping(string className);

        [LoggerMessage(2, LogLevel.Debug, "Added {count} {itemType} to {storeName}")]
        partial void LogItemsAdded(int count, string itemType, string storeName);

        public BonjwaScrapeTask(ILogger<BonjwaScrapeTask> logger, IDataStore dataStore, BonjwaScrapeService bonjwa)
        {
            _logger = logger;
            _dataStore = dataStore;
            _bonjwa = bonjwa;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            LogClassRunning(this.GetType().Name);
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(AppConfig.ScrapeInterval));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var task = _bonjwa.ScrapeEventsAndScheduleAsync();
            task.Wait();

            var (eventItems, scheduleItems) = task.Result;

            _dataStore.SetEvents(eventItems);
            LogItemsAdded(eventItems.Count, eventItems.GetType().GetGenericArguments().Single().Name, _dataStore.GetType().Name);

            _dataStore.SetSchedule(scheduleItems);
            LogItemsAdded(scheduleItems.Count, scheduleItems.GetType().GetGenericArguments().Single().Name, _dataStore.GetType().Name);

            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            LogClassStopping(this.GetType().Name);
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer?.Dispose();
            }
        }
    }
}
