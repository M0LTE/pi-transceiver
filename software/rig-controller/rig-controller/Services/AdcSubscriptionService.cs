using System.Collections.Concurrent;

namespace rig_controller.Services
{
    public interface IAdcSubscriptionService
    {
        Task<Guid> Subscribe(Func<AdcReading, Task> callback, params int[] channelsToMonitor);
        Task<bool> Unsubscribe(Guid guid);
    }

    public class AdcSubscriptionService : IAdcSubscriptionService
    {
        private readonly ConcurrentDictionary<Guid,(int[] channels, Func<AdcReading, Task> callback)> _subscriptions = new();
        private readonly Timer _timer;
        private readonly ILogger<AdcSubscriptionService> _logger;
        private readonly IAdcChannelReaderService _adcChannelReaderService;

        public AdcSubscriptionService(ILogger<AdcSubscriptionService> logger, IAdcChannelReaderService adcChannelReaderService)
        {
            _timer = new(Tick, null, 0, 100);
            _logger = logger;
            _adcChannelReaderService = adcChannelReaderService;
        }

        ~AdcSubscriptionService()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private async void Tick(object? state)
        {
            var tasks = new List<Task>();

            AdcReading[] data = await ReadChannels(0..3);

            foreach (var kvp in _subscriptions)
            {
                var (channels, callback) = kvp.Value;

                foreach (var channel in channels)
                {
                    tasks.Add(CallSafely(callback(data[channel])));
                }
            }

            await Task.WhenAll(tasks.ToArray());
        }

        private async Task CallSafely(Task task)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught in callback");
            }
        }

        private async Task<AdcReading[]> ReadChannels(Range channels)
        {
            var tasks = new List<Task<AdcReading>>();

            for (int i = channels.Start.Value; i < channels.End.Value; i++)
            {
                tasks.Add(_adcChannelReaderService.Read(0, i));
            }

            await Task.WhenAll(tasks);

            return tasks.Select(t => t.Result).ToArray();
        }

        public Task<Guid> Subscribe(Func<AdcReading, Task> callback, params int[] channelsToMonitor)
        {
            var id = Guid.NewGuid();
            _subscriptions.TryAdd(id, (channelsToMonitor, callback));
            return Task.FromResult(id);
        }

        public Task<bool> Unsubscribe(Guid guid)
        {
            return Task.FromResult(_subscriptions.TryRemove(guid, out _));
        }
    }
}