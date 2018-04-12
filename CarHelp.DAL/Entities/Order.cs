using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using NpgsqlTypes;

namespace CarHelp.DAL.Entities
{
    [Table(Schema = "public", Name = "orders")]
    public class Order
    {
        [Column(@"id"), PrimaryKey, Identity] public int Id { get; set; } // integer
        [Column(@"client"), NotNull] public int ClientId { get; set; } // integer
        [Column(@"worker"), NotNull] public int WorkerId { get; set; } // integer
        [Column(@"location"), NotNull] public PostgisGeometry Location { get; set; } // USER-DEFINED
        [Column(@"begining_time"), NotNull] public DateTime BeginingTime { get; set; } // timestamp (6) without time zone
        [Column(@"end_time"), Nullable] public DateTime? EndTime { get; set; } // timestamp (6) without time zone
        [Column(@"category"), NotNull] public int CategoryId { get; set; } // integer
        [Column(@"status"), NotNull] public int StatusId { get; set; } // integer
        [Column(@"summary"), NotNull] public double Price { get; set; } // double precision
        [Column(@"rate"), NotNull] public int Rate { get; set; } // integer

        #region Associations

        [Association(ThisKey = "CategoryId", OtherKey = "Id", CanBeNull = false, Relationship = Relationship.ManyToOne, KeyName = "orders_category_fkey", BackReferenceName = "orderscategoryfkeys")]
        public OrderCategory Category { get; set; }

        [Association(ThisKey = "ClientId", OtherKey = "Id", CanBeNull = false, Relationship = Relationship.ManyToOne, KeyName = "orders_client_fkey", BackReferenceName = "ordersclientfkeys")]
        public UserProfile Client { get; set; }

        [Association(ThisKey = "StatusId", OtherKey = "Id", CanBeNull = false, Relationship = Relationship.ManyToOne, KeyName = "orders_status_fkey", BackReferenceName = "ordersstatusfkeys")]
        public OrderStatus Status { get; set; }

        [Association(ThisKey = "WorkerId", OtherKey = "Id", CanBeNull = false, Relationship = Relationship.ManyToOne, KeyName = "orders_worker_fkey", BackReferenceName = "ordersworkerfkeys")]
        public Worker Worker { get; set; }

        #endregion
    }
}
