using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TcmpTestCore;

namespace TcmpWebForm
{
    public partial class _Default : Page
    {
        //public List<Item> ItemsAvailableForSale = new List<Item>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

                //if button click
                if (IsPostBack)
                {
                    return;
                }

                if (IsItemAddRequest())
                {
                    UpdateShoppingCart();
                }

                SetMutliviewDefaultView();

                //get items for sale
                List<Item> ItemsAvailableForSale = LoadItems();

                //no items found for sale
                if (ItemsAvailableForSale.Count <= 0)
                {
                    multiViewContent.ActiveViewIndex = 1;
                    lblMessage.Text = "There Are NO Items found for Sale Today. Login And Add a few Items first";
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        private void UpdateShoppingCart()
        {
            string ItemID = Request.QueryString["ItemId"];
            string Op = Request.QueryString["Op"];
            if (Op.ToUpper() == "ADD")
            {
                AddItemToShoppingCart(ItemID);
            }
            else
            {
                RemoveItemFromShoppingCart(ItemID);
            }
        }

        private void RemoveItemFromShoppingCart(string ItemId)
        {
            try
            {
                //get items available for sale
                List<Item> ItemsAvailableForSale = LoadItems();

                //get items stored in session
                List<Item> shoppingCart = GetItemsAlreadyInShoppingCart();

                //look for the item with the id specified
                Item itemSelected = ItemsAvailableForSale.Where(i => i.ItemCode == ItemId).FirstOrDefault();

                //we cant find the item selected
                if (itemSelected == null)
                {
                    ShowErrorMsg("Unable to Remove Item Selected. Try again");
                    return;
                }


                //add item to shopping cart
                shoppingCart.Remove(itemSelected);

                //save cart in session
                Session["ItemsInShoppingCart"] = shoppingCart;

                //update count of Items available for sale (locally)..db update will be done at point of payment
                ItemsAvailableForSale.Where(i => i.ItemCode == ItemId).Select(S => { S.ItemCount = S.ItemCount + 1; return S; });

                lblItemCount.InnerText = $"{shoppingCart.Count}";

                lblInfoMsg.Text = $"Item [{itemSelected.ItemName}] Added To Your Cart";
            }
            catch (Exception ex)
            {
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        private bool IsItemAddRequest()
        {
            if (Request.QueryString["ItemId"] != null)
            {
                return true;
            }
            return false;
        }

        private void SetMutliviewDefaultView()
        {
            divInfo.Visible = true;
            lblInfoMsg.Text = "Below are the Items Listed for Sale. Simply click on the item you want to add it to the shopping cart";
            multiViewContent.SetActiveView(viewItems);
            LoadUserRoles();
            GenerateTransactionIDIfNotExists();
        }

        private string GenerateTransactionIDIfNotExists()
        {
            //retrieve sale ID
            string tranId = Session["SaleID"] as string;

            //if no sale ID is not found...create one
            tranId = tranId ?? SharedCommons.GenerateUniqueId("SALE-");

            return tranId;
        }

        public List<Item> LoadItems()
        {
            //get all the any items for sale from session 
            List<Item> ItemsAvailableForSale = Session["AllItems"] as List<Item>;

            //double check...if session is null, then this is the first request, we fetch the items from the database
            ItemsAvailableForSale = ItemsAvailableForSale ?? SharedLogic.TcmpTestCore.GetAllAvailableItems();

            //we save those items in memory
            Session["AllItems"] = ItemsAvailableForSale;

            return ItemsAvailableForSale;
        }

        private void LoadUserRoles()
        {
            object[] objects = SharedLogic.TcmpTestCore.GetAll("USERROLE");
            ddRoles.Items.Clear();
            foreach (var obj in objects)
            {
                UserRole role = obj as UserRole;
                ddRoles.Items.Add(new ListItem(role.RoleName, role.RoleCode));
            }
            return;
        }
        protected void AddItemToShoppingCart(string ItemId)
        {
            try
            {
                //get items available for sale
                List<Item> ItemsAvailableForSale = LoadItems();

                //get items stored in session
                List<Item> shoppingCart = GetItemsAlreadyInShoppingCart();

                //look for the item with the id specified
                Item itemSelected = ItemsAvailableForSale.Where(i => i.ItemCode == ItemId).FirstOrDefault();

                //we cant find the item selected
                if (itemSelected == null)
                {
                    ShowErrorMsg("Unable to add Item Selected. Pick another Item");
                    return;
                }

                if (itemSelected.ItemCount <= 0)
                {
                    ShowErrorMsg("Item Selected Is out of Stock");
                    return;
                }


                //add item to shopping cart
                shoppingCart.Add(itemSelected);

                //save cart in session
                Session["ItemsInShoppingCart"] = shoppingCart;

                //update count of Items available for sale (locally)..db update will be done at point of payment
                ItemsAvailableForSale.Where(i => i.ItemCode == ItemId).Select(S => { S.ItemCount = S.ItemCount - 1; return S; });

                lblItemCount.InnerText = $"{shoppingCart.Count}";

                lblInfoMsg.Text = $"Item [{itemSelected.ItemName}] Added To Your Cart";
            }
            catch (Exception ex)
            {
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        private void ShowErrorMsg(string msg)
        {
            //hide info lbl
            divInfo.Visible = false;

            //switch to error view
            multiViewContent.SetActiveView(viewNoItems);

            //show error
            lblMessage.Text = msg;
        }

        public List<Item> GetItemsAlreadyInShoppingCart()
        {
            //get items stored in session
            List<Item> shoppingCart = Session["ItemsInShoppingCart"] as List<Item>;

            //if no items found initialize
            shoppingCart = shoppingCart ?? new List<Item>();

            //done
            return shoppingCart;
        }

        protected void btnInitiateCheckout_Click(object sender, EventArgs e)
        {
            try
            {
                //get items stored in session
                List<Item> shoppingCart = GetItemsAlreadyInShoppingCart();

                //oops..nothing in shopping cart
                if (shoppingCart.Count <= 0)
                {
                    ShowErrorMsg("No Items Found in Shopping Cart. Please Select Some Items to Purchase");
                    return;
                }

                //its ok
                lblInfoMsg.Text = "Before we proceed, Please Supply the Details below";

                //change the view
                multiViewContent.SetActiveView(viewCustomerDetails);
            }
            catch (Exception ex)
            {
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        protected void btnCompleteCheckout_Click(object sender, EventArgs e)
        {
            try
            {
                //save the customer
                Customer customer = new Customer
                {
                    CustomerName = txtCustName.Text,
                    Email = txtEmail.Text,
                    Phone = txtCustPhone.Text
                };

                //set the customer ID to either the phone or email depending on whats supplied
                customer.CustomerID = customer.Phone ?? customer.Email;

                //register customer
                Result result = SharedLogic.TcmpTestCore.RegisterCustomer(customer);

                //error on initiating a new sale
                if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
                {
                    lblInfoMsg.Text = (result.StatusDesc);
                    return;
                }

                //get items stored in session
                List<Item> shoppingCart = GetItemsAlreadyInShoppingCart();

                //oops..nothing in shopping cart
                if (shoppingCart.Count <= 0)
                {
                    lblInfoMsg.Text = ("No Items Found in Shopping Cart");
                    return;
                }

                //create a new sale
                Sale sale = new Sale
                {
                    SaleID = GenerateTransactionIDIfNotExists(),
                    CustomerId = customer.CustomerID
                };

                //initiate a new sale
                result = SharedLogic.TcmpTestCore.RegisterSale(sale);

                //error on initiating a new sale
                if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
                {
                    lblInfoMsg.Text = (result.StatusDesc);
                    return;
                }

                List<SaleItem> saleItems = new List<SaleItem>();

                //go item by item in cart
                foreach (var item in shoppingCart)
                {
                    //attach each item to this sale
                    SaleItem saleItem = new SaleItem
                    {
                        ItemId = item.ItemCode,
                        SaleId = sale.SaleID
                    };
                    saleItems.Add(saleItem);
                }

                //attach sale item to new sale
                result = SharedLogic.TcmpTestCore.RegisterSaleItems(saleItems.ToArray());

                //error on initiating a new sale
                if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
                {
                    lblInfoMsg.Text = (result.StatusDesc);
                    return;
                }

                //success
                lblInfoMsg.Text = "Please Click ON Your Prefered Method of Payment";

                //make him select a payment method
                multiViewContent.SetActiveView(viewPaymentMethodChoice);
            }
            catch (Exception ex)
            {
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                String username = txtLoginUsername.Text;
                string Password = txtLoginPassword.Text;
                SystemUser user = SharedLogic.TcmpTestCore.Login(username, Password);

                if (user.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
                {
                    lblInfoMsg.Text = user.StatusDesc;
                    return;
                }

                //reset Session
                Session.Clear();
                Session["User"] = user;

                if (user.RoleCode.ToUpper().Equals("ADMIN") || user.RoleCode.ToUpper().Equals("SUPER_ADMIN"))
                {
                    Response.Redirect("ViewReports.aspx");
                    return;
                }

                if (user.RoleCode.ToUpper().Equals("STORE_KEEPER"))
                {
                    Response.Redirect("AddNewItems.aspx");
                    return;
                }

                lblInfoMsg.Text = "UNABLE TO DETERMINE YOUR ROLE IN SYSTEM";
            }
            catch (Exception ex)
            {
                //display error
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                multiViewContent.SetActiveView(viewItems);
            }
            catch (Exception ex)
            {
                //display error
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        protected void btnBankPayment_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblInfoMsg.Text="Deposit Funds on the Account with Details below"
                multiViewContent.SetActiveView(viewBankDepositInstructions);
            }
            catch (Exception ex)
            {
                //display error
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        protected void btnOnlinePayment_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                //display error
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        protected void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                multiViewContent.SetActiveView(viewItems);
            }
            catch (Exception ex)
            {
                //display error
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        protected void btnRegisterSystemUser_Click(object sender, EventArgs e)
        {
            try
            {
                //set up on new user here
                SystemUser user = new SystemUser
                {
                    FullName = txtUserFullName.Text,
                    Password = txtUserPassword.Text,
                    Username = txtUsername.Text,
                    RoleCode = ddRoles.SelectedValue,
                    ModifiedBy = txtUsername.Text

                };

                //password and confirmed password dont match
                if (user.Password != txtConfirmedPassword.Text)
                {
                    lblInfoMsg.Text = ("Passwords dont match. Please retype Password");
                    return;
                }

                //attach sale item to new user
                Result result = SharedLogic.TcmpTestCore.RegisterSystemUser(user);

                //error on initiating a new user
                if (result.StatusCode != SharedCommonsGlobals.SUCCESS_STATUS_CODE)
                {
                    lblInfoMsg.Text = (result.StatusDesc);
                    return;
                }

                lblInfoMsg.Text = ("User has Been Created Successfully");
                //btnInitiateLogin(null, null);
                return;
            }
            catch (Exception ex)
            {
                //display error
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        protected void btnInitiateLogin_Click(object sender, EventArgs e)
        {
            try
            {
                lblInfoMsg.Text = "Supply Login Details Below";
                multiViewContent.SetActiveView(viewLogin);
            }
            catch (Exception ex)
            {
                //display error
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }

        protected void btnInitiateRegister_Click(object sender, EventArgs e)
        {
            try
            {
                lblInfoMsg.Text = "Supply User Details Below";
                multiViewContent.SetActiveView(viewRegisterUser);
            }
            catch (Exception ex)
            {
                //display error
                ShowErrorMsg(SharedLogic.INTERNAL_ERROR_MSG);

                //log error
                SharedLogic.TcmpTestCore.LogError($"EXCEPTION:{ex.Message}", $"{this.GetType().Name}-{SharedLogic.GetCurrentMethod()}", "N/A");
            }
        }
    }
}