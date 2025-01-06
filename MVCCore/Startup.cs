using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistance.Extensions;

namespace MVCCore
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
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.SwaggerDoc("admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });

                // Optionally, you can add custom logic to include/exclude endpoints in each document
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (docName == "admin")
                    {
                        return apiDesc.RelativePath.StartsWith("admin/");
                    }
                    return !apiDesc.RelativePath.StartsWith("admin/");
                });
            });

            services.AddPersistance(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Add this line for user identification
            app.UseAuthorization();

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;

                var result = System.Text.Json.JsonSerializer.Serialize(new { error = exception?.Message, stackTrace = exception?.StackTrace });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));

            // Enable Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.SwaggerEndpoint("swagger.json", "Admin API V1");

                // Optionally, you can set custom route prefixes
                c.RoutePrefix = string.Empty; // Set to empty string to serve Swagger UI at the app's root
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
        }
    }
}
