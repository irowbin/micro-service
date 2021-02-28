namespace Ordering.Core.Entities.Base
{
    public interface IEntityBase<T>
    {
        public T Id { get; set; }
    }
}