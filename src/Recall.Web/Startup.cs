namespace Recall.Web
{
    using GetReady.Services.Mapping;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Recall.Data;
    using Recall.Services.Authentication;
    using Recall.Services.Navigation;
    using Recall.Services.Models.NavigationModels;
    using Recall.Services.Seeder;
    using System;
    using Recall.Services.Jwt;
    using Recall.Services.Directories;
    using Recall.Services.Videos;
    using System.IO;
    using Recall.Services.Meta.Topics;
    using Recall.Services.Meta.Connections;
    using Recall.Data.Models.Options;
    using Recall.Services.Options;
    using Recall.Services.Public;
    using Recall.Services.Admin;

    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.Configuration = configuration;
            this.CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        private IHostingEnvironment CurrentEnvironment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            AutoMapperConfig.RegisterMappings(typeof(DirectoryIndex).Assembly, typeof(UserOptions).Assembly);


            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services.AddTransient<INavigationService, NavigationService>();

            services.AddTransient<ISeederService, SeederService>();

            services.AddTransient<IJwtService, JwtService>();

            services.AddTransient<IDirectoryService, DirectoryService>();

            services.AddTransient<IVideoService, VideoService>();

            services.AddTransient<ITopicService, TopicService>();

            services.AddTransient<IConnectionService, ConnectionService>();

            services.AddTransient<IOptionsService, OptionsService>();

            services.AddTransient<IPublicService, PublicService>();

            services.AddTransient<IAdminService, AdminService>();


            if (this.CurrentEnvironment.IsProduction())
            {
                services.AddDbContext<RecallDbContext>
                        (o => o.UseNpgsql(Configuration.GetConnectionString("RecallHerokuPostgreSql")));
            }
            else if (CurrentEnvironment.IsDevelopment())
            {
                services.AddDbContext<RecallDbContext>
                    (o => o.UseNpgsql(Configuration.GetConnectionString("RecallPostgreSql")));
            }

            //services.AddDbContext<RecallDbContext>
            //            (o => o.UseNpgsql(Configuration.GetConnectionString("RecallHerokuPostgreSql")));

            var key = Convert.FromBase64String(Configuration["Jwt:SigningSecret"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //https://github.com/aspnet/AspNetCore/issues/6166
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200");
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
            });

            app.UseAuthentication();

            app.UseHttpsRedirection();

            ///Hosting The app here
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 &&
                   !Path.HasExtension(context.Request.Path.Value) &&
                   !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
            ///...

            app.UseMvc();
        }
    }
}
