using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
		public T Get<T>(int id) where T : class
		{
			return this.Get<T>(id.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Gets a document of T by id in the default index and the inferred typename for T
		/// </summary>
		/// <returns>an instance of T</returns>
		public T Get<T>(string id) where T : class
		{
			return this._Get<T>(new GetDescriptor<T>().Id(id));
		}

    /// <summary>
    /// Gets a document of T by id in the specified index and the specified typename
    /// </summary>
    /// <returns>an instance of T</returns>
    public T Get<T>(string index, string type, int id) where T : class
    {
      return this.Get<T>(index, type, id.ToString(CultureInfo.InvariantCulture));
    }

		/// <summary>
		/// Gets a document of T by id in the specified index and the specified typename
		/// </summary>
		/// <returns>an instance of T</returns>
		public T Get<T>(string index, string type, string id) where T : class
		{
      var d = new GetDescriptor<T>()
        .Id(id)
        .Type(type)
        .Index(index);
			return this._Get<T>(d);
		}
	
		public FieldSelection<T> GetFieldSelection<T>(Action<GetDescriptor<T>> getSelector) where T : class
		{
			getSelector.ThrowIfNull("getSelector");
			var d = new GetDescriptor<T>();
			getSelector(d);

      return this._GetFieldSelection<T>(d);
    }

		public T Get<T>(Action<GetDescriptor<T>> getSelector) where T : class
		{
			getSelector.ThrowIfNull("getSelector");
			var d = new GetDescriptor<T>();
			getSelector(d);

			d._Id.ThrowIfNullOrEmpty("Id on getselector");

			return this._Get<T>(d);
		}

		private T _Get<T>(GetDescriptor<T> descriptor) where T : class
		{
      var path = this.Path.For(descriptor);
      var response = this.Raw.Get(path.Index, path.Type, path.Id, path.QueryString);

			if (response.Result == null) //a 404 is hit when there is an attempt to grab a non existant document by id, this causes the 'result' to be null
				return null;

			var o = JObject.Parse(response.Result);
			var source = o["_source"];
			if (source != null)
			{
				return this.Deserialize<T>(source.ToString());
			}
			source = o["fields"];
			if (source != null)
			{
				return this.Deserialize<T>(source.ToString());
			}
			return null;
		}

		private FieldSelection<T> _GetFieldSelection<T>(GetDescriptor<T> descriptor) where T : class
		{
			var f = new FieldSelection<T>();
      var path = this.Path.For(descriptor);
      var response = this.Raw.Get(path.Index, path.Type, path.Id, path.QueryString);

			if (response.Result == null) //a 404 is hit when there is an attempt to grab a non existant document by id, this causes the 'result' to be null
				return null;

			var o = JObject.Parse(response.Result);
			var source = o["_source"];
			if (source != null)
			{
				f.Document =  this.Deserialize<T>(source.ToString());
			}
			source = o["fields"];
			if (source != null)
			{
				var json = source.ToString();
				f.Document = this.Deserialize<T>(json);
				f.FieldValues =  this.Deserialize<Dictionary<string, object>>(json);

			}
			return f;
		}
		
	}
}
