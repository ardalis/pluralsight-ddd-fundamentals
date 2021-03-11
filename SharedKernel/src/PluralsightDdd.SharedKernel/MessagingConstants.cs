namespace PluralsightDdd.SharedKernel
{
  // These can be overridden with configuration values but serve as defaults otherwise
  public static class MessagingConstants
  {
    public static class Credentials
    {
      public const string DEFAULT_USERNAME = "guest";
      public const string DEFAULT_PASSWORD = "guest";
    }

    public static class Exchanges
    {
      public const string FRONTDESK_CLINICMANAGEMENT_EXCHANGE = "frontdesk-clinicmanagement";
      public const string FRONTDESK_VETCLINICPUBLIC_EXCHANGE = "frontdesk-vetclinicpublic";
    }

    public static class NetworkConfig
    {
      public const int DEFAULT_PORT = 5672;
      public const string DEFAULT_VIRTUAL_HOST = "/";
    }

    public static class Queues
    {
      public const string FDCM_CLINICMANAGEMENT_IN = "fdcm-clinicmanagement-in";
      public const string FDCM_FRONTDESK_IN = "fdcm-frontdesk-in";

      public const string FDVCP_FRONTDESK_IN = "fdvcp-frontdesk-in";
      public const string FDVCP_VETCLINICPUBLIC_IN = "fdvcp-vetclinicpublic-in";
    }
  }
}
