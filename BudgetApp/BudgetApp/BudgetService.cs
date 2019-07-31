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
      if (IsSameMonth(period))
      {
        var budget = budgets.FirstOrDefault(x => x.YearMonth == startDate.ToString("yyyyMM"));
        if (budget == null)
        {
          return 0;
        }

        return budget.GetDailyBudgetAmount() * period.GetValidDaysInMonth(budget);
      }
      else
      {
        var totalAmount = 0;
        var currentMonth = new DateTime(period.StartDate.Year, period.StartDate.Month, 1);
        var endBefore = new DateTime(period.EndDate.Year, period.EndDate.Month, 1).AddMonths(1);
        while (currentMonth < endBefore)
        {
          var searchMonth = currentMonth.ToString("yyyyMM");
          if (budgets.Any(x => x.YearMonth == searchMonth))
          {
            var budget = budgets.FirstOrDefault(x => x.YearMonth == searchMonth);
            if (budget == null)
            {
              continue;
            }

            if (budget.YearMonth == startDate.ToString("yyyyMM"))
            {
              totalAmount += budget.GetDailyBudgetAmount() * (DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day + 1);
            }
            else
            {
              totalAmount += budget.GetDailyBudgetAmount() * period.GetValidDaysInMonth(budget);
            }
          }

          currentMonth = currentMonth.AddMonths(1);
        }

        return totalAmount;
      }
    }

    private bool IsSameMonth(Period period)
    {
      return period.StartDate.Year == period.EndDate.Year && period.StartDate.Month == period.EndDate.Month;
    }
  }
}