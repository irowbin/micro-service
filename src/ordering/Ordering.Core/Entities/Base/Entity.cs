namespace Ordering.Core.Entities.Base
{
    public abstract class Entity : IEntityBase<int>
    {
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}