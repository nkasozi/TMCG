using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TcmpTestCore;

namespace PegPayWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
    [ServiceContract]
    public interface IService
    {

        [OperationContract]
        [WebGet(UriTemplate = "/GetData?ID={value}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<string> GetData(int value);


        [OperationContract]
        [WebInvoke(UriTemplate = "/PayForTransaction", RequestFormat = WebMessageFormat.Json,ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        Result PayForTransaction(Payment payment);

        [OperationContract]
        [WebGet(UriTemplate = "/VerifySale?SaleId={SaleId}&SystemCode={PaymentSystemCode}&Password={Password}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Sale VerifySale(string SaleId, string PaymentSystemCode, string Password);

    }
}
