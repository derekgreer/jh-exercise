using System;
using JHExercise.API;
using JHExercise.Specs.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace JHExercise.Specs.Infrastructure.Clients;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    readonly Action<IServiceCollection> _configure;

    public TestWebApplicationFactory()
    {
    }

    public TestWebApplicationFactory(Action<IServiceCollection> configure)
    {
        _configure = configure;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            _configure?.Invoke(services);
            
            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var config = new OpenIdConnectConfiguration
                {
                    Issuer = MockJwtTokens.Issuer
                };

                config.SigningKeys.Add(MockJwtTokens.SecurityKey);
                options.Audience = MockJwtTokens.Audience;
                options.Configuration = config;
            });
        });

        return base.CreateHost(builder);
    }
}