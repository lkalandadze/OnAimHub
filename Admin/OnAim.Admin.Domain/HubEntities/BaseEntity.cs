using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnAim.Admin.Domain.HubEntities
{
    // Generated Code

    public abstract class BaseEntity<T> : BaseEntity
    {
        private T _id;
        [BsonId]
        [Column(Order = 1)]
        public new T Id
        {
            get => _id;
            set
            {
                _id = value;
                base.Id = value;
            }
        }
    }

    public abstract class BaseEntity
    {
        public dynamic Id { get; set; } = null!;
    }
}
