using Castle.ActiveRecord;
using DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmpTestCore
{
    [ActiveRecord("ErrorLogs")]
    public class Errorlog:DbEntity<Errorlog>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "RecordId")]
        public int Id { get; set; }

        [Property(Length = 6000)]
        public string Message { get; set; }

        [Property(Length = 50)]
        public string LocationID { get; set; }

        [Property(Length = 50)]
        public string UserID { get; set; }

        [Property(Length = 50)]
        public DateTime CreatedOn { get; set; }

        public Errorlog()
        {
            CreatedOn = DateTime.Now;
        }
    }
}
