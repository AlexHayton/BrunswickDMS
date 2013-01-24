using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BrunswickDMS.User_Controls
{
    public partial class DMSSearchBox : System.Web.UI.UserControl
    {
        public global::System.Web.UI.WebControls.HiddenField SearchTerm;
        public global::System.Web.UI.WebControls.Button SearchNow;

        protected void Page_Load(object sender, EventArgs e)
        {
            string searchBoxInitScript = @"
            // Wire up the autocomplete logic.
            $(document).ready(function () {
                $('#AutoCompleteText').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: 'POST',
                            contentType: 'application/json; charset=utf-8',
                            url: '/Services/TagAutoComplete.asmx/GetTags',
                            dataType: 'json',
                            data: ""{'searchTerm':'"" + request.term + ""', 'limit': 5}"",
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
                        $(this).removeClass('ui-corner-all').addClass('ui-corner-top');
                    },
                    close: function () {
                        $(this).removeClass('ui-corner-top').addClass('ui-corner-all');
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });

            });

            // Wire up the search box.
            $(document).ready(function () {
                // Set the initial value of the search box based on the search term.
                var initialSearchTermValue = $('input[name*=""SearchBox$SearchTerm""]').val();
                $('#AutoCompleteText').val(initialSearchTermValue);
            });

            // Function to change the hidden ASP.NET search term value when we submit the form.
            function SetSearchTerm() {
                $('input[name*=""SearchBox$SearchTerm""]').val($('#AutoCompleteText').val());
            }";

            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "searchBox", searchBoxInitScript, true);
        }
    }
}