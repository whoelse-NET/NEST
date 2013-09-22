using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nest
{
  public class ClearCacheDescriptor
  {
    public Type Type { get; set; }
    public IEnumerable<string> Indices { get; set; }
    public ClearCacheOptions ClearCacheOptions { get; set; }
  }
}
