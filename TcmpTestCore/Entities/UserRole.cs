using Castle.ActiveRecord;
using DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmpTestCore
{
    [ActiveRecord("UserRoles")]
    public class UserRole:DbEntity<UserRole>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "RecordId")]
        public int Id { get; set; }

        [Property(Length = 50, Unique = true)]
        public string RoleCode { get; set; }

        [Property(Length = 50)]
        public string RoleName { get; set; }
    }
}
