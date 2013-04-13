﻿<%@ Page 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ReportViewer.aspx.cs" 
    Inherits="Prompts.Service.ReportViewer" %>

<%@ Register 
    Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" 
    TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html 
    xmlns="http://www.w3.org/1999/xhtml" 
    style="height: 101%; padding:0px 0px;">
<head id="Head1" runat="server">
    <title></title>
</head>
<body style="height: 100%; padding:0px 0px;">
    <form id="form1" runat="server" style="height: 98%;">
    <div style="height: 100%;">
        <asp:ScriptManager ID="ScriptManager2" runat="server">
        </asp:ScriptManager>
        <table id="ViewerTable" style="height: 100%;">
            <tr>
                <td>
                    <rsweb:ReportViewer
                        ID="ReportViewerControl" 
                        AsyncRendering="true"
                        runat="server" 
                        Height="100%" 
                        Width="100%" 
                        ShowParameterPrompts="false" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
