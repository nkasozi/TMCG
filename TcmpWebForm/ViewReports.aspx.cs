using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TcmpTestCore;
using TcmpWebForm.AppCode;

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
                    Response.Redirect($"{SharedLogic.LOGIN_PAGE}?Msg={SharedLogic.RELOGIN_NEEDED_MSG}");
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
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }

        }

        public Chart LoadItemsStockReport()
        {
            Item[] items = SharedLogic.TcmpTestCore.GetAll("ITEM") as Item[];
            Chart barChart = new Chart();
            foreach (var item in items)
            {
                barChart.XAxisValues.Add(item.ItemName);
                barChart.YAxisValues.Add(item.ItemCount.ToString());
                barChart.lblXAxis += $"\"{item.ItemName}\",";
            }
            barChart.lblXAxis = barChart.lblXAxis.Trim(',');
            barChart.lblYAxis = "\"Amount Left In Stock\"";
            lblInfoMsg.Text = "Select Graph You want to see generated";
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
                Response.Redirect($"{SharedLogic.LOGIN_PAGE}?Msg={SharedLogic.SUCCESSFULL_LOGOUT_MSG}");
            }
            catch (Exception ex)
            {
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }
    }
}