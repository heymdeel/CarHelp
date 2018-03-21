using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.Entities
{
    [Table(Schema = "public", Name = "worker_supported_categories")]
    public partial class WorkerSupportedCategories
    {
        [Column(@"id_worker"), PrimaryKey(1), NotNull] public int IdWorker { get; set; } // integer
        [Column(@"id_category"), PrimaryKey(2), NotNull] public int IdCategory { get; set; } // integer
        [Column(@"price"), NotNull] public double Price { get; set; } // double precision

        #region Associations

        [Association(ThisKey = "IdCategory", OtherKey = "Id", CanBeNull = false, Relationship = Relationship.ManyToOne, KeyName = "worker_supported_categories_id_category_fkey", BackReferenceName = "workersupportedcategoriesidcategoryfkeys")]
        public OrderCategory OrderCategory { get; set; }

        [Association(ThisKey = "IdWorker", OtherKey = "Id", CanBeNull = false, Relationship = Relationship.ManyToOne, KeyName = "worker_supported_categories_id_worker_fkey", BackReferenceName = "workersupportedcategoriesidworkerfkeys")]
        public Worker Worker { get; set; }

        #endregion
    }
}
