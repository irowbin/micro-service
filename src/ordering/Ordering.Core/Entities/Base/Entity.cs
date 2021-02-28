namespace Ordering.Core.Entities.Base
{
    public abstract class Entity : IEntityBase<int>
    {
        public int Id { get; set; }
    }
}