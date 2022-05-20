using JHExercise.Domain.Services;
using Newtonsoft.Json;

namespace JHExercise.Infrastructure.Services;

public class AccountingServiceClient : IAccountingServiceClient
{
    readonly AccountingServiceOptions _options;

    public AccountingServiceClient(AccountingServiceOptions options)
    {
        _options = options;
    }
    public async Task<IEnumerable<DepartmentAccountingRecord>> GetDepartmentAccountingRecords()
    {
        var client = new HttpClient();
        var httpResponse = await client.GetAsync(_options.AccountingServiceUrl);
        var responseContent = await httpResponse.Content.ReadAsStringAsync();

        var response = JsonConvert.DeserializeObject<DepartmentAccountingServiceResponse>(responseContent);
        var records = response.Data.Select(r => new DepartmentAccountingRecord(r.Skip(9).ToArray()));

        return records;
    }

    public async Task<IEnumerable<DepartmentAccountingRecord>> GetDepartmentAccountingRecords2()
    {
        return await GetDepartmentAccountingRecords();
    }
}