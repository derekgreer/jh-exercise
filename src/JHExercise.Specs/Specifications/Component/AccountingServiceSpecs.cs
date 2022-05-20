using System.Collections.Generic;
using ExpectedObjects;
using JHExercise.Domain.Records;
using JHExercise.Domain.Services;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace JHExercise.Specs.Specifications.Component;

public class AccountingServiceSpecs
{
    [Subject("Accounting Service")]
    public class when_retrieving_departments_with_expenses_meeting_or_exceeding_funding
    {
        static IEnumerable<Department> _actualDepartmentRecords;
        static ExpectedObject _expectedDepartments;
        static Mock<IAccountingServiceClient> _clientStub;

        Establish context = () =>
        {
            _clientStub = new Mock<IAccountingServiceClient>();
            _clientStub.Setup(x => x.GetDepartmentAccountingRecords()).ReturnsAsync(new []
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

        Because of = () => _actualDepartmentRecords = new AccountingService(_clientStub.Object).GetUnprofitableDepartments().Result;

        It should_only_return_the_expected_departments = () => _expectedDepartments.ShouldMatch(_actualDepartmentRecords);
    }
    
    [Subject("Accounting Service")]
    public class when_retrieving_departments_with_increasing_expenses_over_a_specified_timeframe
    {
        static IEnumerable<DepartmentExpense> _actualDepartments;
        static ExpectedObject _expectedDepartments;
        static Mock<IAccountingServiceClient> _clientStub;

        Establish context = () =>
        {
            _clientStub = new Mock<IAccountingServiceClient>();
            _clientStub.Setup(x => x.GetDepartmentAccountingRecords()).ReturnsAsync(new []
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

        Because of = () => _actualDepartments = new AccountingService(_clientStub.Object).GetDepartmentsExceedingExpenses(15, 2001, 5).Result;

        It should_only_return_the_expected_departments = () => _expectedDepartments.ShouldMatch(_actualDepartments);
    }
    
    [Subject("Accounting Service")]
    public class when_retrieving_departments_with_decreasing_expenses_year_over_year
    {
        static IEnumerable<DepartmentYearOverYearExpense> _actualDepartments;
        static ExpectedObject _expectedDepartments;
        static Mock<IAccountingServiceClient> _clientStub;

        Establish context = () =>
        {
            _clientStub = new Mock<IAccountingServiceClient>();
            _clientStub.Setup(x => x.GetDepartmentAccountingRecords()).ReturnsAsync(new []
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

        Because of = () => _actualDepartments = new AccountingService(_clientStub.Object).GetDepartmentsWithDecreasingExpenses(6).Result;

        It should_only_return_the_expected_departments = () => _expectedDepartments.ShouldMatch(_actualDepartments);
    }
}