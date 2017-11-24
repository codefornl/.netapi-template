namespace Api.Configuration
{
  public class General
  {

    public Database Database { get; set; }
    public int Version { get; set; }
    public string Name { get; set; }
  }

  public class Database
  {
    public string Host { get; set; }
    public int Port { get; set; }
    public string Protocol { get; set; }
    public string Name { get; set; }
  }

}