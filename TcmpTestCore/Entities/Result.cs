using Castle.ActiveRecord;
using DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmpTestCore
{
    [ActiveRecord("Responses")]
    public class Result : DbEntity<Result>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "RecordId")]
        public int Id { get; set; }

        [Property(Length = 100)]
        public string RequestId { get; set; }

        [Property(Length = 100)]
        public string ResponseId { get; set; }

        public override void Save()
        {
            Task.Factory.StartNew(() => { try { base.Save(); } catch (Exception) { } });
        }
    }
}
