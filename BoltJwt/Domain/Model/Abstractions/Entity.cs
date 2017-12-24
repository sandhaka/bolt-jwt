using System;
using System.Collections.Generic;
using MediatR;

namespace BoltJwt.Domain.Model.Abstractions
{
    public abstract class Entity
    {
        private int _id;

        private int? _requestedHashCode;

        private List<INotification> _domainEvents;

        public int Id
        {
            get => _id;
            protected set => _id = value;
        }

        public List<INotification> DomainEvents => _domainEvents;
        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            var item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (IsTransient())
            {
                return base.GetHashCode();
            }

            if (!_requestedHashCode.HasValue)
            {
                _requestedHashCode = this.Id.GetHashCode() ^ 31;
            }

            return _requestedHashCode.Value;
        }

        private bool IsTransient()
        {
            return this.Id == default(Int32);
        }
    }
}