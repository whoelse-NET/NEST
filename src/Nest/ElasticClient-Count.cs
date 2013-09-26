using System;
using System.Collections.Generic;

namespace Nest
{
	public partial class ElasticClient
	{


    public ICountResponse Count<T>(Func<CountQueryDescriptor<T>, CountQueryDescriptor<T>> countQuerySelector) where T : class
    {
      countQuerySelector.ThrowIfNull("countQuerySelector");
      var descriptor = countQuerySelector(new CountQueryDescriptor<T>()); 
      
      var query = this.Serialize(descriptor._Query);
      var path = descriptor.ToPath(this._connectionSettings);

      ConnectionStatus status;
      if (path.Index.IsNullOrEmpty())
        status = this.Raw.CountPost(query, queryString: null);
      else if (path.Type.IsNullOrEmpty())
        status = this.Raw.CountPost(path.Index, query, queryString: null);
      else
        status = this.Raw.CountPost(path.Index, path.Type, query, queryString: null);

      return status.Deserialize<CountResponse>();

    }
	}
}
