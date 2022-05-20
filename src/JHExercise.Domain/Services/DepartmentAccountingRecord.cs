using JHExercise.Domain.Records;

namespace JHExercise.Domain.Services;

public class DepartmentAccountingRecord
{
    public DepartmentAccountingRecord(string[] recordData)
    {
        FiscalYear = recordData[0];
        DepartmentId = recordData[1];
        DepartmentName = recordData[2];
        FundsAvailable = long.Parse(recordData[3]);
        FundsUsed = long.Parse(recordData[4]);
        Remarks = recordData[5];
    }

    public string FiscalYear { get; }
    public string DepartmentId { get;  }
    public string DepartmentName { get; }
    public double FundsAvailable { get;  }
    public double FundsUsed { get;  }
    public string Remarks { get; }
    
    public double NetRevenue => FundsAvailable - FundsUsed;
    public double ExpensePercentage => (FundsUsed / FundsAvailable) * 100;
    
    public Department Department => new Department(DepartmentId, DepartmentName);
}