<%@ Page Title="Brunswick DMS" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BrunswickDMS._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>Start here.</h2>
            </hgroup>
            <p>
                Welcome to the Brunswick DMS 
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>What now?</h3>
    <ol class="round">
        <li class="one">
            <h5>Log in to the system.</h5>
            Create an account and log in via the Register/Login buttons above.
        </li>
        <li class="two">
            <h5>Upload documents</h5>
            Use the Upload menu item to upload documents into the system.
        </li>
        <li class="three">
            <h5>Find documents</h5>
            You can browse through documents that you have already uploaded or use the search functionality to locate documents uploaded by other people.
        </li>
    </ol>
</asp:Content>
