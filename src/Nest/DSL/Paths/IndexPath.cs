using System.Collections.Specialized;

namespace Nest
{
  public class IndexPath
  {
    public NameValueCollection QueryString { get; set; }
    public string Index { get; set; }
  }
}