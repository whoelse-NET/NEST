using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nest
{
  public class CountQueryDescriptor<T> : TypesPathDescriptorBase<CountQueryDescriptor<T>, T>
     where T : class
  {
    internal BaseQuery _Query { get; set; }

    public CountQueryDescriptor<T> Query(Func<QueryDescriptor<T>, BaseQuery> querySelector)
    {
      querySelector.ThrowIfNull("querySelector");
      this._Query = querySelector(new QueryDescriptor<T>());
      return this;
    }
  }
}
