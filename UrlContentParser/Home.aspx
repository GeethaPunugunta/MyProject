<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="UrlContentParser.Home" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-1.12.4.min.js"></script>
    <script src="Scripts/owl.carousel.js"></script>
    <script src="Scripts/owl.carousel.min.js"></script>
    <link href="Css/owl.carousel.css" rel="stylesheet" />
    <link href="Css/owl.theme.css" rel="stylesheet" />
    <link href="Css/owl.transitions.css" rel="stylesheet" />
    <link href="Css/Site.css" rel="stylesheet" />
    <style type="text/css">
        #gallery {
            width: auto;
        }
         #gallery .item{
             margin: 3px;
         }
        #gallery .item img{
            display: block;
            width: 200px;
            height: 200px;
          
        }
      
    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
            <div id="page-wrap">
                <div id="header"><h2>URL Parser </h2></div>
                <div id="pagecontent">
               <b>Enter URL:</b> 
                <asp:TextBox runat="server" ID="txtBoxUrl" Width="250px"></asp:TextBox>
                <asp:Button runat="server" ID="btnGetData" OnClick="btnGetData_OnClick" Text="Parse Data" CssClass="myButton" />
                 <asp:RequiredFieldValidator runat="server" ID="textBoxUrlRequiredField" ControlToValidate="txtBoxUrl" ForeColor="red" 
                    ErrorMessage="URL is required" InitialValue="" ></asp:RequiredFieldValidator>
                <asp:Label runat="server" ID="lblErrorMessage" Text=""></asp:Label>

           
            <div id="gallery"></div>
    
           <div class="table left">
    <h1>  <asp:Label runat="server" ID="lbltotCount"></asp:Label></h1>  
        <asp:GridView ID="GVWords" runat="server" AutoGenerateColumns="False" HorizontalAlign="center" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="Key" HeaderText="Words" />
                <asp:BoundField DataField="Value" HeaderText="Counts" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" Width="100px"/>
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" Wrap="true" />
           
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#0000A9" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#000065" />
        </asp:GridView>
          </div>
            
        <div class="right">
           
                <asp:Chart ID="chtWordCounts" runat="server" BorderWidth="2px" BorderlineColor="232, 162, 25" Width="500px" Height="500px">
                    <Series>
                        <asp:Series Name="seriesX" XValueType="String" ToolTip="Word Count" BorderWidth="3" ChartArea="chartArea" ChartType="Bar"></asp:Series>
                        <asp:Series Name="seriesY" YValueType="Int32" ToolTip="Word" BorderWidth="3" ChartArea="chartArea" ChartType="Bar"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chartArea">
                            <AxisX Interval="1">
                                <MajorGrid LineDashStyle="Solid" LineColor="LightGray" />
                            </AxisX>
                            <AxisY>
                                <MajorGrid LineDashStyle="Solid" LineColor="LightGray" />
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

          
        </div>
       
            </div>
        </div> 
    </form>
</body>

</html>

<script type="text/javascript">
    
    function GetImagesList(input) {
        var parameters = "{'input':'" + input + "'}";
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: 'Services/ImageGalleryService.asmx/GetImagesList',
            data: parameters,
            dataType: "json",
            success: function (result) {
               
                $.each(result.d, function (i, val) {
                    var img = "<div class='item'><img src= '" + val + "' ></div>";
                    $("#gallery").append(img);

                });
                $("#gallery").owlCarousel({
                    autoPlay: 3000, //Set AutoPlay to 3 seconds
                
                    itemsDesktop: [1199, 10],
                    itemsDesktopSmall: [980, 9],
                    itemsTablet: [768, 5],
                    itemsTabletSmall: false,items:2,
                    itemsMobile: [479, 4]
                });
            },
            error: function(xmlHttpRequest) {
                var err = eval("(" + xmlHttpRequest.responseText + ")");
                alert(err.Message);
            }

        });

    }

</script>