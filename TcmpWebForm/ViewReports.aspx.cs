using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TcmpTestCore;

namespace TcmpWebForm
{
    public partial class ViewReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SetMutliviewDefaultView();

                //is he logged in
                if (!IsLoggedIn())
                {
                    Response.Redirect($"{Globals.LOGIN_PAGE}?Msg={Globals.RELOGIN_NEEDED_MSG}");
                }

                //if button click
                if (IsPostBack)
                {
                    return;
                }

                //set the user info
                SystemUser user = Session["User"] as SystemUser;
                btnUserDetails.Text = $"Name: {user.FullName}";
            }
            catch (Exception ex)
            {
                ShowErrorMsg(Globals.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }

        }

        public Chart LoadItemsStockReport()
        {
            lblInfoMsg.Text = "Select Graph You want to see generated";
            Chart barChart = SharedLogic.TcmpTestCore.GenerateChart("ITEM_STOCK_CHART");
            return barChart;
        }

        public Chart LoadPaymentsReport()
        {
            lblInfoMsg.Text = "Select Graph You want to see generated";
            Chart barChart = SharedLogic.TcmpTestCore.GenerateChart("PAYMENTS_PER_MONTH_CHART");
            return barChart;
        }

        private bool IsLoggedIn()
        {
            if (Session["User"] == null)
            {
                return false;
            }
            return true;
        }

        private void SetMutliviewDefaultView()
        {
            multiViewContent.SetActiveView(viewRegsisterItem);
        }

        private void ShowErrorMsg(string msg)
        {
            //show error
            lblInfoMsg.Text = msg;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Clear();
                Response.Redirect($"{Globals.LOGIN_PAGE}?Msg={Globals.SUCCESSFULL_LOGOUT_MSG}");
            }
            catch (Exception ex)
            {
                ShowErrorMsg(Globals.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }
    }
}