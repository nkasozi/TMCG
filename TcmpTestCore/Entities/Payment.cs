using Castle.ActiveRecord;
using DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmpTestCore
{
    [ActiveRecord("Payments")]
    public class Payment:DbEntity<Payment>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "RecordId")]
        public int Id { get; set; }
        
        [Property(Length = 50)]
        public string PaymentId { get; set; }

        [Property(Length = 50)]
        public string PaymentChannel { get; set; }

        [Property(Length = 50)]
        public string PaymentType { get; set; }

        [Property(Length = 50)]
        public string PaymerName { get; set; }

        [Property(Length = 50)]
        public string PaymentNarration { get; set; }

        [Property(Length = 50)]
        public int PaymentAmount { get; set; }

        [Property(Length = 50)]
        public string PayerContact { get; set; }

        [Property(Length = 50)]
        public string PaymentSystemCode { get; set; }

        [Property(Length = 50)]
        public string Password { get; set; }

        [Property(Length = 50)]
        public string DigitalSignature { get; set; }

        public override bool IsValid()
        {
            string propertiesThatCanBeNull = $"{nameof(Id)}|{nameof(PaymentNarration)}";
            string nullCheckResult = SharedCommons.CheckForNulls(this, propertiesThatCanBeNull);
            if (nullCheckResult != SharedCommonsGlobals.SUCCESS_STATUS_TEXT)
            {
                StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                StatusDesc = nullCheckResult;
                return false;
            }

            Payment duplicatePayment = FindAll().Where(i => i.PaymentId?.ToUpper() == this.PaymentId?.ToUpper()).FirstOrDefault();

            if (duplicatePayment != null)
            {
                StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
                return false;
            }
            return base.IsValid();
        }
    }
}
