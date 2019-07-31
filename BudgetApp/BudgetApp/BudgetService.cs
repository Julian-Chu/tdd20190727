using System;
using System.Linq;

namespace BudgetApp
{
  public class BudgetService
  {
    private readonly IBudgetRepository _repo;

    public BudgetService(IBudgetRepository repo)
    {
      _repo = repo;
    }

    public decimal Query(DateTime startDate, DateTime endDate)
    {
      var budgets = this._repo.GetAll();

      //string searchMonth = "";
      if (startDate > endDate)
      {
        return 0;
      }

      if (IsSameMonth(startDate, endDate))
      {
        var searchMonth = startDate.ToString("yyyyMM");
        if (budgets.All(x => x.YearMonth != searchMonth))
        {
          return 0;
        }

        var budget = budgets.FirstOrDefault(x => x.YearMonth == searchMonth);
        return budget.GetDailyBudgetAmount() * (endDate.Day - startDate.Day + 1);
      }
      else
      {
        var firstMonthFullBudget = budgets.FirstOrDefault(x => x.YearMonth == startDate.ToString("yyyyMM"));
        int firstMonth = 0;
        if (firstMonthFullBudget != null)
        {
          firstMonth = firstMonthFullBudget.Amount /
                       DateTime.DaysInMonth(startDate.Year, startDate.Month) *
                       (DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day + 1);
        }

        var lastMonthFullBudget = budgets.FirstOrDefault(x => x.YearMonth == endDate.ToString("yyyyMM"));
        int lastMonth = 0;
        if (lastMonthFullBudget != null)
        {
          lastMonth = lastMonthFullBudget.Amount /
                      DateTime.DaysInMonth(endDate.Year, endDate.Month) * (endDate.Day);
        }
        var totalAmount = firstMonth + lastMonth;
        var allStartMonth = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1);
        var allEndMonth = new DateTime(endDate.Year, endDate.Month, 1);
        while (allEndMonth > allStartMonth)
        {
          var searchMonth = allStartMonth.ToString("yyyyMM");
          if (budgets.Any(x => x.YearMonth == searchMonth))
            totalAmount += budgets.FirstOrDefault(x => x.YearMonth == searchMonth).Amount;

          allStartMonth = allStartMonth.AddMonths(1);
        }

        return totalAmount;
      }
    }

    private bool IsSameMonth(DateTime startDate, DateTime endDate)
    {
      return startDate.Year == endDate.Year && startDate.Month == endDate.Month;
    }
  }
}