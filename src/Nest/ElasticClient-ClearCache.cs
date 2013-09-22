using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Nest
{
	public partial class ElasticClient
	{
		/// <summary>
		/// Clears all caches of all indices
		/// </summary>
		public IIndicesResponse ClearCache()
		{
			return this.ClearCache(null, ClearCacheOptions.All);
		}
		/// <summary>
		/// Clears the entire cache for the default index set in the client settings
		/// </summary>
		public IIndicesResponse ClearCache<T>() where T : class
		{
			return this.ClearCache(new List<string> { this.Infer.IndexName<T>() }, ClearCacheOptions.All);
		}

		/// <summary>
		/// Clears the specified caches for the default index set in the client settings 
		/// </summary>
		public IIndicesResponse ClearCache<T>(ClearCacheOptions options) where T : class
		{
			return this.ClearCache(new List<string> { this.Infer.IndexName<T>() }, options);
		}
		/// <summary>
		/// Clears the specified caches for all indices
		/// </summary>
		public IIndicesResponse ClearCache(ClearCacheOptions options)
		{
			return this.ClearCache(null, options);
		}
		/// <summary>
		/// Clears the specified caches for only the indices passed under indices
		/// </summary>
		public IIndicesResponse ClearCache(IEnumerable<string> indices, ClearCacheOptions options)
		{
      var clearCacheDescriptor = new ClearCacheDescriptor { ClearCacheOptions = options, Indices = indices };
      return this._ClearCache(clearCacheDescriptor);
		}

    private IIndicesResponse _ClearCache(ClearCacheDescriptor clearCacheDescriptor)
    {
      var path = this.Path.For(clearCacheDescriptor);
      ConnectionStatus status;
      if (path.Index.IsNullOrEmpty())
        status = this.Raw.ClearIndicesCacheGet(path.QueryString);
      else
        status = this.Raw.ClearIndicesCacheGet(path.Index, path.QueryString);

      return status.Deserialize<IndicesResponse>();
    }
	}
}
