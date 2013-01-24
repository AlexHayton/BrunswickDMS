<%@ Page Title="Upload" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="BrunswickDMS.Upload" %>
<%@ Register TagPrefix="uc" TagName="DMSSearchBox" Src="~/User Controls/DMSSearchBox.ascx" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function onSilverlightError(sender, args) {

            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }
            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            var errMsg = "Unhandled Error in Silverlight 2 Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }

        $(function () {
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
                Upload new documents to the system.
            </p>
        </div>
    </section>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>What now?</h3>
    <ol class="round">
        <li class="one">
            <h5>Select documents to upload.</h5>
            Click "Select Files", then choose the documents from the file dialog.
        </li>
        <li class="two">
            <h5>Upload the files.</h5>
            Click 'Upload' and wait for the documents to upload.
        </li>
        <li class="two">
            <h5>Search or browse.</h5>
            Either click the Browse button in the top menu or type a search term to find documents.<br />
            Microsoft Word 2007 documents will be automatically scanned for terms, which should then appear in the search box as you type.
        </li>
    </ol>

    <div id="tabs">
      <ul>
        <li><a href="#tabs-1">Found documents</a></li>
      </ul>
      <div id="tabs-1">
        <h2>Upload files:</h2>
        <div id="silverlightControlHost" >
            <object id="MultiFileUploader" data="data:application/x-silverlight-2," type="application/x-silverlight-2" style="text-align: center; width:600px; height:350px;">
                <param name="source" value="/ClientBin/mpost.SilverlightMultiFileUpload.xap" />
                <param name="onerror" value="onSilverlightError" />
                <param name="initParams" value="MaxFileSizeKB=,MaxUploads=2,FileFilter=,ChunkSize=4194304,CustomParams=yourparameters,DefaultColor=White" />
                <param name="background" value="white" />
                <param name="onload" value="pluginLoaded" />
                <param name="minRuntimeVersion" value="5.0.61118.0" />
                <param name="autoUpgrade" value="true" />
                <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=5.0.61118.0" style="text-decoration: none">
                    <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight"
                        style="border-style: none" />
                </a>
            </object>
        </div>
        <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px;
            border: 0px"></iframe>
      </div>
    </div>
</asp:Content>
