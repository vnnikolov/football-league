﻿using AutoMapper;
using FootballLeague.Entities;
using FootballLeague.Services.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SERVICES = FootballLeague.Services;

namespace FootballLeague
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

			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new Services.Impl.MappingProfile());
				mc.AddProfile(new Web.ViewModels.MappingProfile());
			});

			IMapper mapper = mappingConfig.CreateMapper();
			services.AddSingleton(mapper);

			services.AddScoped<SERVICES.ITeamService, TeamServiceImpl>();
			services.AddScoped<SERVICES.IMatchService, MatchServiceImpl>();

			string connectionString = Configuration.GetValue<string>("ConnectionString");
			services.AddDbContextPool<FootballLeagueDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
			});

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

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
