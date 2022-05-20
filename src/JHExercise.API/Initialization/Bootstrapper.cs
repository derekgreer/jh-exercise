using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ConventionalOptions.DependencyInjection;
using FluentValidation.AspNetCore;
using JHExercise.API.Initialization.Extensions;
using JHExercise.API.Initialization.Modules;
using JHExercise.API.Middleware;
using JHExercise.API.Models;
using JHExercise.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace JHExercise.API.Initialization;

public class Bootstrapper
{
    public void Initialize(string[] args)
    {
        var app = CreateWebApplication(args);
        ConfigurePipeline(app);
        app.Run();
    }

    static void ConfigurePipeline(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseHealthChecks("/");

        app.MapControllers();

        app.UseMiddleware<ErrorHandlerMiddleware>();
    }

    static WebApplication CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                fv.ImplicitlyValidateChildProperties = true;
            });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(o => o.DescribeAllParametersInCamelCase());

        builder.Services.AddOptions();
        builder.Services.RegisterOptionsFromAssemblies(builder.Configuration, Assembly.GetExecutingAssembly(), typeof(AccountingServiceOptions).Assembly);

        builder.Services.AddHealthChecks();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var applicationResponse = new ValidationFailureResponse(context.ModelState);
                return new BadRequestObjectResult(applicationResponse);
            };
        });

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        
        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterModule<ConventionRegistrationModule>();

            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CACHING_DISABLED")))
                containerBuilder.RegisterModule<CachingModule>();
        });

        builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

        var app = builder.Build();
        return app;
    }
}