﻿using System;
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
      if (startDate > endDate)
      {
        return 0;
      }

      var period = new Period(startDate, endDate);
      return this._repo.GetAll().Sum(x => x.GetDailyBudgetAmount() * period.GetValidDaysInMonth(x));
    }
  }
}