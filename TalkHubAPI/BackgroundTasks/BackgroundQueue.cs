using System.Collections.Concurrent;
using TalkHubAPI.Interfaces;

namespace TalkHubAPI.BackgroundTasks
{
    public class BackgroundQueue : IBackgroundQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> _Tasks;

        private SemaphoreSlim _Signal;

        private ConcurrentDictionary<Guid, string> _ConversionStatuses;

        public BackgroundQueue()
        {
            _Tasks = new ConcurrentQueue<Func<CancellationToken, Task>>();
            _Signal = new SemaphoreSlim(0);
            _ConversionStatuses = new ConcurrentDictionary<Guid, string>();
        }


        public void QueueTask(Func<CancellationToken, Task> task)
        {
            _Tasks.Enqueue(task);
            _Signal.Release();
        }

        public async Task<Func<CancellationToken, Task>> PopQueue(CancellationToken cancellationToken)
        {
            await _Signal.WaitAsync(cancellationToken);
            _Tasks.TryDequeue(out var task);

            return task;
        }

        public void AddStatus(Guid taskId, string status)
        {
            _ConversionStatuses[taskId] = status;
        }

        public async Task ProcessQueueAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var task = await PopQueue(cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                _ = Task.Run(async () =>
                {
                    using (var source = new CancellationTokenSource())
                    {
                        source.CancelAfter(TimeSpan.FromMinutes(1));
                        var timeoutToken = source.Token;

                        await task(timeoutToken);
                    }
                });
            }
        }

        public string GetStatus(Guid taskId)
        {
            if (_ConversionStatuses.ContainsKey(taskId))
            {
                return _ConversionStatuses[taskId];
            }

            return "Task not found";
        }
    }
}
