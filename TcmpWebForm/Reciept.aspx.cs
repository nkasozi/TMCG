using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TcmpTestCore;

namespace TcmpWebForm
{
    public partial class Reciept : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                GenerateReciept();
            }
            catch (Exception ex)
            {
                //display error
                ShowErrorMsg(Globals.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
                return;
            }
        }

        private void GenerateReciept()
        {
            //get details returned
            string status = Request.QueryString["Status"] as string;
            string vendorTranId = Request.QueryString["VendorId"] as string;
            string reason = Request.QueryString["Reason"] as string;
            string digitalSignature = Request.QueryString["DigitalSignature"] as string;
            string externalTranId = Request.QueryString["TranId"] as string;

            //is it success full
            if (status != SharedCommonsGlobals.SUCCESS_STATUS_TEXT)
            {
                lblMsg.Text = $"Processing of Transaction {vendorTranId} has Failed. Reason:{reason}";
                Multiview1.SetActiveView(ViewError);
                return;
            }

            //check digital signature
            string dataToSign = status + vendorTranId + reason;
            string hmacHashOfResponse = SharedCommons.GenearetHMACSha256Hash(Globals.GATEWAY_SECRET_KEY, dataToSign);

            //its invalid
            if (digitalSignature != hmacHashOfResponse)
            {
                lblMsg.Text = $"Processing of Transaction {vendorTranId} has Failed. Reason: INVALID DIGITAL SIGNATURE";
                Multiview1.SetActiveView(ViewError);
                return;
            }
            
            //Get original Sale Details
            Sale sale = SharedLogic.TcmpTestCore.GetByID("SALE", vendorTranId).FirstOrDefault() as Sale;

            //its invalid
            if (sale == null)
            {
                lblMsg.Text = $"Processing of Transaction {vendorTranId} has Failed. Reason: UNABLE TO RETRIEVE ORIGINAL SALE DETAILS FOR [{vendorTranId}]";
                Multiview1.SetActiveView(ViewError);
                return;
            }

            //Get customer detials
            Customer customer = SharedLogic.TcmpTestCore.GetByID("CUSTOMER", sale.CustomerId).FirstOrDefault() as Customer;

            //its invalid
            if (customer == null)
            {
                lblMsg.Text = $"Processing of Transaction {vendorTranId} has Failed. Reason: UNABLE TO RETRIEVE ORIGINAL CUSTOMER DETAILS FOR [{sale.CustomerId}]";
                Multiview1.SetActiveView(ViewError);
                return;
            }

            Payment payment = new Payment
            {
                Password = SharedCommons.GenearetHMACSha256Hash(Globals.API_SECRETKEY, Globals.API_PASSWORD),
                PayerContact = customer.Phone,
                PayerName = customer.CustomerName,
                PaymentAmount = sale.TotalCost,
                PaymentChannel = Globals.API_PAYMENT_TYPE,
                PaymentId = externalTranId,
                PaymentNarration = $"Payment for {sale.SaleID}",
                PaymentSystemCode = Globals.API_SYSTEMCODE,
                PaymentType = Globals.API_PAYMENT_TYPE,
                SaleID = sale.SaleID
            };

            //sign the request
            dataToSign = payment.PaymentSystemCode + payment.PaymentAmount + payment.PaymentId + payment.PaymentChannel + payment.PayerContact + payment.PayerName + payment.SaleID;
            payment.DigitalSignature = SharedCommons.GenearetHMACSha256Hash(Globals.API_SECRETKEY, dataToSign);

            //post payment 
            Result result = SharedLogic.TcmpTestCore.PayForTransaction(payment);

            if (result.StatusCode != Globals.SUCCESS_STATUS_CODE)
            {
                lblMsg.Text = $"Processing of Transaction {vendorTranId} has Failed. Reason: {result.StatusDesc}";
                Multiview1.SetActiveView(ViewError);
                return;
            }

            //populate reciept
            lblCustContact.Text = customer.Phone;
            lblCustName.Text = customer.CustomerName;
            lblExternalId.Text = externalTranId;
            lblItemDesc.Text = sale.SaleID;
            lblTotalTranAmount.Text = sale.TotalCost.ToString();
        }

        private void ShowErrorMsg(string msg)
        {
            //switch to error view
            Multiview1.SetActiveView(ViewError);

            //show error
            lblMsg.Text = msg;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            GoBackHome();
        }

        private void GoBackHome()
        {
            try
            {
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                //display error
                ShowErrorMsg(Globals.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");

            }
        }
    }
}