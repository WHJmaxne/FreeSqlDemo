using FreesqlDemo.API.DbModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FreesqlDemo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Fsql = new FreeSql.FreeSqlBuilder()
               //.UseConnectionString(FreeSql.DataType.MySql, "Data Source=13.75.68.96; Port=8806;User ID=root;Password=whj1993945..; Initial Catalog=FirstFreesqlDB;Charset=utf8; SslMode=none;Min pool size=1")
               .UseConnectionString(FreeSql.DataType.PostgreSQL, "Host=13.75.68.96;Port=8432;Username=postgres;Password=whj1993945..; Database=FirstFreesqlDB;Pooling=true;Minimum Pool Size=1")

               .UseAutoSyncStructure(true)
               .UseMonitorCommand(cmd => { }, (cmd, log) => Trace.WriteLine(log))
               .Build();

            Fsql.Aop.CurdAfter += (s, e) =>
            {
                Console.WriteLine(e.Identifier + ": " + e.EntityType.FullName + " " + e.ElapsedMilliseconds + "ms, " + e.Sql);
                CurdAfterLog.Current.Value?.Sb.AppendLine($"{Thread.CurrentThread.ManagedThreadId}: {e.EntityType.FullName} {e.ElapsedMilliseconds}ms, {e.Sql}");
            };
        }
        public static IFreeSql Fsql { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreesqlDemo.API", Version = "v1" });
            });

            services.AddSingleton(Fsql);
            services.AddFreeDbContext<AppDbContext>(options => options.UseFreeSql(Fsql));
            services.AddScoped<CurdAfterLog>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreesqlDemo.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    public class CurdAfterLog : IDisposable
    {
        public static AsyncLocal<CurdAfterLog> Current = new AsyncLocal<CurdAfterLog>();
        public StringBuilder Sb { get; } = new StringBuilder();

        public CurdAfterLog()
        {
            Current.Value = this;
        }
        public void Dispose()
        {
            Sb.Clear();
            Current.Value = null;
        }
    }
}
