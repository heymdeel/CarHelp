using LinqToDB.Mapping;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.Entities
{
    [Table(Schema = "public", Name = "orders_responded_workers")]
    public class RespondedWorkers
    {
        [Column(@"order_id"), PrimaryKey(1), NotNull] public int OrderId { get; set; } // integer
        [Column(@"worker_id"), PrimaryKey(2), NotNull] public int WorkerId { get; set; } // integer
        [Column(@"commentary"), Nullable] public string Commentary { get; set; } // character varying(200)
        [Column(@"price"), Nullable] public double? Price { get; set; } // double precision
        [Column(@"location"), NotNull] public Point Location { get; set; } // Point

        #region Associations

        [Association(ThisKey = "OrderId", OtherKey = "Id", CanBeNull = false, Relationship = Relationship.ManyToOne, KeyName = "orders_workers_order_id_fkey", BackReferenceName = "workersorderidfkeys")]
        public Order Order { get; set; }

        [Association(ThisKey = "WorkerId", OtherKey = "Id", CanBeNull = false, Relationship = Relationship.ManyToOne, KeyName = "orders_workers_worker_id_fkey", BackReferenceName = "ordersworkeridfkeys")]
        public Worker Worker { get; set; }

        #endregion
    }
}
