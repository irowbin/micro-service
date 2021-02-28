namespace Ordering.Core.Entities.Base
{
    public abstract class EntityBase<T>: IEntityBase<T>
    {
        public virtual T Id { get; set; }

      private  int? _requestedHashCode;

        public bool IsTransient()
        {
            return Id.Equals(default(T));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EntityBase<T>)) return false;

            if (ReferenceEquals(this, obj.GetType())) return true;

            if (GetType() != obj.GetType()) return false;

            var item = (EntityBase<T>) obj;

            return item.IsTransient() || IsTransient();
        }

        public override int GetHashCode()
        {
            if (IsTransient()) return base.GetHashCode();
            
            if (!_requestedHashCode.HasValue)
            {
                _requestedHashCode = Id.GetHashCode() ^ 31;
                return _requestedHashCode.Value;
            }
            return  base.GetHashCode();
             
        }

        public static bool operator ==(EntityBase<T> left, EntityBase<T> right)
        {
            if (Equals(left, null)) return Equals(right, null) ? true : false;
            return left.Equals(right);
        }

        public static bool operator !=(EntityBase<T> left, EntityBase<T> right)
        {
            return !(left == right);
        }
    }
}