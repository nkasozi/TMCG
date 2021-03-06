﻿using Castle.ActiveRecord;
using DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmpTestCore
{
    [ActiveRecord("PaymentSystems")]
    public class PaymentSystem:DbEntity<PaymentSystem>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "RecordId")]
        public int Id { get; set; }

        [Property(Length = 50, Unique = true)]
        public string PaymentSystemCode { get; set; }

        [Property(Length = 50)]
        public string Password { get; set; }

        [Property(Length = 50)]
        public string SecretKey { get; set; }

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
    }
}
