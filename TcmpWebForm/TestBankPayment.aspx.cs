using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TcmpTestCore;

namespace TcmpWebForm
{
    public partial class TestBankPayment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

                //if button click
                if (IsPostBack)
                {
                    return;
                }

                txtCustName.Enabled = false;
                txtCustPhone.Enabled = false;
                txtItemTotal.Enabled = false;
            }
            catch (Exception ex)
            {
                ShowErrorMsg(Globals.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        private void ShowErrorMsg(string msg)
        {
            //show error
            lblErrorMsg.Text = msg;
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            string Ref = txtTranID.Text;
            try
            {
                Sale sale = SharedLogic.TcmpTestCore.GetByID("SALE", Ref) as Sale;

                //failed to verify PRN at URA
                if (sale==null)
                {
                    lblErrorMsg.Text = $"ERROR: INVALID SALE ID PROVIDED";
                    return;
                }

                txtCustName.Enabled = true;
                txtCustPhone.Enabled = true;
                txtItemTotal.Text = sale.TotalCost.ToString();

                //success
                btnConfirm.Enabled = true;
                lblErrorMsg.Text = $"MSG: SALE ID VALIDATION SUCCESSFULL";
            }
            catch (Exception ex)
            {
                lblErrorMsg.Text = $"ERROR: " + ex.Message;
            }
        }

      
        private static bool RemoteCertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                Payment payment = new Payment
                {
                    Password = SharedCommons.GenearetHMACSha256Hash(Globals.API_SECRETKEY, Globals.API_PASSWORD),
                    PayerContact = txtCustPhone.Text,
                    PayerName = txtCustName.Text,
                    PaymentAmount = int.Parse(txtItemTotal.Text),
                    PaymentChannel = Globals.API_PAYMENT_TYPE,
                    PaymentId = SharedCommons.GenerateUniqueId("BANK-"),
                    PaymentNarration = $"Payment for Sale",
                    PaymentSystemCode = "TEST-BANK",
                    PaymentType = Globals.API_PAYMENT_TYPE,
                    SaleID = txtTranID.Text
                };

                //sign the request
                string dataToSign = payment.PaymentSystemCode + payment.PaymentAmount + payment.PaymentId + payment.PaymentChannel + payment.PayerContact + payment.PayerName + payment.SaleID;
                payment.DigitalSignature = SharedCommons.GenearetHMACSha256Hash(Globals.API_SECRETKEY, dataToSign);

                TcmpCore core = new TcmpCore();
                Result result = SendJsonRequest(payment);

                if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
                {
                    lblErrorMsg.Text = $"ERROR: " + result.StatusDesc;
                    return;
                }

                lblErrorMsg.Text = $"MSG: PAYMENT POSTED SUCCESSFULLY";
            }
            catch (Exception ex)
            {
                lblErrorMsg.Text = $"ERROR: " + ex.Message;
            }
        }

        private Result SendJsonRequest(Payment payment)
        {
            Result result = new Result();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidation;
                WebClient Proxy1 = new WebClient();
                Proxy1.Headers["Content-type"] = "application/json";
                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer serializerToUplaod = new DataContractJsonSerializer(typeof(Payment));
                serializerToUplaod.WriteObject(ms, payment);
                var uploadData = ms.ToArray();
                var data = Proxy1.UploadData(Globals.REST_API_URL, "POST", uploadData);
                var stream = new MemoryStream(data);
                var obj = new DataContractJsonSerializer(typeof(Result));
                result = obj.ReadObject(stream) as Result;
            }
            catch (Exception ex)
            {
                result.StatusCode = SharedCommonsGlobals.FAILURE_STATUS_CODE;
                result.StatusDesc = "ERROR:" + ex.Message;
            }
            return result;
        }
    }
}