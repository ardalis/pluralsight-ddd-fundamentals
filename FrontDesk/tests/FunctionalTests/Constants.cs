using System.Text.Json;

namespace FunctionalTests
{
  public static class Constants
  {
    public static JsonSerializerOptions DefaultJsonOptions = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
  }
}
