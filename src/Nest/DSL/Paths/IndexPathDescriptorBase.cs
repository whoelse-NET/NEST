using System;
using System.Collections.Generic;

namespace Nest
{
  public abstract class IndexPathDescriptorBase<TPathDescriptor, T>
    where TPathDescriptor : IndexPathDescriptorBase<TPathDescriptor, T>
    where T : class
  {
    internal string _index { get; set; }
    internal Type _indexType { get; set; }
   
    public TPathDescriptor OnIndex(string index)
    {
      index.ThrowIfNullOrEmpty("index");
      this._index =  index;
      return (TPathDescriptor)this;
    }

    public TPathDescriptor OnIndex<K>() where K : class
    {
      return this.OnIndex(typeof(K));
    }

    public TPathDescriptor OnIndex(Type type)
    {
      type.ThrowIfNull("type");
      this._indexType = type;
      return (TPathDescriptor)this;
    }
   
    internal IndexPath ToPath(IConnectionSettings connectionSettings)
    {
      var indexPath = new IndexPath();

      var infer = new ElasticInferrer(connectionSettings);

      string index;
      if (!this._index.IsNullOrEmpty()) 
        index = this._index;
      else if (this._indexType != null)
        index = infer.IndexName(this._indexType);
      else
        index = infer.IndexName<T>();

      indexPath.Index = index;

      return indexPath;
    }
  }
}