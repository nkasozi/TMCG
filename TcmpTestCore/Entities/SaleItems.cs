using Castle.ActiveRecord;
using DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmpTestCore
{
    [ActiveRecord("SaleItems")]
    public class SaleItem: DbEntity<SaleItem>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "RecordId")]
        public int Id { get; set; }

        [Property]
        public string SaleId { get; set; }

        [Property]
        public string ItemId { get; set; }
    }
}
