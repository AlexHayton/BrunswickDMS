<%@ Page Title="Browse Documents" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Browse.aspx.cs" Inherits="BrunswickDMS.Browse" %>
<%@ Register TagPrefix="uc" TagName="DMSSearchBox" Src="~/User Controls/DMSSearchBox.ascx" %>
<%@ Register TagPrefix="uc" TagName="DocumentListView" Src="~/User Controls/DocumentListView.ascx" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
  <script type="text/javascript">
      <!-- Set up tabs -->
      $(function() {
          $('#tabs').tabs({
              fx: [{ opacity: 'toggle', duration: 'fast' },   // hide option
                   { opacity: 'toggle', duration: 'fast' }],
              select: function (event, ui) {
                  $(ui.panel).find('input[name*=UpdatePanel]').click();
              }

          });
      });
  </script>
</asp:Content>

<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <uc:DMSSearchBox ID="SearchBox" runat="server" />
            <hgroup class="title">
                <h1><%: Title %></h1>
            </hgroup>
            <p>
                Browse your documents, or search for a document.
            </p>
        </div>
    </section>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>What now?</h3>
    <ol class="round">
        <li class="one">
            <h5>Browse the documents.</h5>
            Select a tab below to view your own documents, or look at all documents in the system.
        </li>
    </ol>

    <div id="tabs">
      <ul>
        <li><a href="#tabs-1">Your Documents</a></li>
        <li><a href="#tabs-2">Most Recent Documents</a></li>
        <li><a href="#tabs-3">All Documents</a></li>
      </ul>
      <div id="tabs-1">
        <h2>Your Documents</h2>
        <uc:DocumentListView ID="DocumentListView1" QueryMode="CurrentUser" runat="server"/>
      </div>
      <div id="tabs-2">
        <h2>Most Recent Documents</h2>
        <uc:DocumentListView ID="DocumentListView2" QueryMode="DescendingDateOrder" runat="server"/>
      </div>
      <div id="tabs-3">
        <h2>All Documents</h2>
        <uc:DocumentListView ID="DocumentListView3" runat="server"/>
      </div>
    </div>
</asp:Content>
