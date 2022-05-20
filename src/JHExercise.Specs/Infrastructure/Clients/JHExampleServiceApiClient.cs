using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using JHExercise.Domain.Services;
using JHExercise.Specs.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace JHExercise.Specs.Infrastructure.Clients;

class JHExampleServiceApiClient : WebServiceClientBase
{
    readonly HttpClient _client;
    IEnumerable<Claim> _claims;

    public JHExampleServiceApiClient()
    {
        _client = new TestWebApplicationFactory(services =>
        {
            services.AddSingleton<IAccountingServiceClient>(FakeAccountingServiceClient.Instance);
        }).CreateClient();
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