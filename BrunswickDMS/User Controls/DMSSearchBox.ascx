<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DMSSearchBox.ascx.cs" Inherits="BrunswickDMS.User_Controls.DMSSearchBox" %>
<!-- Search box -->
<div runat="server" id="SearchBoxDiv" style="float:right;">
    <asp:HiddenField ID="SearchTerm" runat="server" />
    <input id="AutoCompleteText" name="AutoCompleteText" type="search" placeholder="Enter your search term here">
    <asp:Button ID="SearchNow" runat="server" onclientclick="javascript:SetSearchTerm();" PostBackUrl="/Pages/Search.aspx" Text="Search!"/>
</div>
