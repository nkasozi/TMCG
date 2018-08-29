using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TcmpWebForm
{
    public partial class Reciept : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //BussinessLogic bll = new BussinessLogic();
                string Status = Request.QueryString["Status"] as string;
                string VendorTranId = Request.QueryString["VendorId"] as string;
                string Reason = Request.QueryString["Reason"] as string;
                string DigitalSignature = Request.QueryString["DigitalSignature"] as string;
                string PrintOptions = Request.QueryString["PrintOptions"] as string;
                ViewState["VendorID"] = VendorTranId;



                //disable print optionss
                if (!string.IsNullOrEmpty(PrintOptions))
                {
                    btnReturn.Visible = false;
                    Button3.Visible = false;
                }

               

                //by this stage, status is SUCCESS
                //GenerateReciept(tr);
            }
            catch (Exception ex)
            {
                //string msg = "FAILED: " + ex.Message;
                //BussinessLogic.LogInterfaceError(msg, this.Page.GetType().ToString(), "");
                //BussinessLogic.ShowMessage(lblMsg, msg, true, Session);
                //Multiview1.SetActiveView(EmptyView);
                return;
            }
        }

        //private void GenerateReciept(TransactionReciept tr)
        //{
        //    string ExternalId = Request.QueryString["TranID"].ToString();

        //    string msg1 = "Transaction Status: SUCCESS";
        //    lblItemDesc.Text = "Payment Amount";
        //    lblItemTotal.Text = tr.ItemTotal + " UGX";
        //    lblItemChargeAmount.Text = tr.ChargeAmount + " UGX";
        //    lblTotalTranAmount.Text = tr.TotalTranAmount + " UGX";
        //    lblExternalId.Text = "#" + tr.PaymentChannelTranId;
        //    lblFlexipayId.Text = "#" + tr.FlexipayTranID;
        //    lblPaymentDate.Text = tr.PaymentDate;
        //    lblPaymentType.Text = tr.PaymentType;
        //    lblPRN.Text = tr.CustRef;
        //    lblMerchantName.Text = tr.MerchantName;
        //    lblCustName.Text = tr.CustomerName;
        //}

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
                string msg = "FAILED: " + ex.Message;
                //BussinessLogic.LogInterfaceError(msg, this.Page.GetType().ToString(), "");
                //BussinessLogic.ShowMessage(lblMsg, msg, true, Session);
                Multiview1.SetActiveView(EmptyView);
                return;
            }
        }
    }
}