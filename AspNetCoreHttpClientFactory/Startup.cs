using AspNetCoreHttpClientFactory.Business;
using AspNetCoreHttpClientFactory.Core.Client.Base;
using AspNetCoreHttpClientFactory.Core.TypedClients;
using AspNetCoreHttpClientFactory.Helper.Delegating;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using static HttpClientFactoryEnum;

namespace AspNetCoreHttpClientFactory
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

            services.AddControllersWithViews();

            //Test
            //services.Configure<FormOptions>(x => x.MultipartBodyLengthLimit = 4294967295);

            //AppSettings
            services.AddSingleton(Configuration.GetSection("ConfigHelp").Get<ConfigHelp>());

            //---Policy

            //2 P
            var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .RetryAsync(3);
            var noOp = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

            //3 P
            var registry = services.AddPolicyRegistry();
            var timeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
            var longTimeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));
            registry.Add("regular", timeout);
            registry.Add("long", longTimeout);


            //IOC
            services.AddScoped<IHttpClientFactoryBase, HttpClientFactoryBase>();
            services.AddScoped<ICountryService, CountryManager>();
            //

            // ---- IOC HttpClientFactory----//
            services.AddTransient<TimingHandler>();
            services.AddTransient<AuthTokenHandler>();
            services.AddTransient<LogHandler>();
            services.AddTransient<ValidateHeaderHandler>();

            // ---- Base HttpClientFactory----//
            services.AddHttpClient();



            // ---- Name HttpClientFactory----//
            services.AddHttpClient(namedClients.FarmerExpert.ToString(), c =>
            {
                c.BaseAddress = new Uri(ConfigHelp.WebApiURL);
                /*
                c.DefaultRequestHeaders.Add("CustomHeaderKey", "It-is-a-HttpClientFactory-Sample");
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                c.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true,
                    NoStore = true,
                    MaxAge = new TimeSpan(0),
                    MustRevalidate = true
                };
                */

            });
            /*
            .AddPolicyHandlerFromRegistry("regular");
            */
            /*
            .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOp);
            */
            /*
            .AddPolicyHandler(Policy.TimeoutAsync(20).AsAsyncPolicy<HttpResponseMessage>()); 
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10))) //Time out policy ekleyerek, gidecek requestlerin en fazla 10 sn bekledikten sonra eðer response alamazsa doðrudan polly tarafýndan hataya düþürülmesini saðladýk
            .AddTransientHttpErrorPolicy(p => p.RetryAsync(3)); // Haberleþme sýrasýnda geçici sorunlar olduðunda 3 kere yeniden denenmesini istedik. Anlýk internet kayýplarý, server anlýk olarak response verememesi available olamamasý gibi durumlarda request 3 kere yeniden denenecek.(RetryPattern)
            */
            /*
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            }));
            */
            /*
            .AddHttpMessageHandler<TimingHandler>()
            .AddHttpMessageHandler<AuthTokenHandler>()
            .AddHttpMessageHandler<LogHandler>()
            .AddHttpMessageHandler<ValidateHeaderHandler>();
            */
            /*
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });
            */
            /*
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            */




            // ---- Typed Clients HttpClientFactory----//
            services.AddHttpClient<IFarmerExpertHttpClient, FarmerExpertHttpClient>();

            /*
            .ConfigurePrimaryHttpMessageHandler(messageHandler =>
            {
                var handler = new HttpClientHandler();
                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }
                return handler;
            })
            */

            /*
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            */

            /*
            http://enesaysan.com/software/polly-ve-net-core-3-0-retry-pattern/
            https://www.stevejgordon.co.uk/httpclientfactory-using-polly-for-transient-fault-handling
            https://github.com/aspnet/HttpClientFactory/blob/master/samples/HttpClientFactorySample/Program.cs
            https://erhanballieker.com/2019/03/15/asp-net-core-httpclientfactory-refit/
            https://erhanballieker.com/2019/03/27/asp-net-core-delegatinghandler/
            https://www.stevejgordon.co.uk/httpclientfactory-aspnetcore-outgoing-request-middleware-pipeline-delegatinghandlers
            https://www.stevejgordon.co.uk/httpclientfactory-using-polly-for-transient-fault-handling
            https://tr.coredump.biz/questions/50747749/how-to-use-httpclienthandler-with-httpclientfactory-in-net-core
            https://github.com/aspnet/HttpClientFactory/issues/71
            */

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = 4294967295;
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
