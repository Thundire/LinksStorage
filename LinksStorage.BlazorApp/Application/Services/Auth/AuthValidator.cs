using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Spark.Library.Auth;
using System.Security.Claims;

namespace LinksStorage.BlazorApp.Application.Services.Auth;

public class AuthValidator : IAuthValidator
{

	private readonly UsersService _usersService;

	public AuthValidator(UsersService usersService)
	{
		_usersService = usersService;
	}

	public async Task ValidateAsync(CookieValidatePrincipalContext context)
	{
		var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
		if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
		{
			// this is not our issued cookie
			await HandleUnauthorizedRequest(context);
			return;
		}

		var userIdString = claimsIdentity.FindFirst(ClaimTypes.UserData)?.Value;
		if (!int.TryParse(userIdString, out int userId))
		{
			await HandleUnauthorizedRequest(context);
			return;
		}

		var user = await _usersService.FindUserAsync(userId);
		if (user == null)
		{
			await HandleUnauthorizedRequest(context);
		}
	}

	private Task HandleUnauthorizedRequest(CookieValidatePrincipalContext context)
	{
		context.RejectPrincipal();
		return context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
	}
}