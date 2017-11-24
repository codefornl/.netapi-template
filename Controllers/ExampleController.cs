
using Api.Configuration;
using Api.Models;
using Microsoft.Extensions.Options;

namespace Api.Controllers
{
  public class ExampleController : Base<Example>
  {
    public ExampleController(IOptions<General> config) : base(config)
    {
      _idcol = "InternalId";
    }
  }
}
