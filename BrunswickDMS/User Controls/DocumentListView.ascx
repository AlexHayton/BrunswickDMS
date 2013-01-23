<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentListView.ascx.cs" Inherits="BrunswickDMS.DocumentListView" %>
<asp:ListView ID="DocumentsView" ItemType="DataLayer.Models.Document" runat="server" SelectMethod="GetDocuments">
    <EmptyDataTemplate>
        <p>No documents in the system!</p>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table border="1" runat="server" id="documentViewContainer">
            <tr>
                <th>
                    Type
                </th>
                <th>
                    Name
                </th>
                <th>
                    Size
                </th>
                <th>
                    Author
                </th>
                <th>
                    Uploaded
                </th>
                <th>
                    Link
                </th>
            </tr>
            <tr id="itemPlaceholder" runat="server">
            </tr>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:Image ID="DocumentTypeImage" runat="server" Width="4em" Height="4em" ImageUrl='<%# GetIconForDocument(Eval("DocType"), Eval("DocumentId")) %>' />
            </td>
            <td>
                <asp:Label ID="DocumentNameLabel" runat="server" Text='<%# Eval("Name") %>' />
            </td>
            <td>
                <asp:Label ID="DocumentSizeLabel" runat="server" Text='<%# GetHumanReadableFileSize(Eval("DocSize")) %>' />
            </td>
            <td>
                <asp:Label ID="DocumentAuthorLabel" runat="server" Text='<%# Eval("Author.UserName") %>' />
            </td>
            <td>
                <asp:Label ID="DocumentUploadedLabel" runat="server" Text='<%# Eval("CreatedDate") %>' />
            </td>
            <td>
                <a href='<%# GetDownloadLinkForDocument(Eval("DocumentId")) %>' target="_blank">Get</a>
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>
