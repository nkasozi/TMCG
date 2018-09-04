using Castle.ActiveRecord;
using DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmpTestCore
{
    [ActiveRecord("Sales")]
    public class Sale:DbEntity<Sale>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "RecordId")]
        public int Id { get; set; }

        [Property(Length = 50, Unique = true)]
        public string SaleID { get; set; }

        [Property(Length = 50)]
        public string CustomerId { get; set; }

        [Property]
        public int Tax { get; set; }

        [Property]
        public int TotalCost { get; set; }

        public override bool IsValid()
        {
            string propertiesThatCanBeNull = $"{nameof(Id)}";
            string nullCheckResult = SharedCommons.CheckForNulls(this, propertiesThatCanBeNull);
            if (nullCheckResult != SharedCommonsGlobals.SUCCESS_STATUS_TEXT)
            {
                StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                StatusDesc = nullCheckResult;
                return false;
            }

            return base.IsValid();
        }

        public static object[] GetSales()
        {
            Sale[] sales = Sale.QueryWithStoredProc("GetAllSales").ToArray();
            foreach (var sale in sales)
            {
                SaleItem[] saleItems = SaleItem.QueryWithStoredProc("GetSaleItemsBySaleID", sale.Id).ToArray();
                foreach (var saleItem in saleItems)
                {
                    Item item = Item.QueryWithStoredProc("GetItemByID", saleItem.ItemId).FirstOrDefault();
                    sale.TotalCost += item != null ? item.ItemPrice : 0;
                }
            }
            return sales;
        }

        public static object[] GetSalesById(string ID)
        {
            Sale[] sales = Sale.QueryWithStoredProc("GetSaleByID", ID).ToArray();
            foreach (var sale in sales)
            {
                SaleItem[] saleItems = SaleItem.QueryWithStoredProc("GetSaleItemsBySaleID", sale.SaleID).ToArray();
                foreach (var saleItem in saleItems)
                {
                    Item item = Item.QueryWithStoredProc("GetItemByID", saleItem.ItemId).FirstOrDefault();
                    sale.TotalCost += item != null ? item.ItemPrice : 0;
                }
            }
            return sales;
        }
    }
}
