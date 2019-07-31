using System;

namespace BudgetApp
{
  public class Budget
  {
    public string YearMonth { get; set; }
    public int Amount { get; set; }

    public int GetDailyBudgetAmount()
    {
      return Amount / GetDaysInMonth();
    }

    private int GetDaysInMonth()
    {
      var date = BudgetDateTime;
      return DateTime.DaysInMonth(date.Year, date.Month);
    }

    private DateTime BudgetDateTime
    {
      get
      {
        var date = DateTime.ParseExact(YearMonth, "yyyyMM", null);
        return date;
      }
    }

    public Period Period
    {
      get
      {
        var firstDay = DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        var lastDay = DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null).AddMonths(1).AddDays(-1);

        return new Period(firstDay, lastDay);
      }
    }
  }
}