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

    public int GetOverlappingDays(Period budgetPeriod)
    {
      if (EndDate < budgetPeriod.StartDate || StartDate > budgetPeriod.EndDate)
      {
        return 0;
      }

      var startDate = StartDate > budgetPeriod.StartDate ? StartDate : budgetPeriod.StartDate;
      var endDate = EndDate < budgetPeriod.EndDate ? EndDate : budgetPeriod.EndDate;

      return endDate.Day - startDate.Day + 1;
    }
  }
}