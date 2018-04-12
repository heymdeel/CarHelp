using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.Entities
{
    [Table(Schema = "public", Name = "user_profiles")]
    public class UserProfile
    {
        [Column(@"id"), PrimaryKey, NotNull] public int Id { get; set; } // integer
        [Column(@"name"), NotNull] public string Name { get; set; } // character varying(15)
        [Column(@"surname"), NotNull] public string Surname { get; set; } // character varying(15)
        [Column(@"phone"), NotNull] public string Phone { get; set; } // character varying(11)
        [Column(@"car_number"), NotNull] public string CarNumber { get; set; } // character varying(15)

        #region Associations

        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = false, Relationship = Relationship.OneToOne, KeyName = "clients_id_fkey", BackReferenceName = "clientsidfkey")]
        public User User { get; set; }

        [Association(ThisKey = "Id", OtherKey = "Client", CanBeNull = true, Relationship = Relationship.OneToMany, IsBackReference = true)]
        public IEnumerable<Order> Orders { get; set; }

        #endregion
    }
}
