using LinksStorage.BlazorApp.Application.Database;
using LinksStorage.BlazorApp.Application.Events.Listeners;
using LinksStorage.BlazorApp.Application.Models;
using LinksStorage.BlazorApp.Application.Services.Auth;
using LinksStorage.BlazorApp.Application.Services;
using Spark.Library.Database;
using Spark.Library.Logging;
using Coravel;
using Microsoft.AspNetCore.Components.Authorization;
using Spark.Library.Auth;
using LinksStorage.BlazorApp.Application.Jobs;
using MudBlazor.Services;
using Spark.Library.Mail;

namespace LinksStorage.BlazorApp.Application.Startup;

public static class AppServiceRegistration
{
	public static void AddAppServices(this IServiceCollection services, IConfiguration config)
	{
		services.AddCustomServices();
		services.AddRazorPages();
		services.AddServerSideBlazor();
		services.AddDatabase<DatabaseContext>(config);
		services.AddLogger(config);
		services.AddAuthorization(config, new[] { CustomRoles.Admin, CustomRoles.User });
		services.AddAuthentication<IAuthValidator>(config);
		services.AddScoped<AuthenticationStateProvider, SparkAuthenticationStateProvider>();
		services.AddJobServices();
		services.AddScheduler();
		services.AddQueue();
		services.AddEventServices();
		services.AddEvents();
		services.AddMailer(config);
		services.AddComponentsServices();
	}

	private static void AddCustomServices(this IServiceCollection services)
	{
		// add custom services
		services.AddScoped<UsersService>();
		services.AddScoped<RolesService>();
		services.AddScoped<IAuthValidator, AuthValidator>();
		services.AddScoped<AuthService>();
		services.AddScoped<LinksRepository>();
		services.AddScoped<TagsRepository>();
	}

	private static void AddEventServices(this IServiceCollection services)
	{
		// add custom events here
		services.AddTransient<EmailNewUser>();
	}

	private static void AddJobServices(this IServiceCollection services)
	{
		// add custom background tasks here
		services.AddTransient<ExampleJob>();
	}

	private static void AddComponentsServices(this IServiceCollection services)
	{
		services.AddMudServices();
	}
}