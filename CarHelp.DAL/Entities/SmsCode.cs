using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.Entities
{
    [Table(Schema = "public", Name = "sms_codes")]
    public partial class SmsCode
    {
        [Column(@"id"), PrimaryKey, Identity] public int Id { get; set; } // integer
        [Column(@"phone"), NotNull] public string Phone { get; set; } // character varying(11)
        [Column(@"code"), NotNull] public int Code { get; set; } // integer
        [Column(@"time_stamp"), NotNull] public DateTime TimeStamp { get; set; } // timestamp (6) without time zone
    }
}
