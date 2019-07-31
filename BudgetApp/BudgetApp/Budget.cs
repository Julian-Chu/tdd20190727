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

    public DateTime LastDay()
    {
      var date = BudgetDateTime;
      return new DateTime(
        date.Year,
        date.Month,
        1
        ).AddMonths(1).AddDays(-1);
    }

    public DateTime FirstDay()
    {
      var date = BudgetDateTime;
      return new DateTime(
        date.Year,
        date.Month,
        1);
    }
  }
}