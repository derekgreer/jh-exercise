using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using Autofac;
using JHExercise.Domain.Services;
using JHExercise.Specs.Domain;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace JHExercise.Specs.Infrastructure.Clients;

class JHExampleServiceApiClient : WebServiceClientBase
{
    readonly HttpClient _client;
    IEnumerable<Claim> _claims;

    public JHExampleServiceApiClient()
    {
        System.Environment.SetEnvironmentVariable("CACHING_DISABLED", "true");
        _client = new TestWebApplicationFactory()
            .WithWebHostBuilder(cfg =>
            {
                cfg.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IAccountingServiceClient>(FakeAccountingServiceClient.Instance);
                });
            })
            .CreateClient();
    }

    public JHExampleServiceApiClient WithClaims(IEnumerable<Claim> claims)
    {
        _claims = claims;
        return this;
    }

    protected override HttpClient GetClient()
    {
        return _client;
    }

    protected override string GetBearerTokenHeader()
    {
        return MockJwtTokens.GenerateJwtToken(_claims);
    }
}