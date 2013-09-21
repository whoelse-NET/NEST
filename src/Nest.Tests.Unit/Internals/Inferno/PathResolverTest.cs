using NUnit.Framework;
using Nest.Tests.MockData.Domain;
using Nest.Resolvers;

namespace Nest.Tests.Unit.Internals.Inferno
{
	[TestFixture]
	public class PathResolverTests
	{
		private static readonly ConnectionSettings Settings = new ConnectionSettings(Test.Default.Uri)
			.SetDefaultIndex(Test.Default.DefaultIndex);
    
    private readonly IElasticClient _client = new ElasticClient(Settings, new InMemoryConnection(Settings));

		[Test]
		public void SimpleGetPath()
		{
			var status = this._client.GetFull<ElasticSearchProject>(d=>d
				.Id(1)
      );
			var expected = "/nest_test_data/elasticsearchprojects/1";
			var path = status.ConnectionStatus.RequestUrl;

			StringAssert.EndsWith(expected, path);
		}
		[Test]
		public void ComplexGetPath()
		{
			var status = this._client.GetFull<ElasticSearchProject>(d=>d
				.Index("newindex")
				.Type("myothertype")
				.Refresh()
				.Routing("routing")
				.ExecuteOnPrimary()
				.Id(1)
      );
			var expected = "/newindex/myothertype/1?refresh=true&preference=_primary&routing=routing";
      var path = status.ConnectionStatus.RequestUrl;

			StringAssert.EndsWith(expected, path, path);
		}
	}
}
