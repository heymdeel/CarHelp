using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.Entities
{
    [Table(Schema = "public", Name = "orders_status")]
    public partial class OrderStatus
    {
        [Column(@"id"), PrimaryKey, Identity] public int Id { get; set; } // integer
        [Column(@"description"), NotNull] public string Description { get; set; } // character varying(20)

        #region Associations

        [Association(ThisKey = "Id", OtherKey = "Status", CanBeNull = true, Relationship = Relationship.OneToMany, IsBackReference = true)]
        public IEnumerable<Order> Orders { get; set; }

        #endregion
    }
}
