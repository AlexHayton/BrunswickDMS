<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentListView.ascx.cs" Inherits="BrunswickDMS.DocumentListView" %>
<!-- This list view is used wherever we want to display documents
     Key features are that it can refresh independently of the rest of the page
     It also supports a parameter called QueryMode that affects its behaviour and
     a parameter called SearchTerm that allows filtering by text -->
<asp:UpdatePanel ID="DocumentListUpdatePanel" 
                 UpdateMode="Conditional"
                 runat="server">
    <ContentTemplate>
        <!-- This button is used by some pages to refresh the inner panel at will -->
        <asp:Button style="display:none;" ID="UpdatePanel" runat="server" />
        <asp:ListView ID="DocumentsView" ItemType="DataLayer.Models.Document" runat="server" SelectMethod="GetDocuments">
            <EmptyDataTemplate>
                <p>No documents found! <a href="/Pages/Upload.aspx">Click here to upload a document.</a></p>
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
                            Date
                        </th>
                        <th>
                            Delete
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
                    <asp:Label Visible="false" ID="DocumentId" runat="server" Text='<%# Eval("DocumentId") %>' />
                    <td>
                        <asp:Image ID="DocumentTypeImage" runat="server" Width="3.5em" Height="3.5em" ImageUrl='<%# GetIconForDocument(Eval("DocType"), Eval("DocumentId")) %>' />
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
                        <asp:Button ID="DocumentDeleteButton" runat="server" Text="Delete" onclick="delete_OnClick" />
                    </td>
                    <td>
                        <asp:Button ID="DocumentDownloadButton" runat="server" PostBackUrl='<%# GetDownloadLinkForDocument(Eval("DocumentId")) %>' target="_blank" Text="Download" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </ContentTemplate>
</asp:UpdatePanel>
