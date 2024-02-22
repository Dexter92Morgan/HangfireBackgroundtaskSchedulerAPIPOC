using Hangfire;
using HangfireBackgroundtaskSchedulerAPIPOC.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace HangfireBackgroundtaskSchedulerAPIPOC.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IJobTestService _jobTestService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public HomeController(IBackgroundJobClient backgroundJobClient, IJobTestService jobTestService, IRecurringJobManager recurringJobManager)
        {
            _backgroundJobClient = backgroundJobClient;
            _jobTestService = jobTestService;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet]
        [Route("FireAndForgetJob")]
        public string FireAndForgetJob()
        {
            //Fire - and - Forget Jobs
            //Fire - and - forget jobs are executed only once and almost immediately after creation.
            var jobId = _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());

            return $"Job ID: {jobId}. Welcome user in Fire and Forget Job Demo!";
        }

        [HttpGet]
        [Route("DelayedJob")]
        public string DelayedJob()
        {
            //Delayed Jobs
            //Delayed jobs are executed only once too, but not immediately, after a certain time interval.
            var jobId = _backgroundJobClient.Schedule(() => _jobTestService.DelayedJob(), TimeSpan.FromSeconds(60));

            return $"Job ID: {jobId}. Welcome user in Delayed Job Demo!";
        }

        [HttpGet]
        [Route("ContinuousJob")]
        public string ContinuousJob()
        {
            //Fire - and - Forget Jobs
            //Fire - and - forget jobs are executed only once and almost immediately after creation.
            var parentJobId = _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());

            //Continuations
            //Continuations are executed when its parent job has been finished.
            _backgroundJobClient.ContinueJobWith(parentJobId, () => _jobTestService.ContinuationJob());

            return "Welcome user in Continuos Job Demo!";
        }

        [HttpGet]
        [Route("RecurringJob")]
        public string RecurringJobs()
        {
            //Recurring Jobs
            //Recurring jobs fire many times on the specified CRON schedule.
            _recurringJobManager.AddOrUpdate("jobId", () => _jobTestService.ReccuringJob(), Cron.Minutely);

            return "Welcome user in Recurring Job Demo!";

        }

        [HttpGet]
        [Route("BatchesJob")]
        public string BatchesJob()
        {
            //Batches - This option is available into hangfire Pro only
            //Batch is a group of background jobs that is created atomically and considered as a single entity.
            //Commenting the code as it's only available into Pro version


            //var batchId = BatchJob.StartNew(x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Batch Job 1"));
            //    x.Enqueue(() => Console.WriteLine("Batch Job 2"));
            //});

            return "Welcome user in Batches Job Demo!";
        }

        [HttpGet]
        [Route("BatchContinuationsJob")]
        public string BatchContinuationsJob()
        {
            //Batch Continuations - This option is available into hangfire Pro only
            //Batch continuation is fired when all background jobs in a parent batch finished.
            //Commenting the code as it's only available into Pro version

            //var batchId = BatchJob.StartNew(x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Batch Job 1"));
            //    x.Enqueue(() => Console.WriteLine("Batch Job 2"));
            //});

            //BatchJob.ContinueBatchWith(batchId, x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Last Job"));
            //});

            return "Welcome user in Batch Continuations Job Demo!";
        }
    }
}
