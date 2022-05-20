using System.Collections.Generic;
using System.Threading.Tasks;
using JHExercise.Domain.Services;

namespace JHExercise.Specs.Domain;

public class FakeAccountingServiceClient : IAccountingServiceClient
{
    public IEnumerable<DepartmentAccountingRecord> Records { get; set; }
    public static FakeAccountingServiceClient Instance { get; } = new();

    public Task<IEnumerable<DepartmentAccountingRecord>> GetDepartmentAccountingRecords()
    {
        return Task.FromResult(Instance.Records);
    }

    public static void SetRecords(DepartmentAccountingRecord[] departmentAccountingRecords)
    {
        Instance.Records = departmentAccountingRecords;
    }
}