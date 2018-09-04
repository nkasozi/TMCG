<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="~/Default.aspx.cs" Inherits="TcmpWebForm._Default" %>

<%@ Import Namespace="TcmpTestCore" %>
<html>

<head>
    <title>TMCG Shopping Site</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Bangers" rel="stylesheet">
</head>

<body>
    <form runat="server">
        <div class="container">
            <div class="row">

                <div class="gallery col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center">
                    <h1 class="standOut-Heading">TMCG Shopping Site</h1>
                    <hr />
                </div>

                <div align="center">
                    <asp:Button runat="server" Text="Browse Items" ID="btnBrowse" CssClass="btn btn-primary padded-button" data-filter="all" OnClick="btnBrowse_Click" />
                    <asp:LinkButton runat="server" Text="Check Out & Pay " ID="btnCheckout" class="btn btn-success padded-button" data-filter="hdpe" OnClick="btnInitiateCheckout_Click"><span id="lblItemCount" runat="server" class="badge">0</span></asp:LinkButton>
                    <asp:Button runat="server" Text="Login/Sign In" ID="btnInitiateLogin" class="btn btn-info padded-button" data-filter="sprinkle" OnClick="btnInitiateLogin_Click" />
                    <asp:Button runat="server" Text="Register/Sign Up" ID="btnInitiateRegister" class="btn btn-warning padded-button" data-filter="spray" OnClick="btnInitiateRegister_Click" />
                </div>
                <hr />

                <div class="row">
                    <div id="divInfo" runat="server" class="alert alert-info text-center">
                        <asp:Label runat="server" ID="lblInfoMsg">
                                Below are the Items Listed for Sale. Simply click on the item you want to add it to the shopping cart 
                        </asp:Label>
                    </div>
                </div>

                <asp:MultiView ActiveViewIndex="0" runat="server" ID="multiViewContent">

                    <%------------------------ SHOP ITEMS VIEW -------------- --%>
                    <asp:View runat="server" ID="viewItems">


                        <%-- loop thru and display each item for sale--%>
                        <% 
                            List<Item> ItemsAvailableForSale = LoadItems();
                            foreach (var item in ItemsAvailableForSale)
                            {
                        %>

                        <div class="gallery_product col-lg-4 col-md-4 col-sm-4 col-xs-6 filter hdpe">
                            <a href="Default.aspx?ItemID=<% Response.Write(item.ItemCode); %>&Op=Add">
                                <img src="<% Response.Write(item.ItemImage); %>" style="height:300px;width:365px;" class="img-thumbnail img-rounded img-responsive" />
                            </a>
                            <br />
                            <label>Name: <% Response.Write(item.ItemName); %></label><br />
                            <label>Price: <% Response.Write(item.ItemPrice); %> UGX</label><br />
                            <label>Number Left: <% Response.Write(item.ItemCount); %></label>
                        </div>

                        <% } %>
                    </asp:View>

                    <%-- -------------------- ERROR VIEW ----------------- --%>
                    <asp:View runat="server" ID="viewNoItems">
                        <div class="row">
                            <div class="alert alert-danger text-center">
                                <asp:Label runat="server" ID="lblMessage">
                                There Are NO Items found for Sale Today. Login And Add a few Items first
                                </asp:Label>
                            </div>
                        </div>
                        <div class="row text-center">
                            <asp:Button ID="btnCancelError" runat="server" Text="Cancel" Width="200px" CssClass="btn btn-danger btn-md  padded-button" OnClick="btnCancel_Click" />
                        </div>
                    </asp:View>

                    <%-- ------------------  SUPPLY CUSTOMER DETAILS VIEW--------- --%>
                    <asp:View runat="server" ID="viewCustomerDetails">
                        <div class="row">
                            <div class="col-lg-3">
                            </div>
                            <div class="col-lg-6">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>
                                            Full Name <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                        </label>
                                        <asp:TextBox ID="txtCustName" runat="server" CssClass="form-control" placeholder="Enter text" />
                                        <p class="help-block">
                                        </p>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>
                                            Email <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                        </label>
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter text" />
                                        <p class="help-block">
                                        </p>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>
                                            Phone <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                        </label>
                                        <asp:TextBox ID="txtCustPhone" runat="server" CssClass="form-control" placeholder="Enter text" />
                                        <p class="help-block">
                                        </p>
                                    </div>
                                </div>


                            </div>
                            <div class="col-lg-3">
                            </div>
                        </div>

                        <div class="row text-center">
                            <h3>Purchase Details</h3>
                            <hr />
                        </div>

                        <div class="row">

                            <table class="table">
                                <thead>
                                    <tr>
                                        <th scope="col">Item Code</th>
                                        <th scope="col">Item Name</th>
                                        <th scope="col">Item Price</th>
                                        <th scope="col">#</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <%-- loop thru and display each item for sale--%>
                                    <% 
                                        List<Item> shoppingCart = GetItemsAlreadyInShoppingCart();
                                        int total = 0;
                                        foreach (var item in shoppingCart)
                                        {
                                            total += item.ItemPrice;
                                    %>
                                    <tr>
                                        <th scope="row"><%Response.Write(item.ItemCode); %></th>
                                        <td><%Response.Write(item.ItemName); %></td>
                                        <td><%Response.Write(item.ItemPrice); %></td>
                                        <td><a class="btn btn-default btn-md  padded-button" href="Default.aspx?ItemID=<% Response.Write(item.ItemCode); %>&Op=Remove">Remove </a></td>
                                    </tr>
                                    <% } %>
                                    <tr>
                                        <th scope="row">Total</th>
                                        <td></td>
                                        <td><%Response.Write(total); %></td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        <div class="row">
                            <div class="text-center">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="200px" CssClass="btn btn-danger btn-md  padded-button"
                                    OnClick="btnCancel_Click" />
                                <asp:Button ID="btnCompleteCheckOut" runat="server" Text="Pay Now" Width="200px" CssClass="btn btn-success btn-md padded-button"
                                    OnClick="btnCompleteCheckout_Click" />
                            </div>
                        </div>

                    </asp:View>

                    <%-- -------------------- PAYMET METHOD CHOICE VIEW ----------------- --%>
                    <asp:View runat="server" ID="viewPaymentMethodChoice">
                        <div class="row">
                            <div class="col-lg-3">
                            </div>
                            <div class="col-lg-3">

                                <asp:ImageButton runat="server" ID="btnBankPayment" Height="200" Width="365" OnClick="btnBankPayment_Click" ImageUrl="Images/bankIcon2.png" CssClass="img-thumbnail img-rounded img-responsive" />
                            </div>
                            <div class="col-lg-3">
                                <asp:ImageButton runat="server" ID="btnOnlinePayment" Height="200" Width="365" OnClick="btnOnlinePayment_Click" ImageUrl="Images/cardsIcon.png" CssClass="img-thumbnail img-rounded img-responsive" />
                            </div>
                            <div class="col-lg-3">
                            </div>
                        </div>

                    </asp:View>

                    <%-- -------------------- BANK DETAILS VIEW VIEW ----------------- --%>
                    <asp:View runat="server" ID="viewBankDepositInstructions">
                        <div class="row">
                            <div class="col-lg-3">
                            </div>
                            <div class="col-lg-6">
                                <div class="row">

                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th scope="col">Name</th>
                                                <th scope="col">Details</th>
                                            </tr>
                                        </thead>

                                        <tbody>
                                            <tr>
                                                <th scope="row">Bank Name</th>
                                                <td>Stanbic Bank Uganda</td>
                                            </tr>
                                            <tr>
                                                <th scope="row">Account Names</th>
                                                <td>TMCG Online Web Shop</td>
                                            </tr>
                                            <tr>
                                                <th scope="row">Account Number</th>
                                                <td>014055687758601</td>
                                            </tr>
                                            <tr>
                                                <th scope="row">Country</th>
                                                <td>Uganda</td>
                                            </tr>
                                            <tr>
                                                <th scope="row">Sale ID</th>
                                                <td><asp:Label ID="lblSaleID" runat="server"/></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="row text-center">
                                    <asp:Button runat="server" Text="Go Back" ID="btnGoBackToPaymentMethod" CssClass="btn btn-primary" OnClick="btnGoBackToPaymentMethod_Click" />
                                </div>
                            </div>
                            <div class="col-lg-3">
                            </div>
                        </div>

                    </asp:View>


                    <%-- ------------------  LOGIN VIEW--------- --%>
                    <asp:View runat="server" ID="viewLogin">
                        <div class="col-lg-3">
                        </div>
                        <div class="col-lg-6">


                            <div class="row">
                                <div class="col-lg-12">
                                    <label>
                                        Username <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                    </label>
                                    <asp:TextBox ID="txtLoginUsername" runat="server" CssClass="form-control" placeholder="Enter text" />
                                    <p class="help-block">
                                    </p>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-12">
                                    <label>
                                        Password <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                    </label>
                                    <asp:TextBox ID="txtLoginPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="Enter text" />
                                    <p class="help-block">
                                    </p>
                                </div>
                            </div>


                            <hr />
                            <div class="row">
                                <div class="text-center">
                                    <asp:Button ID="btnCancelLogin" runat="server" Text="Cancel" Width="200px" CssClass="btn btn-danger btn-md  padded-button"
                                        OnClick="btnCancel_Click" />
                                    <asp:Button ID="btnLogin" runat="server" Text="Login" Width="200px" CssClass="btn btn-success btn-md  padded-button"
                                        OnClick="btnLogin_Click" />
                                </div>
                            </div>
                            <hr />

                        </div>

                        <div class="col-lg-3">
                        </div>

                    </asp:View>

                    <%-- ------------------  REGISTER SYSTEM USER DETAILS VIEW--------- --%>
                    <asp:View runat="server" ID="viewRegisterUser">
                        <div class="col-lg-3">
                        </div>
                        <div class="col-lg-6">
                            <div class="row">
                                <div class="col-lg-12">
                                    <label>
                                        Full Name <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                    </label>
                                    <asp:TextBox ID="txtUserFullName" runat="server" CssClass="form-control" placeholder="Enter text" />
                                    <p class="help-block">
                                    </p>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-12">
                                    <label>
                                        Username <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                    </label>
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter text" />
                                    <p class="help-block">
                                    </p>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-12">
                                    <label>
                                        Password <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                    </label>
                                    <asp:TextBox ID="txtUserPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="Enter text" />
                                    <p class="help-block">
                                    </p>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-12">
                                    <label>
                                        Confirm Password <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                    </label>
                                    <asp:TextBox ID="txtConfirmedPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="Enter text" />
                                    <p class="help-block">
                                    </p>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-12">
                                    <label>
                                        Role <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                    </label>
                                    <asp:DropDownList ID="ddRoles" runat="server" CssClass="form-control">
                                        <asp:ListItem>No</asp:ListItem>
                                        <asp:ListItem>Yes</asp:ListItem>
                                    </asp:DropDownList>
                                    <p class="help-block">
                                    </p>
                                </div>
                            </div>

                            <div class="row">
                                <div class="text-center">
                                    <asp:Button ID="btnCancelRegisteration" runat="server" Text="Cancel" Width="200px" CssClass="btn btn-danger btn-md  padded-button"
                                        OnClick="btnCancel_Click" />
                                    <asp:Button ID="btnRegisterSystemUser" runat="server" Text="Regsister User" Width="200px" CssClass="btn btn-success btn-md  padded-button"
                                        OnClick="btnRegisterSystemUser_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3">
                        </div>
                    </asp:View>

                </asp:MultiView>

            </div>
        </div>
    </form>
</body>
</html>
