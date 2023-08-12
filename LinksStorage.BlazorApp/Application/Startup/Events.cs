using LinksStorage.BlazorApp.Application.Events.Listeners;
using LinksStorage.BlazorApp.Application.Events;
using Coravel.Events.Interfaces;
using Coravel;

namespace LinksStorage.BlazorApp.Application.Startup;

public static class Events
{
	public static IServiceProvider RegisterEvents(this IServiceProvider services)
	{
		IEventRegistration registration = services.ConfigureEvents();

		// add events and listeners here
		registration
			.Register<UserCreated>()
			.Subscribe<EmailNewUser>();

		return services;
	}
}