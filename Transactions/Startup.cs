using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Npgsql;
using Transactions.Database;
using Transactions.Database.Repositories;
using Transactions.Services;
using Newtonsoft.Json;

namespace Transactions
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transactions", Version = "v1" });
            });

            services.AddDbContext<TransactionsDbContext>(opts=>{
                opts.UseNpgsql(CreateConnectionString());
            });

            services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            services.AddScoped<ITransactionsService, TransactionsService>();

            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //cors
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transactions v1"));
            }

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private string CreateConnectionString(){
            var username = this.Configuration["Database:Username"];
            var pass = this.Configuration["Database:Password"];
            var host = this.Configuration["Database:Host"];
            var port = this.Configuration["Database:Port"];
            var dbName = this.Configuration["Database:Name"];

            var builder = new NpgsqlConnectionStringBuilder(){
                Username=username,
                Password=pass,
                Host=host,
                Port=int.Parse(port),
                Database=dbName,
                Pooling=true
            };

            return builder.ConnectionString;
        }
    }
}
