<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewReports.aspx.cs" Inherits="TcmpWebForm.ViewReports" %>
<%@ Import Namespace="TcmpWebForm.AppCode" %>

<html>

<head>
    <title>TCMP Shopping Site</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Bangers" rel="stylesheet">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.2/Chart.js"></script>
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
                     <asp:Button runat="server" Text="Items Stock Graph" ID="btnStockGraph" class="btn btn-primary padded-button" data-filter="sprinkle" Enabled="true" />
                    <asp:Button runat="server" Text="PaymentChannels Frequency" ID="btnAddItems" CssClass="btn btn-info padded-button" data-filter="all" Enabled="false" />
                    <asp:Button runat="server" Text="Logout/Sign Out" ID="btnLogout" class="btn btn-warning padded-button" data-filter="sprinkle" OnClick="btnLogout_Click" />
                </div>
                <hr />


                <div class="row">
                    <div id="divInfo" runat="server" class="alert alert-success text-center">
                        <asp:Label runat="server" ID="lblInfoMsg">
                               Use Above Buttons to Select Desired Graph
                        </asp:Label>
                    </div>
                </div>

                <asp:MultiView ActiveViewIndex="0" runat="server" ID="multiViewContent">


                    <%-- ------------------  ITEM GRAPH--------- --%>
                    <asp:View runat="server" ID="viewRegsisterItem">

                        <div class="row">

                            <div class="col-lg-3">
                            </div>

                            <div class="col-lg-6">
                                <% 
                                    Chart chart = LoadItemsStockReport();
                                %>
                                <canvas id="myChart" width="400" height="400"></canvas>
                                <script>
                                    var ctx = document.getElementById("myChart").getContext('2d');
                                    var myChart = new Chart(ctx, {
                                        type: 'bar',
                                        data: {
                                            labels: [<%Response.Write(chart.GetXAxisValuesAsCommaSeparatedString());%>],
                                            datasets: [{
                                                label: <%Response.Write(chart.lblYAxis);%>,
                                                data: [<%Response.Write(chart.GetYAxisValuesAsCommaSeparatedString());%>],
                                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                                borderColor: 'rgba(75, 192, 192, 1)',
                                                borderWidth: 1
                                            }]
                                        },
                                        options: {
                                            scales: {
                                                yAxes: [{
                                                    ticks: {
                                                        beginAtZero:true
                                                    }
                                                }]
                                            }
                                        }
                                    });
                                </script>

                            </div>

                            <div class="col-lg-3">
                            </div>

                        </div>

                    </asp:View>



                </asp:MultiView>

            </div>
        </div>
    </form>
</body>
</html>
