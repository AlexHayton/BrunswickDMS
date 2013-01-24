<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="BrunswickDMS.Search" %>
<%@ Register TagPrefix="uc" TagName="DocumentListView" Src="~/User Controls/DocumentListView.ascx" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        // Wire up the autocomplete logic.
        $(document).ready(function () {
            $("#AutoCompleteText").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/Services/TagAutoComplete.asmx/GetTags",
                        dataType: "json",
                        data: "{'searchTerm':'" + request.term + "', 'limit': 5}",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.Tag,
                                    value: item.Tag
                                }
                            }))
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {

                },
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

        });

        // Wire up the search box.
        $(document).ready(function () {
            // Set the initial value of the search box based on the search term.
            var initialSearchTermValue = $('#MainContent_SearchTerm').val();
            $('#AutoCompleteText').val(initialSearchTermValue);
        });

        // Function to change the hidden ASP.NET search term value when we submit the form.
        function SetSearchTerm() {
            $('#MainContent_SearchTerm').val($('#AutoCompleteText').val());
        }
    </script>
</asp:Content>

<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
            </hgroup>
            <p>
                Search for a document
            </p>
        </div>
    </section>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" id="SearchBoxDiv">
        <asp:HiddenField ID="SearchTerm" runat="server" />
        <input id="AutoCompleteText" name="AutoCompleteText" type="search" placeholder="Enter your search term here">
        <asp:Button ID="SearchNow" runat="server" onclientclick="javascript:SetSearchTerm();"/>
    </div>

    <h2>Search Results:</h2>
    <div runat="server" id="SearchResultsDiv">
        <uc:DocumentListView ID="SearchListView" QueryMode="Search" runat="server"/>
    </div>
</asp:Content>
