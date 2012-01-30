<%@ Control Language="C#" CodeBehind="ForeignKey.ascx.cs" Inherits="Vidpub.Admin.ForeignKeyField" %>

<asp:HyperLink ID="HyperLink1" runat="server"
    Text="<%# GetDisplayString() %>"
    NavigateUrl="<%# GetNavigateUrl() %>"  />

