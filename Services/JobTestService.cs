using HangfireBackgroundtaskSchedulerAPIPOC.Abstract;

namespace HangfireBackgroundtaskSchedulerAPIPOC.Services
{
    public class JobTestService : IJobTestService
    {
        public void FireAndForgetJob()
        {
            Console.WriteLine("Welcome user in Fire and Forget Job Demo!");
        }

        public void DelayedJob()
        {
            Console.WriteLine("Welcome user in Delayed Job Demo!");
        }

        public void ContinuationJob()
        {
            Console.WriteLine("Welcome user in Fire and Forget Job Demo!");
        }

        public void ReccuringJob()
        {
            Console.WriteLine("Welcome user in Recurring Job Demo!");
        }
    }
}
