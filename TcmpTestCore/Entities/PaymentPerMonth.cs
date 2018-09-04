using DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmpTestCore
{
    public class PaymentPerMonth:DbEntity<PaymentPerMonth>
    {
        public int NumberOfPayments { get; set; }
        public string MonthAndYear { get; set; }
    }
}
