using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nest
{
  public class FlushDescriptor<T> : IndicesPathDescriptorBase<FlushDescriptor<T>, T>
     where T : class
  {
    internal bool _Refresh { get; set; }

    public FlushDescriptor<T> Refresh(bool refresh = true)
    {
      this._Refresh = true;
      return this;
    }
  }
}
