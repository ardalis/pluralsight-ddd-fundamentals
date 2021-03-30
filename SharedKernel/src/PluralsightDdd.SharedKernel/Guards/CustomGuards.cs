using System;
using System.Collections.Generic;

namespace Ardalis.GuardClauses
{
  public static class CustomGuards
  {
    public static DateTimeOffset OutOfRange(this IGuardClause guardClause, DateTimeOffset input, string parameterName,  DateTimeOffset rangeFrom, DateTimeOffset rangeTo)
    {
      return OutOfRange<DateTimeOffset>(guardClause, input, parameterName, rangeFrom, rangeTo);
    }

    // https://github.com/ardalis/GuardClauses/blob/master/src/GuardClauses/GuardClauseExtensions.cs#L255
    private static T OutOfRange<T>(this IGuardClause guardClause, T input, string parameterName, T rangeFrom, T rangeTo)
    {
      Comparer<T> comparer = Comparer<T>.Default;

      if (comparer.Compare(rangeFrom, rangeTo) > 0)
      {
        throw new ArgumentException($"{nameof(rangeFrom)} should be less or equal than {nameof(rangeTo)}");
      }

      if (comparer.Compare(input, rangeFrom) < 0 || comparer.Compare(input, rangeTo) > 0)
      {
        throw new ArgumentOutOfRangeException(parameterName, $"Input {parameterName} was out of range");
      }

      return input;
    }
  }
}
