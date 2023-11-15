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
            while (!stoppingToken.IsCancellationRequested)
            {
                var task = await _Queue.PopQueue(stoppingToken);

                if (stoppingToken.IsCancellationRequested)
                {
                    return;
                }

                using (var source = new CancellationTokenSource())
                {
                    source.CancelAfter(TimeSpan.FromMinutes(1));
                    var timeoutToken = source.Token;

                    await task(timeoutToken);
                }
            }
        }
    }
}
