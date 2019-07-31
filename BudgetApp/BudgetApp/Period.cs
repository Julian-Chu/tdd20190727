using System;

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

    public int GetValidDaysInMonth(Budget budget)
    {
      var datetime = DateTime.ParseExact(budget.YearMonth, "yyyyMM", null);

      if (StartDate.ToString("yyyyMM") == EndDate.ToString("yyyyMM"))
      {
        return this.EndDate.Day - this.StartDate.Day + 1;
      }
      else if (datetime.ToString("yyyyMM") == EndDate.ToString("yyyyMM"))
      {
        return EndDate.Day;
      }
      else if (datetime.ToString("yyyyMM") == StartDate.ToString("yyyyMM"))
      {
        return DateTime.DaysInMonth(StartDate.Year, StartDate.Month) - StartDate.Day + 1;
      }

      return DateTime.DaysInMonth(datetime.Year, datetime.Month);
    }
  }
}