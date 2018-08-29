<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNewItems.aspx.cs" Inherits="TcmpWebForm.AddNewItems" %>

<html>

<head>
    <title>TCMP Shopping Site</title>
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
                    <h1 class="standOut-Heading">TCMP Shopping Site</h1>
                    <hr />
                </div>


                <div align="center">
                    <asp:Button runat="server" Text="Name: Username" ID="btnUserDetails" CssClass="btn btn-success padded-button" data-filter="all" Enabled="false" />
                    <asp:Button runat="server" Text="Add Items for Sale" ID="btnAddItems" class="btn btn-primary padded-button" data-filter="sprinkle" Enabled="true" />
                    <asp:Button runat="server" Text="Logout/Sign Out" ID="btnLogout" class="btn btn-warning padded-button" data-filter="sprinkle" OnClick="btnLogout_Click" />
                </div>
                <hr />


                <div class="row">
                    <div id="divInfo" runat="server" class="alert alert-success text-center">
                        <asp:Label runat="server" ID="lblInfoMsg">
                                Below are the Items Listed for Sale. Simply click on the item you want to add it to the shopping cart 
                        </asp:Label>
                    </div>
                </div>

                <asp:MultiView ActiveViewIndex="0" runat="server" ID="multiViewContent">


                    <%-- ------------------  REGISTER ITEM VIEW--------- --%>
                    <asp:View runat="server" ID="viewRegsisterItem">

                        <div class="row">
                            <div class="col-lg-3">
                            </div>

                            <div class="col-lg-6">

                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>
                                            Item Name <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                        </label>
                                        <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control" placeholder="Enter text" />
                                        <p class="help-block">
                                        </p>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>
                                            Item Price <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                        </label>
                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="Enter text" />
                                        <p class="help-block">
                                        </p>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>
                                            Item Count <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                        </label>
                                        <asp:TextBox ID="txtItemCount" runat="server" CssClass="form-control" placeholder="Enter text" />
                                        <p class="help-block">
                                        </p>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>
                                            Item Image <b style="color: Red; font-size: 20px"><strong>*</strong></b>
                                        </label>
                                        <asp:FileUpload accept="image/*" ID="fuItemImage" runat="server" CssClass="form-control" />
                                        <p class="help-block">
                                        </p>
                                    </div>
                                </div>

                                <hr />
                                <div class="row">
                                    <div class="text-center">
                                        <asp:Button ID="btnRegisterItem" runat="server" Text="Save Item" Width="200px" CssClass="btn btn-success btn-md"
                                            OnClick="btnRegisterItem_Click" />
                                    </div>
                                </div>
                                 <hr />

                            </div>

                            <div class="col-lg-3">
                            </div>
                        </div>

                        <div class="row" style="margin-top:15px;">

                            <table class="table">
                                <thead>
                                    <tr>
                                        <th scope="col">Item Code</th>
                                        <th scope="col">Item Name</th>
                                        <th scope="col">Item Price</th>
                                        <th scope="col">Item Count</th>
                                        <th scope="col">Update</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <% 
                                        foreach (var item in ItemsAvailableForSale)
                                        {
                                    %>
                                    <tr>
                                        <th scope="row"><%Response.Write(item.ItemCode); %></th>
                                        <td><%Response.Write(item.ItemName); %></td>
                                        <td><%Response.Write(item.ItemPrice); %></td>
                                        <td><%Response.Write(item.ItemCount); %></td>
                                        <td><a class="btn btn-default btn-md" href="AddNewItems.aspx?ItemId=<%Response.Write(item.ItemCode); %>"> Update </a></td>
                                    </tr>
                                    <%} %>
                                </tbody>
                            </table>
                        </div>
                    </asp:View>



                </asp:MultiView>

            </div>
        </div>
    </form>
</body>
</html>
