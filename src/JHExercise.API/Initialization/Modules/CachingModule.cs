using Autofac;
using Autofac.Extras.DynamicProxy;
using JHExercise.API.Initialization.Interceptors;
using JHExercise.Domain.Services;
using JHExercise.Infrastructure.Services;

namespace JHExercise.API.Initialization.Modules;

public class CachingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AccountingServiceClient>()
            .As<IAccountingServiceClient>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(AccountServiceClientCachingInterceptor));

        builder.RegisterType<AccountServiceClientCachingInterceptor>().AsSelf().SingleInstance();
    }
}