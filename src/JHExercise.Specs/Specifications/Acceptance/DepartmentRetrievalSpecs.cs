using System.Collections.Generic;
using System.Net;
using ExpectedObjects;
using JHExercise.Application.Responses;
using JHExercise.Domain.Records;
using JHExercise.Domain.Services;
using JHExercise.Specs.Domain;
using JHExercise.Specs.Infrastructure.Clients;
using Machine.Specifications;

namespace JHExercise.Specs.Specifications.Acceptance;

public class DepartmentRetrievalSpecs
{
    [Subject("Department Retrieval")]
    class when_retrieving_unprofitable_departments
    {
        static WebServiceClientResponse<ApplicationResponse<IEnumerable<Department>>> _clientResponse;
        static ExpectedObject _expectedDepartments;

        Establish context = () =>
        {
            FakeAccountingServiceClient.SetRecords(new []
            {
                new DepartmentAccountingRecord(new[] { "2002", "1", "name1", "20", "10", null }),
                new DepartmentAccountingRecord(new[] { "2002", "2", "name2", "20", "42", null }),
                new DepartmentAccountingRecord(new[] { "2002", "3", "name3", "20", "20", null })
            });
            
            _expectedDepartments = new[]
            {
                new Department("2", "name2"),
                new Department("3", "name3")
            }.ToExpectedObject();
        };

        Because of = () => _clientResponse = new JHExampleServiceApiClient().GetRequest<ApplicationResponse<IEnumerable<Department>>>("departments/unprofitable").Result;

        It should_return_success_status = () => _clientResponse.HttpStatusCode.ShouldEqual(HttpStatusCode.OK);

        It should_return_the_expected_departments = () => _expectedDepartments.ShouldMatch(_clientResponse.Response?.Data);
    }
    
    [Subject("Department Retrieval")]
    class when_retrieving_departments_with_excessive_expenses
    {
        static WebServiceClientResponse<ApplicationResponse<IEnumerable<DepartmentExpense>>> _clientResponse;
        static ExpectedObject _expectedDepartments;

        Establish context = () =>
        {
            FakeAccountingServiceClient.SetRecords(new []
            {
                new DepartmentAccountingRecord(new[] { "2002", "1", "name1", "1000", "50", null }),
                new DepartmentAccountingRecord(new[] { "2003", "1", "name1", "1000", "100", null }),
                new DepartmentAccountingRecord(new[] { "1999", "2", "name2", "1000", "50", null }),
                new DepartmentAccountingRecord(new[] { "2000", "2", "name2", "1000", "100", null }),
                new DepartmentAccountingRecord(new[] { "1999", "3", "name3", "1000", "50", null }),
                new DepartmentAccountingRecord(new[] { "2000", "3", "name3", "1000", "50", null }),
                new DepartmentAccountingRecord(new[] { "2002", "4", "name4", "1000", "100", null }),
                new DepartmentAccountingRecord(new[] { "2003", "4", "name4", "1000", "100", null })
            });

            _expectedDepartments = new[]
            {
                new DepartmentExpense { FiscalYear = 2003, Department = new Department("1", "name1"), Revenue = 1000, Expenses = 100 },
                new DepartmentExpense { FiscalYear = 2003, Department = new Department("4", "name4"), Revenue = 1000, Expenses = 100 }
            }.ToExpectedObject();
        };

        Because of = () => _clientResponse = new JHExampleServiceApiClient().GetRequest<ApplicationResponse<IEnumerable<DepartmentExpense>>>("departments/excessive-expenses?PercentageThreshold=15&StartFiscalYear=2001&DurationInYears=5").Result;


        It should_return_success_status = () => _clientResponse.HttpStatusCode.ShouldEqual(HttpStatusCode.OK);

        It should_return_the_expected_departments = () => _expectedDepartments.ShouldMatch(_clientResponse.Response?.Data);
    }
    
     [Subject("Department Retrieval")]
    class when_retrieving_departments_with_decreasing_expenses_year_over_year
    {
        static WebServiceClientResponse<ApplicationResponse<IEnumerable<DepartmentYearOverYearExpense>>> _clientResponse;
        static ExpectedObject _expectedDepartments;

        Establish context = () =>
        {
            FakeAccountingServiceClient.SetRecords(new []
            {
                new DepartmentAccountingRecord(new[] { "2002", "1", "name1", "1000", "50", null }),
                new DepartmentAccountingRecord(new[] { "2003", "1", "name1", "1000", "100", null }),
                new DepartmentAccountingRecord(new[] { "2004", "1", "name1", "1000", "50", null }),
                new DepartmentAccountingRecord(new[] { "1999", "2", "name2", "1000", "50", null }),
                new DepartmentAccountingRecord(new[] { "2000", "2", "name2", "1000", "120", null }),
                new DepartmentAccountingRecord(new[] { "1999", "3", "name3", "1000", "50", null }),
                new DepartmentAccountingRecord(new[] { "2000", "3", "name3", "1000", "50", null }),
                new DepartmentAccountingRecord(new[] { "2002", "4", "name4", "1000", "100", null }),
                new DepartmentAccountingRecord(new[] { "2003", "4", "name4", "1000", "100", null })
            });

            _expectedDepartments = new[]
            {
                new DepartmentYearOverYearExpense
                {
                    Department = new Department("1", "name1"),
                    FiscalYear = 2003,
                    ExpenseChangeFromPreviousYear = 5
                },
                new DepartmentYearOverYearExpense
                {
                    Department = new Department("1", "name1"),
                    FiscalYear = 2004,
                    ExpenseChangeFromPreviousYear = -5
                },
                new DepartmentYearOverYearExpense
                {
                    Department = new Department("3", "name3"),
                    FiscalYear = 2000,
                    ExpenseChangeFromPreviousYear = 0
                },
                new DepartmentYearOverYearExpense
                {
                    Department = new Department("4", "name4"),
                    FiscalYear = 2003,
                    ExpenseChangeFromPreviousYear = 0
                }
            }.ToExpectedObject();
        };

        Because of = () => _clientResponse = new JHExampleServiceApiClient().GetRequest<ApplicationResponse<IEnumerable<DepartmentYearOverYearExpense>>>("departments/decreasing-expenses?PercentageThreshold=6").Result;


        It should_return_success_status = () => _clientResponse.HttpStatusCode.ShouldEqual(HttpStatusCode.OK);

        It should_return_the_expected_departments = () => _expectedDepartments.ShouldMatch(_clientResponse.Response?.Data);
    }
}