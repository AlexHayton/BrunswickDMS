<%@ Page Title="Browse Documents" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Browse.aspx.cs" Inherits="BrunswickDMS.Browse" %>
<%@ Register TagPrefix="uc" TagName="DocumentListView" Src="~/User Controls/DocumentListView.ascx" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
  <script type="text/javascript">
      $(function() {
          $('#tabs').tabs({
              fx: [{ opacity: 'toggle', duration: 'fast' },   // hide option
                   { opacity: 'toggle', duration: 'fast' }]
          });
      });
  </script>
</asp:Content>

<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
            </hgroup>
            <p>
                Browse your documents, or search for a document.
            </p>
        </div>
    </section>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="tabs">
      <ul>
        <li><a href="#tabs-1">Your Documents</a></li>
        <li><a href="#tabs-2">Most Recent Documents</a></li>
        <li><a href="#tabs-3">All Documents</a></li>
      </ul>
      <div id="tabs-1">
        <h3>Your Documents</h3>
        <uc:DocumentListView ID="DocumentListView1" QueryMode="CurrentUser" runat="server"/>
      </div>
      <div id="tabs-2">
        <h3>Most Recent Documents</h3>
        <uc:DocumentListView ID="DocumentListView2" QueryMode="DescendingDateOrder" runat="server"/>
      </div>
      <div id="tabs-3">
        <h3>All Documents</h3>
        <uc:DocumentListView ID="DocumentListView3" runat="server"/>
      </div>
    </div>
</asp:Content>
