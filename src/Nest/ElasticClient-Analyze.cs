using System;
using System.Linq.Expressions;

namespace Nest
{
	public partial class ElasticClient
	{
		/// <summary>
		/// Performs the standard analysis process on a text and return the tokens breakdown of the text.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public IAnalyzeResponse Analyze(string text)
		{
			return this._Analyze(new AnalyzeParams() { Text = text });
		}
		/// <summary>
		/// Analyzes specified text according to the analyzeparams passed.
		/// </summary>
		/// <returns>AnalyzeResponse contains a breakdown of the token under .Tokens</returns>
		public IAnalyzeResponse Analyze(AnalyzeParams analyzeParams)
		{
			analyzeParams.ThrowIfNull("analyzeParams");
			return this._Analyze(analyzeParams);
		}
		/// <summary>
		/// Analyzes text according to the current analyzer of the field in the default index set in the clientsettings.
		/// </summary>
		/// <returns>AnalyzeResponse contains a breakdown of the token under .Tokens</returns>
		public IAnalyzeResponse Analyze<T>(Expression<Func<T, object>> selector, string text) where T : class
		{
			var index = this.Infer.IndexName<T>();
			return this.Analyze<T>(selector, index, text);
		}
		/// <summary>
		///  Analyzes text according to the current analyzer of the field in the passed index.
		/// </summary>
		public IAnalyzeResponse Analyze<T>(Expression<Func<T, object>> selector, string index, string text) where T : class
		{
			selector.ThrowIfNull("selector");
			var fieldName = this.PropertyNameResolver.Resolve(selector);
			var analyzeParams = new AnalyzeParams() { Index = index, Field = fieldName, Text = text };
			return this._Analyze(analyzeParams);
		}
		
		private AnalyzeResponse _Analyze(AnalyzeParams analyzeParams)
		{
      var path = this.Path.For(analyzeParams);
		  var status = path.Index.IsNullOrEmpty() 
        ? this.Raw.AnalyzeGet(path.QueryString) 
        : this.Raw.AnalyzeGet(path.Index, path.QueryString);
        
			var r = status.Deserialize<AnalyzeResponse>();
			return r;
		}
	}
}
