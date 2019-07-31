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
      var date = DateTime.ParseExact(YearMonth, "yyyyMM", null);
      return DateTime.DaysInMonth(date.Year, date.Month);
    }
  }
}