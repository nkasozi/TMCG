using Castle.ActiveRecord;
using DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TcmpTestCore
{
    [ActiveRecord("Payments")]
    public class Payment : DbEntity<Payment>
    {
        [DataMember]
        [PrimaryKey(PrimaryKeyType.Identity, "RecordId")]
        public int Id { get; set; }

        [DataMember]
        [Property(Length = 50)]
        public string PaymentId { get; set; }

        [DataMember]
        [Property(Length = 50)]
        public string PaymentChannel { get; set; }

        [DataMember]
        [Property(Length = 50)]
        public string PaymentType { get; set; }

        [DataMember]
        [Property(Length = 50)]
        public string PayerName { get; set; }

        [DataMember]
        [Property(Length = 50)]
        public string PaymentNarration { get; set; }

        [DataMember]
        [Property(Length = 50)]
        public int PaymentAmount { get; set; }

        [DataMember]
        [Property(Length = 50)]
        public string PayerContact { get; set; }

        [DataMember]
        [Property(Length = 50)]
        public string PaymentSystemCode { get; set; }

        [DataMember]
        [Property(Length = 50)]
        public string SaleID { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        [Property(Length = 50)]
        public string DigitalSignature { get; set; }

        [DataMember]
        [Property]
        public DateTime PaymentDate { get; set; }

        public Payment()
        {
            PaymentDate = DateTime.Now;
        }

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

            Payment duplicatePayment = Payment.QueryWithStoredProc("GetPaymentByPaymentSystemCodeAndID", PaymentId, PaymentSystemCode).FirstOrDefault();

            if (duplicatePayment != null)
            {
                StatusCode = SharedCommonsGlobals.SUCCESS_STATUS_CODE;
                StatusDesc = SharedCommonsGlobals.SUCCESS_STATUS_TEXT;
                return false;
            }

            PaymentSystem system = PaymentSystem.QueryWithStoredProc("GetPaymentSystemByID", PaymentSystemCode).FirstOrDefault();

            if (system == null)
            {
                StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                StatusDesc = "INVALID PAYMENT SYSTEM CODE OR PASSWORD";
                return false;
            }

            string hashedPassword = SharedCommons.GenearetHMACSha256Hash(system.SecretKey, system.Password);

            if (hashedPassword != Password)
            {
                StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                StatusDesc = "INVALID PAYMENT SYSTEM CODE OR PASSWORD";
                return false;
            }

            string dataToSign = PaymentSystemCode + PaymentAmount + PaymentId + PaymentChannel + PayerContact + PayerName + SaleID;
            string hmacHash = SharedCommons.GenearetHMACSha256Hash(system.SecretKey, dataToSign);

            if (DigitalSignature != hmacHash)
            {
                StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                StatusDesc = "INVALID DIGITAL SIGNATURE";
                return false;
            }

            return base.IsValid();
        }
    }
}
