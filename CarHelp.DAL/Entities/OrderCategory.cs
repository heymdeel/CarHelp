using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.Entities
{
    [Table(Schema = "public", Name = "orders_categorires")]
    public partial class OrderCategory
    {
        [Column(@"id"), PrimaryKey, Identity] public int Id { get; set; } // integer
        [Column(@"description"), NotNull] public string Description { get; set; } // character varying(25)

        #region Associations

        [Association(ThisKey = "Id", OtherKey = "Category", CanBeNull = true, Relationship = Relationship.OneToMany, IsBackReference = true)]
        public IEnumerable<Order> Orders { get; set; }

        [Association(ThisKey = "Id", OtherKey = "IdCategory", CanBeNull = true, Relationship = Relationship.OneToMany, IsBackReference = true)]
        public IEnumerable<WorkerSupportedCategories> WorkerSupportedCategories { get; set; }

        #endregion
    }
}
