using TalkHubAPI.Interfaces;

namespace TalkHubAPI.Helper
{
    public class QueueService : BackgroundService
    {
        private IBackgroundQueue _Queue;

        public QueueService(IBackgroundQueue queue)
        {
            _Queue = queue;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _Queue.ProcessQueueAsync(stoppingToken);
        }
    }
}
