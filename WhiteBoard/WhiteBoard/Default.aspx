<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-family:Verdana">
    Identify yourself: <asp:TextBox runat="server" ID="txtName"></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ID="vldName" ControlToValidate="txtName">*</asp:RequiredFieldValidator>
        <asp:Button runat="server" ID="btnJoinDiscussion"
            OnClick="btnJoinDiscussion_Click" Text="Join Discussion" />
    </div>
    </form>
</body>
</html>
