using System.Reflection;
using Autofac;
using JHExercise.API.Initialization.Extensions;
using JHExercise.Infrastructure.Services;
using Module = Autofac.Module;

namespace JHExercise.API.Initialization.Modules;

public class ConventionRegistrationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        Assembly.GetExecutingAssembly()
            .GetRelatedAssemblies()
            .ToList()
            .ForEach(a => builder
                .RegisterAssemblyTypes(a)
                .AsImplementedInterfaces()
                .AsSelf()
                .PreserveExistingDefaults());

        builder.RegisterType<AccountingServiceClient>().As<AccountingServiceClient>().InstancePerDependency();
    }
}