using System;
using System.Collections.Generic;
using System.Linq;
using Nest.Resolvers;

namespace Nest
{
  public abstract class TypesPathDescriptorBase<TPathDescriptor, T>
    : IndicesPathDescriptorBase<TPathDescriptor, T>
    where TPathDescriptor : TypesPathDescriptorBase<TPathDescriptor, T>
    where T : class
  {
    private bool _allTypes { get; set; }
    private IEnumerable<TypeNameMarker> _types { get; set; }

    public TPathDescriptor OnTypes(IEnumerable<string> types)
    {
      types.ThrowIfEmpty("types");
      this._types = types.Select(s => (TypeNameMarker)s); ;
      return (TPathDescriptor)this;
    }
    public TPathDescriptor OnTypes(params string[] types)
    {
      return this.OnTypes((IEnumerable<string>)types);
    }
    public TPathDescriptor OnTypes(IEnumerable<Type> types)
    {
      types.ThrowIfEmpty("types");
      this._types = types.Cast<TypeNameMarker>();
      return (TPathDescriptor)this;
    }
    public TPathDescriptor OnTypes(params Type[] types)
    {
      return this.OnTypes((IEnumerable<Type>)types);
    }
    public TPathDescriptor OnType(string type)
    {
      type.ThrowIfNullOrEmpty("type");
      this._types = new[] { (TypeNameMarker)type };
      return (TPathDescriptor)this;
    }
    public TPathDescriptor OnType(Type type)
    {
      type.ThrowIfNull("type");
      this._types = new[] { (TypeNameMarker)type };
      return (TPathDescriptor)this;
    }
    public TPathDescriptor OnAllTypes()
    {
      this._allTypes = true;
      return (TPathDescriptor)this;
    }

    internal TypePath ToPath(IConnectionSettings connectionSettings)
    {
      var indexPath = base.ToPath(connectionSettings);
      var typePath = new TypePath { Index = indexPath.Index };
      if (this._allTypes)
        return typePath;

      var infer = new ElasticInferrer(connectionSettings);
      string types = null;
      if (typeof(T) != typeof(object))
        types = infer.TypeName<T>();
			if (this._types.HasAny())
				types = this.JoinTypes(this._types, connectionSettings);
      else if (this._types != null) //if set to empty array assume all
				types = null;

      typePath.Type = types;

      return typePath;
    }
    private string JoinTypes(IEnumerable<TypeNameMarker> markers, IConnectionSettings connectionSettings)
    {
      if (!markers.HasAny())
        return null;
      return string.Join(",", markers.Select(t => t.Resolve(connectionSettings)));
    }
  }
}