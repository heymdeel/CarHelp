using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.Entities
{
    [Table(Schema = "public", Name = "users")]
    public partial class User
    {
        [Column(@"id"), PrimaryKey, Identity] public int Id { get; set; } // integer
        [Column(@"phone"), NotNull] public string Phone { get; set; } // character varying(11)
        [Column(@"roles"), NotNull] public string[] Roles { get; set; } // ARRAY
        [Column(@"date_of_registration"), NotNull] public DateTime DateOfRegistration { get; set; } // timestamp (6) without time zone
        [Column(@"refresh_token"), NotNull] public string RefreshToken { get; set; } // text

        #region Associations

        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = true, Relationship = Relationship.OneToOne, IsBackReference = true)]
        public UserProfile Profile { get; set; }

        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = true, Relationship = Relationship.OneToOne, IsBackReference = true)]
        public Worker Worker { get; set; }

        #endregion
    }
}
