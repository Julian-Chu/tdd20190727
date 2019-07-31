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

      if (startDate > endDate)
      {
        return 0;
      }

      var period = new Period(startDate, endDate);
      var totalAmount = 0;
      var currentMonth = period.GetFirstCalendarDayOfStartMonth();
      var endMonth = period.GetLastCalendarDayOfEndMonth();
      while (currentMonth <= endMonth)
      {
        var budget = budgets.FirstOrDefault(x => x.YearMonth == currentMonth.ToString("yyyyMM"));
        if (budget != null)
        {
          totalAmount += budget.GetDailyBudgetAmount() * period.GetValidDaysInMonth(budget);
        }

        currentMonth = currentMonth.AddMonths(1);
      }

      return totalAmount;
    }
  }
}