using System.Collections.Generic;

namespace Nest
{
  public abstract class IndicesPathDescriptorBase<TPathDescriptor, T>
    where TPathDescriptor : IndicesPathDescriptorBase<TPathDescriptor, T>
    where T : class
  {
    internal bool _allIndices { get; set; }

    internal IEnumerable<string> _indices { get; set; }

    public TPathDescriptor OnIndices(IEnumerable<string> indices)
    {
      indices.ThrowIfEmpty("indices");
      this._indices = indices;
      return (TPathDescriptor)this;
    }
    public TPathDescriptor OnIndices(params string[] types)
    {
      return this.OnIndices((IEnumerable<string>)types);
    }
    public TPathDescriptor OnIndex(string index)
    {
      index.ThrowIfNullOrEmpty("indices");
      this._indices = new[] { index };
      return (TPathDescriptor)this;
    }

    public TPathDescriptor OnAllIndices()
    {
      this._allIndices = true;
      return (TPathDescriptor)this;
    }
    
    internal IndexPath ToPath(IConnectionSettings connectionSettings)
    {
      var indexPath = new IndexPath();
      if (this._allIndices)
        return indexPath;

      var infer = new ElasticInferrer(connectionSettings);

      string indices;
      if (this._indices.HasAny())
        indices = string.Join(",", this._indices);
      else if (this._indices != null) //if set to empty array asume all
        indices = null;
      else
        indices = infer.IndexName<T>();

      indexPath.Index = indices;

      return indexPath;
    }
  }
}