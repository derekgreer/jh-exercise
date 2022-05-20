namespace JHExercise.Domain.Services;

public interface IAccountingServiceClient
{
    Task<IEnumerable<DepartmentAccountingRecord>> GetDepartmentAccountingRecords();
}