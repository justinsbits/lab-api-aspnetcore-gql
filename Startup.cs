
using CommanderData;
using CommanderGQL.GraphQL;
using CommanderGQL.GraphQL.Commands;
using CommanderGQL.GraphQL.Tools;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace CommanderGQL
{
    public class Startup
    {
        private readonly string MyAllowedSpecificOrigins = "_myAllowSpecificOrigins";
        private readonly IConfiguration Configuration;
        
        

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowedSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000").WithOrigins("http://localhost:3001").AllowAnyHeader(); // !!! local dev testing
                });
            });
            
            // !!! centralize appsettings.json
           // var sqlConStrBuilder = new SqlConnectionStringBuilder(
            //    Configuration.GetConnectionString("CommandConStr"));
            // !!! UserSecretsId in .csproj is copy paste of GUID from CommanderDBMigraitonMgr (i.e. reference from there) 
           // sqlConStrBuilder.Password = Configuration["DbPassword"]; 
           // string sqlConStr = sqlConStrBuilder.ConnectionString;

            string sqlConStr = Configuration.GetConnectionString("CommandConStr");
            services.AddPooledDbContextFactory<AppDbContext>(opt => opt.UseSqlServer(sqlConStr)); 
            services
                .AddGraphQLServer()
                .EnableRelaySupport()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddType<ToolType>()
                .AddType<CommandType>()
                .AddFiltering()
                .AddSorting()
                .AddErrorFilter<CommandNotFoundExceptionFilter>();
                //.AddProjections(); // along with [UseProjection] in Query.cs ensure schema/queries take into account child relationships
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Add Error Page
                app.UseExceptionHandler("/error");
            }

            app.UseRouting();

            app.UseCors(MyAllowedSpecificOrigins);

            // setup endpoint (e.g. access to http://localhost:5000/graphql/   ...note: 'graphql' is typical convention for endpoint)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL(); 
            });

            // setup UI for visualizing schema - http://localhost:5000/graphql-voyager
            app.UseGraphQLVoyager(new GraphQLVoyagerOptions()
            {
                GraphQLEndPoint = "/graphql",
                Path = "/graphql-voyager"
            });
        }
    }
}
