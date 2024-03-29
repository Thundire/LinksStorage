using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using LinksStorage.BlazorApp.Application.Models;
using LinksStorage.BlazorApp.Application.Services.Auth;
using Spark.Library.Auth;

namespace LinksStorage.BlazorApp.Pages.Auth;

public class LoginModel : PageModel
{
	private readonly IConfiguration _configuration;
	private readonly UsersService _usersService;
	private readonly AuthService _cookieService;

	public LoginModel(
		UsersService usersService,
		IConfiguration configuration,
		AuthService cookieService)
	{
		_usersService  = usersService;
		_configuration = configuration;
		_cookieService = cookieService;
	}

	[BindProperty] public LoginForm Input { get; set; } = new();

	public void OnGet()
	{

	}

	public async Task<IActionResult> OnPost()
	{

		if (!ModelState.IsValid)
			return Page();

		if (Input == null)
		{
			return BadRequest("user is not set.");
		}

		var user = await _usersService.FindUserAsync(Input.Email, _usersService.GetSha256Hash(Input.Password));

		if (user == null)
		{
			ModelState.AddModelError("FailedLogin", "Login Failed: Your email or password was incorrect");
			return Page();
		}

		var cookieExpirationDays = _configuration.GetValue("Spark:Auth:CookieExpirationDays", 5);
		var cookieClaims = await _cookieService.CreateCookieClaims(user);

		await HttpContext.SignInAsync(
			CookieAuthenticationDefaults.AuthenticationScheme,
			cookieClaims,
			new AuthenticationProperties
			{
				IsPersistent = Input.RememberMe,
				IssuedUtc    = DateTimeOffset.UtcNow,
				ExpiresUtc   = DateTimeOffset.UtcNow.AddDays(cookieExpirationDays)
			});

		return Redirect("~/dashboard");
	}

	public class LoginForm
	{
		[Display(Name = "Email")]
		[Required(ErrorMessage = "Please enter a valid email address")]
		public string Email { get; set; } = string.Empty;

		[Display(Name = "Password")]
		[Required(ErrorMessage = "Invalid password")]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;
		public bool RememberMe { get; set; } = false;

	}
}