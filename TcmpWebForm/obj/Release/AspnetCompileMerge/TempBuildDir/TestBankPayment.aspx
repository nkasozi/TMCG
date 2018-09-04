<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestBankPayment.aspx.cs" Inherits="TcmpWebForm.TestBankPayment" %>

<html>

<head>
    <title>TMCG Shopping Site</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Bangers" rel="stylesheet">
</head>

<body>
    <form runat="server">
        <div class="row" style="border-top-left-radius: 10px; border-top-left-radius: 10px;">
            <div class="col-lg-4">
            </div>
            <div class="col-lg-4">
                <%-------------------------------------------------- Header Bar  ----------------------------------------%>
                <div class="row" style="height: 5px;">
                </div>
                <%----------------------------------------------- Main Form  -------------------------------------------%>
                <div class="row alert alert-success text-center" style="padding-top: 20px;">

                    <strong>
                        <asp:Label ID="lblErrorMsg" runat="server"> Test Bank Payments Page</asp:Label>
                    </strong>
                </div>
                <%------------------------------------------------- Payment Details  ----------------------------------%>
                <div class="row">
                    <%------------------------------------------- Payment Details  ---------------------------%>
                    <div class="card sbuColors text-white">
                        <div class="card-header text-center" style="color: black">
                            <asp:Label ID="lblMsg" runat="server">PAYMENT DETAILS</asp:Label>
                        </div>
                        <div id="Div1" class="card-body bg-default text-center" runat="server">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text" id="basic-addon1">Sale ID&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                        </div>
                                        <asp:TextBox ID="txtTranID" runat="server" CssClass="form-control" placeholder="Enter text" />
                                    </div>
                                    <p class="help-block"></p>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text" id="basic-addon1">Payer Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                        </div>
                                        <asp:TextBox ID="txtCustName" runat="server" CssClass="form-control" placeholder="Enter text" />
                                    </div>
                                    <p class="help-block"></p>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text" id="basic-addon1">Payer Phone&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                        </div>
                                        <asp:TextBox ID="txtCustPhone" runat="server" CssClass="form-control" placeholder="Enter text" />
                                    </div>
                                    <p class="help-block"></p>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text" id="basic-addon1">Total Amount&nbsp;&nbsp;</span>
                                        </div>
                                        <asp:TextBox ID="txtItemTotal" runat="server" CssClass="form-control" placeholder="Enter text" />
                                    </div>
                                    <p class="help-block"></p>
                                </div>
                            </div>
                        </div>
                        <div class="card-footer bg-default text-center">
                            <div class="row text-center">
                                <div class="col-6">
                                    <asp:Button ID="btnVerifyPRN" CssClass="btn btn-primary padded-button" Text="Verify the Ref" runat="server"
                                        OnClick="btnVerify_Click" />
                                </div>
                                <div class="col-6">
                                    <asp:Button ID="btnConfirm" CssClass="btn btn-success  padded-button" Text="Make the Payment" runat="server"
                                        OnClick="btnConfirm_Click" />
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
            </div>
        </div>
    </form>
</body>
</html>
