using Castle.ActiveRecord;
using DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmpTestCore
{
    [ActiveRecord("Customers")]
    public class Customer : DbEntity<Customer>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "RecordId")]
        public int Id { get; set; }

        [Property(Length = 50, Unique = true)]
        public string CustomerID { get; set; }

        [Property(Length = 50)]
        public string CustomerName { get; set; }

        [Property(Length = 50)]
        public string Phone { get; set; }

        [Property(Length = 50)]
        public string Email { get; set; }

        [Property(Length = 50)]
        public DateTime CreatedOn { get; set; }

        public Customer()
        {
            CreatedOn = DateTime.Now;
        }

        public override bool IsValid()
        {
            if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Phone))
            {
                StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                StatusDesc = "Please Supply an Email or Phone Number";
                return false;
            }

            string propertiesThatCanBeNull = $"{nameof(Id)}|{nameof(Email)}|{nameof(Phone)}";
            string nullCheckResult = SharedCommons.CheckForNulls(this, propertiesThatCanBeNull);
            if (nullCheckResult != SharedCommonsGlobals.SUCCESS_STATUS_TEXT)
            {
                StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                StatusDesc = nullCheckResult;
                return false;
            }

            return base.IsValid();
        }
    }
}
