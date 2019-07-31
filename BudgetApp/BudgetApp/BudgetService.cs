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

    public int GetValidDaysInMonth(string budgetYearMonth)
    {
      var datetime = DateTime.ParseExact(budgetYearMonth, "yyyyMM", null);

      if (StartDate.ToString("yyyyMM") == EndDate.ToString("yyyyMM"))
      {
        return this.EndDate.Day - this.StartDate.Day + 1;
      }

      return DateTime.DaysInMonth(datetime.Year, datetime.Month);
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

        return budget.GetDailyBudgetAmount() * period.GetValidDaysInMonth(budget.YearMonth);
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
            else if (budget.YearMonth == endDate.ToString("yyyyMM"))
            {
              totalAmount += budget.GetDailyBudgetAmount() * (endDate.Day);
            }
            else
            {
              //totalAmount += budget.Amount;
              totalAmount += budget.GetDailyBudgetAmount() * period.GetValidDaysInMonth(budget.YearMonth);
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