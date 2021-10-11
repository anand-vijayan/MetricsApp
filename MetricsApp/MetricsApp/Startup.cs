using System;
using System.Net.Http;
using MetricsApp.Middlewares;
using MetricsApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

namespace MetricsApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add the ExceptionHandler middleware to handle all exceptions in one place to services collection in "Transient" scope
            services.AddTransient<ExceptionHandler>();

            services.AddControllers();

            //Since ILogger not supported in this method, we need to use traditional Console.Writeline to print messages.
            //And we need these log info only if trace is enabled in logging
            if (Configuration["Logging:LogLevel:Default"].CompareTo("Trace") == 0)
                Console.WriteLine("Adding HttpClient for WeatherService and setting Transient Fault handlers");

            //AddHttpClient::Enabling Single connection for the service using IHttpClientFactory. It can also be used for handling multiple APIs
            services.AddHttpClient<IWeatherService, WeatherService>(w =>
                {
                    w.BaseAddress = new Uri(Configuration["AppSettings:WeatherApiBaseAddress"]);
                })
                .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(10, _ => TimeSpan.FromSeconds(5))) //To handle 5XX and 408 http errors
                .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(5))) //To handle continuous Bad Requests
                .AddPolicyHandler(request =>
                {
                    if (request.Method == HttpMethod.Get)
                    {
                        return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(Convert.ToInt32(Configuration["AppSettings:GetMethodTimeOut"])));
                    }
                    return Policy.NoOpAsync<HttpResponseMessage>();
                }); //To limit the timeout of the request based on its type.

            if (Configuration["Logging:LogLevel:Default"].CompareTo("Trace") == 0)
                Console.WriteLine("Adding HttpClient for WeatherService and setting Transient Fault handlers completed");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Use the custom middleware created for Exception handling here
            app.UseMiddleware<ExceptionHandler>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
