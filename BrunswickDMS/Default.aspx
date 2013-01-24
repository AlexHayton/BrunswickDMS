<%@ Page Title="Brunswick DMS" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BrunswickDMS._Default" %>
<%@ Register TagPrefix="uc" TagName="DMSSearchBox" Src="~/User Controls/DMSSearchBox.ascx" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <%--Only show the search box if logged in.--%>
            <% if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated) { %>
            <uc:DMSSearchBox ID="SearchBox" runat="server" />
            <% } %>

            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>The home of your documents.</h2>
            </hgroup>
            <p>
                Welcome to the Brunswick DMS. This system is designed to store documents and allow you to search and share them with your team.
            </p>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>What now?</h3>
    <ol class="round">
        <li class="one">
            <h5>Log in to the system.</h5>
            Create an account and log in via the <a href="/Account/Register.aspx">Register</a>/<a href="/Account/Login.aspx">Login</a> buttons.
        </li>
        <li class="two">
            <h5>Upload documents</h5>
            Use the <a href="/Pages/Upload.aspx">Upload</a> menu item to upload documents into the system.
        </li>
        <li class="three">
            <h5>Find documents</h5>
            You can <a href="/Pages/Browse.aspx">browse</a> through documents that you have already uploaded or <a href="/Pages/Search.aspx">search</a> for documents that have been uploaded by other people.
        </li>
    </ol>
</asp:Content>
