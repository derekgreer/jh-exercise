using System.Collections.Generic;
using System.Linq;
using JHExercise.Domain.Services;
using JHExercise.Infrastructure.Services;
using Machine.Specifications;

namespace JHExercise.Specs.Specifications.Integration;

public class AccountingServiceClientSpecs
{
    [Subject("Accounting Service Client")]
    class when_retrieving_department_accounting_records_from_accounting_service_client
    {
        static IEnumerable<DepartmentAccountingRecord> _response;

        Establish context = () => { };

        Because of = () => _response = new AccountingServiceClient(new AccountingServiceOptions { AccountingServiceUrl = "https://mockbin.org/bin/20acd654-c45a-4cea-bf6c-ad320a3dc303"}).GetDepartmentAccountingRecords().Result;

        It should_return_the_expected_department_records = () => _response.Count().ShouldNotBeNull();
    }
}