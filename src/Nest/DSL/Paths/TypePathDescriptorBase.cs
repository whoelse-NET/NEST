using System;
using System.Collections.Generic;
using System.Linq;
using Nest.Resolvers;

namespace Nest
{
  public abstract class TypePathDescriptorBase<TPathDescriptor, T>
    : IndexPathDescriptorBase<TPathDescriptor, T>
    where TPathDescriptor : TypePathDescriptorBase<TPathDescriptor, T>
    where T : class
  {
    private TypeNameMarker _type { get; set; }

    public TPathDescriptor OnType(string type)
    {
      type.ThrowIfNullOrEmpty("type");
      this._type = (TypeNameMarker)type;
      return (TPathDescriptor)this;
    }
    public TPathDescriptor OnType(Type type)
    {
      type.ThrowIfNull("type");
      this._type = (TypeNameMarker)type;
      return (TPathDescriptor)this;
    }

    internal TypePath ToPath(IConnectionSettings connectionSettings)
    {
      var indexPath = base.ToPath(connectionSettings);
      var typePath = new TypePath { Index = indexPath.Index };

      var infer = new ElasticInferrer(connectionSettings);

      string type = null;
      if (this._type == null && typeof(T) != typeof(object))
        type = infer.TypeName<T>();
      else if (this._type != null) 
        type = this._type.Resolve(connectionSettings);

      typePath.Type = type;

      return typePath;
    }
  
  }
}