<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reciept.aspx.cs" Inherits="TcmpWebForm.Reciept" %>

<!doctype html>
<html>
<head>
    <meta charset="utf-8" />
    <title>PAYMENT RECIEPT</title>
    <!-- Bootstrap Core CSS -->
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"
        rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.js"></script>
    <style type="text/css">
        .invoice-box
        {
            max-width: 800px;
            margin: auto;
            padding: 30px;
            border: 1px solid #eee;
            box-shadow: 0 0 10px rgba(0, 0, 0, .15);
            font-size: 16px;
            line-height: 24px;
            font-family: 'Helvetica Neue' , 'Helvetica' , Helvetica, Arial, sans-serif;
            color: #555;
        }
        
        .invoice-box table
        {
            width: 100%;
            line-height: inherit;
            text-align: left;
        }
        
        .invoice-box table td
        {
            padding: 5px;
            vertical-align: top;
        }
        
        .invoice-box table tr td:nth-child(2)
        {
            text-align: right;
        }
        
        .invoice-box table tr.top table td
        {
            padding-bottom: 20px;
        }
        
        .invoice-box table tr.top table td.title
        {
            font-size: 45px;
            line-height: 45px;
            color: #333;
        }
        
        .invoice-box table tr.information table td
        {
            padding-bottom: 40px;
        }
        
        .invoice-box table tr.heading td
        {
            background: #eee;
            border-bottom: 1px solid #ddd;
            font-weight: bold;
        }
        
        .invoice-box table tr.details td
        {
            padding-bottom: 20px;
        }
        
        .invoice-box table tr.item td
        {
            border-bottom: 1px solid #eee;
        }
        
        .invoice-box table tr.item.last td
        {
            border-bottom: none;
        }
        
        .invoice-box table tr.total td:nth-child(2)
        {
            border-top: 2px solid #eee;
            font-weight: bold;
        }
        
        @media only screen and (max-width: 600px)
        {
            .invoice-box table tr.top table td
            {
                width: 100%;
                display: block;
                text-align: center;
            }
        
            .invoice-box table tr.information table td
            {
                width: 100%;
                display: block;
                text-align: center;
            }
        }
        
        .auto-style1
        {
            width: 100%;
            height: 2px;
        }
        
        @media print
        {
            .hide-print
            {
                display: none;
            }
        }
    </style>
    
</head>
<body>
    <form id="Form1" runat="server">
    <div class="row" style="padding-top: 100px; padding-bottom: 10px">
        <div class="text-center">
            <input id="Button3" runat="server" accesskey="P" class="btn btn-success btn-md hide-print"
                onclick="window.print();" value="Print this Receipt" style="padding-bottom: 5px;" />
            <asp:Button runat="server" ID="btnReturn" CssClass="btn btn-primary btn-md hide-print"
                Style="width: 150px" OnClick="btnReturn_Click" Text="Go Back Home" />
        </div>
    </div>
    <asp:MultiView ID="Multiview1" ActiveViewIndex="0" runat="server">
        <asp:View ID="RecieptView" runat="server">
            <div class="invoice-box table-responsive" style="padding-top: 50px">
                <table cellpadding="0" cellspacing="0">
                    <tr class="heading">
                        <td>
                            Payment Details
                        </td>
                        <td>
                            Value
                        </td>
                    </tr>
                    <tr class="item">
                        <td>
                            <asp:Label ID="Label2" runat="server">TMCG Shop Transaction Id</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblShopId" runat="server">$10.00</asp:Label>
                        </td>
                    </tr>
                    <tr class="item">
                        <td>
                            <asp:Label ID="Label1" runat="server">Payment Transaction Id</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblExternalId" runat="server">$10.00</asp:Label>
                        </td>
                    </tr>
                    <tr class="item">
                        <td>
                            <asp:Label ID="Label6" runat="server">Customer Name</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCustName" runat="server">$10.00</asp:Label>
                        </td>
                    </tr>
                    <tr class="item">
                        <td>
                            <asp:Label ID="Label5" runat="server">Custoer Contact</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCustContact" runat="server">$10.00</asp:Label>
                        </td>
                    </tr>
                    <tr class="item">
                        <td>
                            <asp:Label ID="Label4" runat="server">Payment Date Time</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPaymentDate" runat="server">$10.00</asp:Label>
                        </td>
                    </tr>
                    <tr class="item">
                        <td>
                            <asp:Label ID="lblDesc" runat="server">Item Desc</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblItemDesc" runat="server">$10.00</asp:Label>
                        </td>
                    </tr>
                    <tr class="total">
                        <td>
                            <asp:Label ID="Label7" runat="server">Total Payment Amount</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTotalTranAmount" runat="server">$10.00</asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
        <asp:View ID="ViewError" runat="server">
            <div class="jumbotron text-center" style="padding-top: 10px; padding-bottom: 10px">
                <h3 class="display-3">
                    <asp:Label ID="lblMsg" runat="server" />
                </h3>
            </div>
        </asp:View>
        
    </asp:MultiView>
    </form>
</body>
</html>
