using LinksStorage.BlazorApp.Application.Models;
using Coravel.Events.Interfaces;

namespace LinksStorage.BlazorApp.Application.Events;

public class UserCreated : IEvent
{
	public User User { get; set; }

	public UserCreated(User user)
	{
		this.User = user;
	}
}