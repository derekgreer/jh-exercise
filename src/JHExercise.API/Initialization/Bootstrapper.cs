using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ConventionalOptions.DependencyInjection;
using FluentValidation.AspNetCore;
using JHExercise.API.Initialization.Extensions;
using JHExercise.API.Middleware;
using JHExercise.API.Models;
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
        builder.Services.AddSwaggerGen();

        builder.Services.AddOptions();
        builder.Services.RegisterOptionsFromAssemblies(builder.Configuration, Assembly.GetExecutingAssembly());

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
        builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            builder.RegisterModulesFor(Assembly.GetExecutingAssembly()));

        builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

        builder.Services.AddSwaggerGen(o => o.DescribeAllParametersInCamelCase());

        var app = builder.Build();
        return app;
    }
}