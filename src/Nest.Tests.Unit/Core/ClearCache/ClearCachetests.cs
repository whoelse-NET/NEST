using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Nest;
using Newtonsoft.Json.Converters;
using Nest.Resolvers.Converters;
using Nest.Tests.MockData.Domain;

namespace Nest.Tests.Unit.Core.Get
{
	[TestFixture]
	public class ClearCacheTests : BaseJsonTests
	{
		[Test]
		public void ClearAll()
		{
			var result = this._client.ClearCache();
			var status = result.ConnectionStatus;
			StringAssert.EndsWith("/_cache/clear", status.RequestUrl);
		}
		
     [Test]
		public void ClearCacheByType()
		{
			var result = this._client.ClearCache<ElasticSearchProject>(ClearCacheOptions.Filter | ClearCacheOptions.Bloom);
			var status = result.ConnectionStatus;
      StringAssert.EndsWith("/nest_test_data/_cache/clear?filter=true&bloom=true", status.RequestUrl);
		}
	
    [Test]
		public void ClearCacheByIndices()
		{
			var result = this._client.ClearCache(new [] { "a", "b"}, ClearCacheOptions.Filter | ClearCacheOptions.Bloom);
			var status = result.ConnectionStatus;
      StringAssert.EndsWith("/a,b/_cache/clear?filter=true&bloom=true", status.RequestUrl);
		}
	
	}
}
