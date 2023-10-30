using System.Collections.Concurrent;
using TalkHubAPI.Interfaces;

namespace TalkHubAPI.BackgroundTasks
{
    public class BackgroundQueue : IBackgroundQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> _Tasks;

        private SemaphoreSlim _Signal;

        public BackgroundQueue()
        {
            _Tasks = new ConcurrentQueue<Func<CancellationToken, Task>>();
            _Signal = new SemaphoreSlim(0);
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
    }
}
