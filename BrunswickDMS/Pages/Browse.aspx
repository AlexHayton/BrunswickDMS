<%@ Page Title="Browse Documents" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Browse.aspx.cs" Inherits="BrunswickDMS.Browse" %>
<%@ Register TagPrefix="uc" TagName="DocumentListView" Src="~/User Controls/DocumentListView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>Browse documents</h2>
            </hgroup>
            <p>
                Browse your documents, or search for a document.
            </p>
        </div>
    </section>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Your Documents</h3>
    <uc:DocumentListView runat="server"/>
</asp:Content>
