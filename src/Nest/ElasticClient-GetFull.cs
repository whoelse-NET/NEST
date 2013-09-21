using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Nest.Resolvers;
using Nest.Domain;

namespace Nest
{

	public partial class ElasticClient
	{
		/// <summary>
		/// Gets a document of T by id in the default index and the inferred typename for T
		/// </summary>
		/// <returns>an instance of T</returns>
		public IGetResponse<T> GetFull<T>(int id) where T : class
		{
			return this.GetFull<T>(id.ToString(CultureInfo.InvariantCulture));
		}
		/// <summary>
		/// Gets a document of T by id in the default index and the inferred typename for T
		/// </summary>
		/// <returns>an instance of T</returns>
		public IGetResponse<T> GetFull<T>(string id) where T : class
		{
			return this.GetFull<T>(a=>a.Id(id));
		}
		/// <summary>
		/// Gets a document of T by id in the specified index and the specified typename
		/// </summary>
		/// <returns>an instance of T</returns>
		public IGetResponse<T> GetFull<T>(string index, string type, string id) where T : class
		{
			return this.GetFull<T>(a => a.Index(index).Type(type).Id(id));
		}
		/// <summary>
		/// Gets a document of T by id in the specified index and the specified typename
		/// </summary>
		/// <returns>an instance of T</returns>
		public IGetResponse<T> GetFull<T>(string index, string type, int id) where T : class
		{
			return this.GetFull<T>(index, type, id.ToString(CultureInfo.InvariantCulture));
		}
		

		public IGetResponse<T> GetFull<T>(Action<GetDescriptor<T>> getSelector) where T : class
		{
			getSelector.ThrowIfNull("getSelector");
			var d = new GetDescriptor<T>();
			getSelector(d);

			return this._GetFull<T>(d);
		}

		private IGetResponse<T> _GetFull<T>(GetDescriptor<T> descriptor) where T : class
		{
      var path = this.Path.For(descriptor);

      var response = this.Raw.Get(path.Index, path.Type, path.Id, path.QueryString);
			var getResponse = this.Deserialize<GetResponse<T>>(response);

			if (response.Result != null)
			{
				var f = new FieldSelection<T>();
				var o = JObject.Parse(response.Result);
				var source = o["fields"];
				if (source != null)
				{
					var json = source.ToString();
					f.Document = getResponse.Source;
					f.FieldValues = this.Deserialize<Dictionary<string, object>>(json);

				}
				getResponse.Fields = f;
			}

			return getResponse;
		}

	
		
	}
}
