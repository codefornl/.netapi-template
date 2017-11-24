using Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

using System;
using System.IO;

namespace Api
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
      //services.AddAuthentication(IISDefaults.AuthenticationScheme);
      services.AddOptions();
      services.AddCors(
          options => options.AddPolicy("AllowAll",
          builder =>
          {
            builder
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowAnyOrigin()
                      .AllowCredentials();
            builder.WithExposedHeaders(new String[] { "Content-Range", "X-Content-Range" });
          })
      );
      services.AddRouting(options => options.LowercaseUrls = true);
      services.AddResponseCompression(options =>
      {
        // TODO: Find out if this HTTPS and GZIP exploit is still there: https://en.wikipedia.org/wiki/BREACH
      });
      services.Configure<General>(Configuration.GetSection("Configuration"));

      // Uncomment next section to force https
      // services.Configure<MvcOptions>(
      //     options => {
      //         options.Filters.Add(new RequireHttpsAttribute());
      //     }
      // );
      services.AddMvc().AddJsonOptions(options =>
      {
        options.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = false });
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.None;
      });

      //ConfigureDependencyInjection(services);
      //ConfigureAuthorizationServices(services);
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "Api", Version = "v1" });
        var basePath = PlatformServices.Default.Application.ApplicationBasePath;
        var xmlPath = Path.Combine(basePath, "api.xml");
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseResponseCompression();
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
      });
      app.UseCors("AllowAll");
      app.UseMvc();
    }
  }
}
