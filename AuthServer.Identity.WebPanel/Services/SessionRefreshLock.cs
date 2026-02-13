using System.Collections.Concurrent;
namespace AuthServer.Identity.WebPanel.Services
{
    public static class SessionRefreshLock
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> Locks = new();

        public static async Task<IDisposable> AcquireAsync(string key, CancellationToken ct = default)
        {
            var sem = Locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
            await sem.WaitAsync(ct);
            return new Releaser(key, sem);
        }

        private sealed class Releaser : IDisposable
        {
            private readonly string _key;
            private readonly SemaphoreSlim _sem;

            public Releaser(string key, SemaphoreSlim sem)
            {
                _key = key; _sem = sem;
            }

            public void Dispose()
            {
                _sem.Release();
                // İstersen burada cleanup yapabilirsin (şimdilik basit bırakalım)
            }
        }
    }
}
