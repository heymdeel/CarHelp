using LinqToDB.Mapping;
using NetTopologySuite.Geometries;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.Entities
{
    [Table(Schema = "public", Name = "workers")]
    public class Worker
    {
        [Column(@"id"), PrimaryKey, NotNull] public int Id { get; set; } // integer
        [Column(@"location"), NotNull] public Geometry Location { get; set; } // character varying
        [Column(@"status"), NotNull] public int StatusId { get; set; } // integer

        #region Associations

        [Association(ThisKey = "StatusId", OtherKey = "Id", CanBeNull = false, Relationship = Relationship.ManyToOne, KeyName = "fk_worker_status", BackReferenceName = "fkworkerstatus")]
        public WorkerStatus Status { get; set; }

        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = false, Relationship = Relationship.OneToOne, KeyName = "workers_id_fkey", BackReferenceName = "workersidfkey")]
        public User User { get; set; }

        [Association(ThisKey = "Id", OtherKey = "Worker", CanBeNull = true, Relationship = Relationship.OneToMany, IsBackReference = true)]
        public IEnumerable<Order> Orders { get; set; }

        [Association(ThisKey = "Id", OtherKey = "WorkerId", CanBeNull = true, Relationship = Relationship.OneToMany, IsBackReference = true)]
        public IEnumerable<RespondedWorkers> RespondedOrders { get; set; }

        #endregion
    }
}
