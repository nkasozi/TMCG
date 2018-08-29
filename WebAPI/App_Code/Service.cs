using PegPayWebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using TcmpTestCore;

namespace PegPayWebAPI
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service : IService
    {

        public List<string> GetData(int value)
        {
            List<string> result = new List<string>();
            result.Add(string.Format("You entered: {0}", value));
            Result aresult = SharedLogic.TcmpTestCore.SetUpDatabase();
            return result;
        }
        

        public Result PayForTransaction(Payment payment)
        {
            return SharedLogic.TcmpTestCore.PayForTransaction(payment);
        }
    }
}