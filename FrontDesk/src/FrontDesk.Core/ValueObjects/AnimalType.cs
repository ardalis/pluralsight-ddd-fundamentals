using System.Collections.Generic;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.ValueObjects
{
  public class AnimalType : ValueObject
  {
    public string Species { get; private set; }
    public string Breed { get; private set; }

    private AnimalType()
    {
      // EF
    }
    public AnimalType(string species, string breed)
    {
      Species = species;
      Breed = breed;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
      yield return Breed;
      yield return Species;
    }
  }
}
