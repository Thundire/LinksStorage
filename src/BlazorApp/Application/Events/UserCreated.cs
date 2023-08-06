using Coravel.Events.Interfaces;
using LinksStorage.BlazorApp.Application.Models;

namespace LinksStorage.BlazorApp.Application.Events;

public class UserCreated : IEvent
{
	public User User { get; set; }

	public UserCreated(User user)
	{
		this.User = user;
	}
}