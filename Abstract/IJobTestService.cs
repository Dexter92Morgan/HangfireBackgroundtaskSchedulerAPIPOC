namespace HangfireBackgroundtaskSchedulerAPIPOC.Abstract
{
    public interface IJobTestService
    {
        void FireAndForgetJob();
        void DelayedJob();
        void ContinuationJob();
        void ReccuringJob();
    }
}
