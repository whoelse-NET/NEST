namespace Nest
{
  public abstract class DocumentPathDescriptorBase<TPathDescriptor, T>
    : TypePathDescriptorBase<TPathDescriptor, T>
    where TPathDescriptor : DocumentPathDescriptorBase<TPathDescriptor, T>
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