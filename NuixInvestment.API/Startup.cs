using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace NuixInvestment.API
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
			services.AddSwaggerDocument(c =>
			{
				c.PostProcess = d =>
				{
					d.Info.Title = "Nuix Investment API";
					d.SecurityDefinitions.Add("ApiKey", new OpenApiSecurityScheme
					{
						Type = OpenApiSecuritySchemeType.ApiKey,
						Scheme = "ApiKey",
						In = OpenApiSecurityApiKeyLocation.Header,
						Name = "ApiKey"
					});
				};
				c.OperationProcessors.Add(new OperationSecurityScopeProcessor("ApiKey"));
			});
			//services.AddCors(options =>
			//{
			//	options.AddDefaultPolicy(builder =>
			//	{
			//		builder.WithOrigins("https://localhost:3000",
			//												"https://localhost:5001",
			//												"https://localhost:44338");
			//	});
			//});
			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseOpenApi();
			app.UseSwaggerUi3();
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();
			//app.UseCors();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}