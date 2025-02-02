using System;
using System.Text.Json.Serialization;
using CodeChallenge.Data;
using CodeChallenge.Helpers;
using CodeChallenge.Repositories;
using CodeChallenge.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeChallenge.Config
{
    public class App
    {
        public WebApplication Configure(string[] args)
        {
            args ??= Array.Empty<string>();

            var builder = WebApplication.CreateBuilder(args);

            builder.UseEmployeeDB();
            
            AddServices(builder.Services);

            var app = builder.Build();

            var env = builder.Environment;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedEmployeeDB();
            }

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        private void AddServices(IServiceCollection services)
        {

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRespository>();
            services.AddScoped<ICompensationService, CompensationService>();
            services.AddScoped<ICompensationRepository, CompensationRepository>();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // This is needed to allow .NET 6 to serialize/deserialize DateOnly properties.
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                })
                .AddJsonOptions(options =>
                {
                    // This is needed to prevent EF from trying to follow the object references between
                    // Employee and Compensation indefinitely.
                    // Source: https://stackoverflow.com/questions/59199593/net-core-3-0-possible-object-cycle-was-detected-which-is-not-supported
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
        }

        private void SeedEmployeeDB()
        {
            new EmployeeDataSeeder(
                new EmployeeContext(
                    new DbContextOptionsBuilder<EmployeeContext>().UseInMemoryDatabase("EmployeeDB").Options
            )).Seed().Wait();
        }
    }
}
