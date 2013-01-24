<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="BrunswickDMS.Search" %>
<%@ Register TagPrefix="uc" TagName="DMSSearchBox" Src="~/User Controls/DMSSearchBox.ascx" %>
<%@ Register TagPrefix="uc" TagName="DocumentListView" Src="~/User Controls/DocumentListView.ascx" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
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
                Search for a document
            </p>
        </div>
    </section>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>What now?</h3>
    <ol class="round">
        <li class="one">
            <h5>Review the documents.</h5>
            Look at the documents you just searched for here.
        </li>
    </ol>

    <div id="tabs">
      <ul>
        <li><a href="#tabs-1">Found documents</a></li>
      </ul>
      <div id="tabs-1">
        <h2>Search Results for: <%: SearchBox.SearchTerm.Value %></h2>
        <uc:DocumentListView ID="SearchListView" QueryMode="Search" runat="server"/>
      </div>
    </div>
</asp:Content>
