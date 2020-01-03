using System.Threading;
using System.Threading.Tasks;

namespace DemoWorkerService.Services
{
    public interface IMyService
    {
        Task DoWorkAsync();
    }

    public class MyService : IMyService
    {
        public async Task DoWorkAsync()
        {
            await Task.Run(() => Thread.Sleep(8000));
        }
    }
}