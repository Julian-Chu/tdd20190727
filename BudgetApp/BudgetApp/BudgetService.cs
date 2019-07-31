using System;
using System.Linq;

namespace BudgetApp
{
  public class Period
  {
    public Period(DateTime startDate, DateTime endDate)
    {
      StartDate = startDate;
      EndDate = endDate;
    }

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public int GetValidDaysInMonth()
    {
      return this.EndDate.Day - this.StartDate.Day + 1;
    }
  }

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
        return budget.GetDailyBudgetAmount() * period.GetValidDaysInMonth();
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
        var currentMonth = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1);
        var endBefore = new DateTime(endDate.Year, endDate.Month, 1);
        while (currentMonth < endBefore)
        {
          var searchMonth = currentMonth.ToString("yyyyMM");
          if (budgets.Any(x => x.YearMonth == searchMonth))
          {
            var budget = budgets.FirstOrDefault(x => x.YearMonth == searchMonth);

            totalAmount += budget.Amount;
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