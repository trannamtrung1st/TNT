using System;
using System.Reflection;

namespace TNT.Layers.Domain.Entities
{
    public abstract class BaseEntity : DomainEntity
    {
    }

    public abstract class BaseEntity<TId> : BaseEntity
    {
        public abstract TId TransientIdValue();

        private int? _requestedHashCode;
        private TId _id;

        protected BaseEntity() : base()
        {
        }

        public virtual TId Id
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is AppAuditableEntity<TId>))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            AppAuditableEntity<TId> item = (AppAuditableEntity<TId>)obj;

            return item.Id?.Equals(Id) == true;
        }

        public override int GetHashCode()
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;
        }

        public static bool operator ==(BaseEntity<TId> left, BaseEntity<TId> right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(BaseEntity<TId> left, BaseEntity<TId> right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{this.GetGenericTypeName()}: {Id}";
        }
    }

    public abstract class AppAuditableEntity<TId> : BaseEntity<TId>, IAuditableEntity<TId>
    {
        protected AppAuditableEntity() : base()
        {
        }

        public TId CreatorId { get; set; }
        public TId LastModifyUserId { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastModifiedTime { get; set; }
    }

    public abstract class AppFullAuditableEntity<TId> : AppAuditableEntity<TId>, ISoftDeleteEntity<TId>
    {
        public TId DeletorId { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
