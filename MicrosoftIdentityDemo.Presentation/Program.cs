using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MicrosoftIdentity.Infrstructure.DBContext;
using MicrosoftIdentityDemo.Core.Domain.IdentityEntites;
using MicrosoftIdentityDemo.Core.Services.Implementation;
using MicrosoftIdentityDemo.Core.Services.Implementation.Interfaces;
using System.Security.Claims;

namespace MicrosoftIdentityDemo.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //registering dbcontext
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

            // registering identity services 
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 6; //number of characters required in password
                options.Password.RequireNonAlphanumeric = false; //is non-alphanumeric characters (symbols)
                                                                // required in password
                options.Password.RequireUppercase = true; //is at least one upper case character required in password
                options.Password.RequireLowercase = true; //is at least one lower case character required in password
                options.Password.RequireDigit = true; //is at least one digit required in password
                options.Password.RequiredUniqueChars = 1; //number of distinct characters required in password
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IUserService, UserService>();


            // add authorization policy to service collection
            builder.Services.AddAuthorization(options =>
            {
                // check if the request has authentication cookie or not , restrict access to the resource and will redirect to login page 
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                // fallback policy will be applied if no policy provided be the resource using Authorize attribute above the action method 
              //  options.FallbackPolicy = policy;

                // add custom policy
                options.AddPolicy("roleBasedPolicy", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
                        return email.Contains("@gmail.com");
                     });
                });
            });

            //configure the application cookie , redirect the user to specific URL when user has no access
            //to the requested action method or resource
            builder.Services.ConfigureApplicationCookie(
                options =>
                {
                    options.LoginPath = "/login-view";
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            // Always redirect to /login-view without appending ReturnUrl
                            context.Response.Redirect("/users/login-view");
                            return Task.CompletedTask;
                        }
                    };
                });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
