using BusinessLayer.Middlewares;
using BusinessLayer.ValidationRules;
using Core.ExtensionService.BlogService;
using Core.Hubs;
using Core.Repository;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CoreDemo
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<Context>();

			services.AddIdentity<User, Role>(x =>
			{
				x.Password.RequireUppercase = false;
				x.Password.RequireLowercase = false;
				x.Password.RequireDigit = false;
				x.Password.RequireNonAlphanumeric = false;
				x.Password.RequiredLength = 8;
			})
				.AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();

			services.Configure<SecurityStampValidatorOptions>(options =>
			{
				options.ValidationInterval = TimeSpan.Zero;
			});
			services.AddTransient<IEmailSender, EmailSender>();
			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.HttpOnly = true;
				options.SlidingExpiration = true;
				options.LoginPath = "/Login/Index";
				options.LogoutPath = "/Login/LogOut";
				options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Login/AccessDenied");
				options.ExpireTimeSpan = System.TimeSpan.FromDays(7);
			});

			services.Configure<DataProtectionTokenProviderOptions>(opts=>opts.TokenLifespan = TimeSpan.FromHours(7));
			services.AddControllersWithViews();
			services.AddMvc(
				config =>
				{
					var policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();

					config.Filters.Add(new AuthorizeFilter(policy));
				});

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
			{
				x.LoginPath = "/Login/Index";
			});

			services.AddValidatorsFromAssemblyContaining<WriterValidator>();
			services.AddValidatorsFromAssemblyContaining<BlogValidator>();
			services.AddValidatorsFromAssemblyContaining<MessageValidator>();

			services.AddFluentValidationAutoValidation();
			services.AddFluentValidationClientsideAdapters();

			services.AddSignalR();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseExceptionHandler("/Home/Error");
			app.UseHsts();

			app.UseMiddleware<VisitorMiddleware>();

			app.UseStatusCodePagesWithReExecute("/ErrorPage/Error1", "?code={0}");

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "areas",
					pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

				endpoints.MapHub<PostHub>("/post-hub");
			});
		}
	}
}
