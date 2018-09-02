using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TcmpTestCore;

namespace TcmpWebForm
{
    public partial class AddNewItems : Page
    {
        public List<Item> ItemsAvailableForSale = new List<Item>();

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

                //get items for sale
                ItemsAvailableForSale = LoadItems();

                if (!IsItemEditRequest())
                {
                    return;
                }

                string ItemID = Request.QueryString["ItemId"];
                Item item = ItemsAvailableForSale.Where(i => i.ItemCode == ItemID).FirstOrDefault();

                if (item == null)
                {
                    ShowErrorMsg($"Unable to find Item with ID [{ItemID}] to Edit");
                    return;
                }

                txtItemCount.Text = item.ItemCount.ToString();
                txtItemName.Text = item.ItemName;
                txtPrice.Text = item.ItemPrice.ToString();

            }
            catch (Exception ex)
            {
                ShowErrorMsg(Globals.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }


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
            divInfo.Visible = true;
            lblInfoMsg.Text = "Supply Details below to create a new Item";
            multiViewContent.SetActiveView(viewRegsisterItem);
        }


        private bool IsItemEditRequest()
        {
            if (Request.QueryString["ItemId"] != null)
            {
                return true;
            }
            return false;
        }

        private List<Item> LoadItems()
        {

            //double check...if session is null, then this is the first request, we fetch the items from the database
            ItemsAvailableForSale = SharedLogic.TcmpTestCore.GetAllAvailableItems();

            return ItemsAvailableForSale;
        }

        

        private void ShowErrorMsg(string msg)
        {
          
            //show error
            lblInfoMsg.Text = msg;
        }



        protected void btnAddItems_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                ShowErrorMsg(Globals.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        private string GetBase64StringOfImageUploaded()
        {
            string base64String = "";

            if (!fuItemImage.HasFile)
            {
                return Globals.PLACEHOLDER_BASE64_STRING;
            }

            string fileExtension = Path.GetExtension(fuItemImage.PostedFile.FileName);
            string[] allowedExtensions = { ".png", ".jpg", ".jpeg", ".bitmap" };

            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new Exception($"Invalid File Format [{fileExtension}] Uploaded. Please Upload an Image");
            }

            Stream fs = fuItemImage.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            base64String = $"data:image/{fileExtension.Trim('.')};base64," + base64String;
            return base64String;
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

        protected void btnRegisterItem_Click(object sender, EventArgs e)
        {
            try
            {
                SystemUser user = Session["User"] as SystemUser;

                //create item
                Item item = new Item
                {
                    CreatedBy = user.Username,
                    ItemCode = Request.QueryString["ItemId"] == null ? SharedCommons.GenerateUniqueId("ITEM-") : Request.QueryString["ItemId"],
                    ItemCount = SharedCommons.GetIntFromStringDefaultsToZero(txtItemCount.Text),
                    ItemImage = GetBase64StringOfImageUploaded(),
                    ItemName = txtItemName.Text,
                    ItemPrice = SharedCommons.GetIntFromStringDefaultsToZero(txtPrice.Text),
                    ModifiedBy = user.Username,
                };

                Result result = SharedLogic.TcmpTestCore.RegisterItem(item);

                //failed to save
                if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
                {
                    lblInfoMsg.Text = result.StatusDesc;
                    return;
                }

                //success
                lblInfoMsg.Text = "Item Registered Successfully";

                //reload items from database
                ItemsAvailableForSale = LoadItems();
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