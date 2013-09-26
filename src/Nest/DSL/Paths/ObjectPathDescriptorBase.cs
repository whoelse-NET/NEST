namespace Nest
{
  public abstract class ObjectPathDescriptorBase<TPathDescriptor, T>
    : PathDescriptorBase<TPathDescriptor, T>
    where TPathDescriptor : ObjectPathDescriptorBase<TPathDescriptor, T>
    where T : class
  {

    public TPathDescriptor OnId(string id)
    {
      return (TPathDescriptor)this;
    }
    public TPathDescriptor OnId(int id)
    {
      return (TPathDescriptor)this;
    }
    public TPathDescriptor OnObject(T @object)
    {
      return (TPathDescriptor)this;
    }
  }
}