using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace PluralsightDdd.SharedKernel
{
  public class DateTimeRange : ValueObject
  {
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    public DateTimeRange(DateTime start, DateTime end)
    {
      // Ardalis.GuardClauses supports extensions with custom guards per project
      Guard.Against.OutOfRange(start, nameof(start), start, end);
      Start = start;
      End = end;
    }

    public DateTimeRange(DateTime start, TimeSpan duration) : this(start, start.Add(duration))
    {
    }

    public int DurationInMinutes()
    {
      return (int)Math.Round((End - Start).TotalMinutes, 0);
    }

    public DateTimeRange NewDuration(TimeSpan newDuration)
    {
      return new DateTimeRange(this.Start, newDuration);
    }

    public DateTimeRange NewEnd(DateTime newEnd)
    {
      return new DateTimeRange(this.Start, newEnd);
    }

    public DateTimeRange NewStart(DateTime newStart)
    {
      return new DateTimeRange(newStart, this.End);
    }

    public static DateTimeRange CreateOneDayRange(DateTime day)
    {
      return new DateTimeRange(day, day.AddDays(1));
    }

    public static DateTimeRange CreateOneWeekRange(DateTime startDay)
    {
      return new DateTimeRange(startDay, startDay.AddDays(7));
    }

    public bool Overlaps(DateTimeRange dateTimeRange)
    {
      return this.Start < dateTimeRange.End &&
          this.End > dateTimeRange.Start;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
      yield return Start;
      yield return End;
    }
  }
}
