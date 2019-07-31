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

    private DateTime StartDate { get; set; }
    private DateTime EndDate { get; set; }

    public int GetValidDaysInMonth(Budget budget)
    {
      var datetime = DateTime.ParseExact(budget.YearMonth, "yyyyMM", null);

      if (StartDate > budget.LastDay() || EndDate < budget.FirstDay())
      {
        return 0;
      }
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