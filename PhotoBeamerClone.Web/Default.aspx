<%--<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PhotoBeamerClone.Web._Default" %>--%>

<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PhotoBeamerClone.Web._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="Scripts/jquery-1.7.1.min.js"></script>
    <script src="Scripts/jquery.signalR-1.0.0-rc2.min.js"></script>
    <script src="http://pbclone.azurewebsites.net/signalr/hubs/" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.connection.hub.url = 'http://pbclone.azurewebsites.net/signalr';
            //$.connection.hub.url = 'http://localhost:4341/signalr';
            // Proxy created on the fly
            var mainHub = $.connection.imgHub;
            var guid = '<%= HttpContext.Current.Session["InstanceGuid"] %>';

            // Declare a function on the hub so the server can invoke it
            mainHub.client.receiveImage = function (imageArray) {
                //window.location = "/ImagePage.aspx?arr=" + imageArray;
                $('#hiddenByteArray').val(imageArray);
                $('#form1').submit();
            };
           
            // Start the connection
            $.connection.hub.start().done(function () {
                mainHub.server.create(guid);
            });
        });
    </script>
</head>
<body>
    <form id="form1" method="post" action="ImagePage.aspx">
        <input type="hidden" id="hiddenByteArray" name="hiddenByteArray" />
        <div>
            <img src="/imagehandler.ashx" style="text-align: center" />
        </div>
    </form>
</body>
</html>
