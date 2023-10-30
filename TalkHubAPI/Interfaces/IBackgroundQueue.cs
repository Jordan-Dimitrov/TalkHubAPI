namespace TalkHubAPI.Interfaces
{
    public interface IBackgroundQueue
    {
        void QueueTask(Func<CancellationToken, Task> task);
        Task<Func<CancellationToken, Task>> PopQueue(CancellationToken cancellationToken);
    }
}
