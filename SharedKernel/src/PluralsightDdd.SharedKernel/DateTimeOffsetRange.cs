using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace PluralsightDdd.SharedKernel
{
  public class DateTimeOffsetRange : ValueObject
  {
    public DateTimeOffset Start { get; private set; }
    public DateTimeOffset End { get; private set; }

    public DateTimeOffsetRange(DateTimeOffset start, DateTimeOffset end)
    {
      // Ardalis.GuardClauses supports extensions with custom guards per project
      Guard.Against.OutOfRange(start, nameof(start), start, end);
      Start = start;
      End = end;
    }

    public DateTimeOffsetRange(DateTimeOffset start, TimeSpan duration) : this(start, start.Add(duration))
    {
    }

    public int DurationInMinutes()
    {
      return (int)Math.Round((End - Start).TotalMinutes, 0);
    }

    public DateTimeOffsetRange NewDuration(TimeSpan newDuration)
    {
      return new DateTimeOffsetRange(this.Start, newDuration);
    }

    public DateTimeOffsetRange NewEnd(DateTimeOffset newEnd)
    {
      return new DateTimeOffsetRange(this.Start, newEnd);
    }

    public DateTimeOffsetRange NewStart(DateTimeOffset newStart)
    {
      return new DateTimeOffsetRange(newStart, this.End);
    }

    public static DateTimeOffsetRange CreateOneDayRange(DateTimeOffset day)
    {
      return new DateTimeOffsetRange(day, day.AddDays(1));
    }

    public static DateTimeOffsetRange CreateOneWeekRange(DateTimeOffset startDay)
    {
      return new DateTimeOffsetRange(startDay, startDay.AddDays(7));
    }

    public bool Overlaps(DateTimeOffsetRange dateTimeRange)
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
