<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="Prompts.Service.ReportViewer" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html, body, form, #content {
            height: 100%;
            width: 100%;
        }

        body, form {
            margin: 0px;
            padding: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
    <div id="content">
        <rsweb:ReportViewer
            ID="ReportViewerControl" 
            AsyncRendering="true"
            runat="server" 
            Height="100%" 
            Width="100%" 
            ShowParameterPrompts="false" />
    </div>
    </form>
</body>
</html>