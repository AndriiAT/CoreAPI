using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistance.Extensions;
using System.Linq;
using System.Reflection;
using static Persistance.Extensions.ServiceCollectionExtension;

namespace MVCCore
{
    public class Startup
    {
        //// Seed roles and claims
        //private readonly IRoleSeeder roleSeeder;
        //// Seed users and claims
        //private readonly IUserSeeder userSeeder;

        public Startup(IConfiguration configuration) //, IRoleSeeder roleSeeder, IUserSeeder userSeeder
        {
            Configuration = configuration;
            //this.roleSeeder = roleSeeder;
            //this.userSeeder = userSeeder;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                // Get all controllers
                var controllers = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
                    .ToList();

                // Create a Swagger document for each controller
                foreach (var controller in controllers)
                {
                    var controllerName = controller.Name.Replace("Controller", string.Empty);
                    c.SwaggerDoc(controllerName.ToLower(), new OpenApiInfo { Title = $"{controllerName} API", Version = "v1" });
                    // Optionally, you can add custom logic to include/exclude endpoints in each document
                    c.DocInclusionPredicate((docName, apiDesc) =>
                    {
                        var controllerActionDescriptor = apiDesc.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
                        if (controllerActionDescriptor != null)
                        {
                            var controllerName = controllerActionDescriptor.ControllerName.Replace("Controller", string.Empty);
                            return docName == controllerName.ToLower();
                        }
                        return false;
                    });
                }

            });

            services.AddPersistance(Configuration);

            // Configure cookie authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/api/account/login";
                    options.LogoutPath = "/api/account/logout";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });

            services.AddAuthorization();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(a => a.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    var result = System.Text.Json.JsonSerializer.Serialize(new { error = exception?.Message, stackTrace = exception?.StackTrace });
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result);
                }));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Enable Swagger UI
            app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/admin/swagger.json", "Admin API V1");
            //    c.SwaggerEndpoint("/swagger/user/swagger.json", "User API V1");
            //});
            app.UseSwaggerUI(c =>
            {
                // Get all controllers
                var controllers = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
                    .ToList();

                // Create a Swagger endpoint for each controller
                foreach (var controller in controllers)
                {
                    var controllerName = controller.Name.Replace("Controller", string.Empty);
                    var isAdminController = controller.Namespace.Contains("Admin");
                    var docName = isAdminController ? "admin" : controllerName.ToLower();
                    c.SwaggerEndpoint($"/swagger/{docName}/swagger.json", $"{controllerName} API V1");
                }

                // Optionally, you can set custom route prefixes
                // c.RoutePrefix = string.Empty; // Set to empty string to serve Swagger UI at the app's root
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Redirect to Swagger UI when accessing the root URL
            app.Run(async (context) =>
            {
                context.Response.Redirect("/swagger");
            });

            //// Seed roles and claims
            //roleSeeder.SeedRolesAndClaims().Wait();
            //// Seed users and claims
            //userSeeder.SeedUsersAndClaims().Wait();
        }
    }
}
