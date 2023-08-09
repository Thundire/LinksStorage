using LinksStorage.BlazorApp.Application.Jobs;
using Coravel;

namespace LinksStorage.BlazorApp.Application.Startup;

public static class Scheduler
{
	public static IServiceProvider RegisterScheduledJobs(this IServiceProvider services)
	{
		services.UseScheduler(scheduler =>
		{
			// example scheduled job
			//scheduler
			//    .Schedule<ExampleJob>()
			//    .EveryFiveMinutes();
		});
		return services;
	}
}