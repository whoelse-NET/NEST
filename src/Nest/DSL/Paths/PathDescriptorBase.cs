using System.Collections.Generic;

namespace Nest
{
  public abstract class PathDescriptorBase<TPathDescriptor, T>
    where TPathDescriptor : PathDescriptorBase<TPathDescriptor, T>
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
    
    internal IndexPath ToPath(IConnectionSettings settings)
    {
      return null;
    }
  }
}