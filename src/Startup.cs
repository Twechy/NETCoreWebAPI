using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Db.DbContext;
using Db.DbContext.Db_Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using netcorewebapi.Ioc;
using Swashbuckle.AspNetCore.Swagger;

namespace netcorewebapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            IoCContainer.Configuration = configuration;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = IoCContainer.Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CarDb>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<CarDb>()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultTokenProviders();

            var validIssuer = IoCContainer.Configuration["Jwt:Issuer"];
            var validAudience = IoCContainer.Configuration["Jwt:Audience"];
            var issuerSigningKey = IoCContainer.Configuration["Jwt:SecretKey"];

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.Audience = "http://localhost:5001/";
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = validIssuer,
                        ValidAudience = validAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(issuerSigningKey)),
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.DescribeStringEnumsInCamelCase();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "HTTP API",
                    Version = "v1",
                    Description = "The Service HTTP API"
                });
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            IoCContainer.Context = (CarDb) serviceProvider.GetService(typeof(CarDb));

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "HTTP API V1"); });
        }
    }
}