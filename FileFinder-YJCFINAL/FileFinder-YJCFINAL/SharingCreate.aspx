<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SharingCreate.aspx.cs" Inherits="FileFinder_YJCFINAL.SharingCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <hr />
    <fieldset>
        <legend>Share/Sell Your Files</legend>
        <div class="form-group">
            <label for="inputTitle">Product Title</label>
            <asp:TextBox ID="TitleTextBox" maxlength="20" class="form-control" placeholder="Elephants" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator
                ID="RequiredFieldValidator1"
                runat="server"
                ControlToValidate="TitleTextBox"
                CssClass="text-danger"
                ErrorMessage="The title field is required!!">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="TitleRegex" runat="server"
                ControlToValidate="TitleTextBox"
                ValidationExpression="^[a-zA-Z0-9\s]{1,20}$"
                ErrorMessage="Invalid Title"
                 CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>

        <div class="form-group">
            <label for="inputDescription">Design Description</label>
            <asp:TextBox ID="DescriptionTextBox" MaxLength="500" TextMode="multiline" Columns="20" Rows="4" class="form-control" placeholder="This is an awesome design!!" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator
                ID="RequiredFieldValidator2"
                runat="server"
                ControlToValidate="DescriptionTextBox"
                CssClass="text-danger"
                ErrorMessage="The Description field is required">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                ControlToValidate="DescriptionTextBox"
                ValidationExpression="^[a-zA-Z0-9'.,--&\s]{1,500}$"
                ErrorMessage="Invalid Description(Forbidden Character Detected)"
                 CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
        <div class="form-group">
            <label for="inputCategory">Category</label>
            <div class="input-group-btn">
                <asp:DropDownList ID="CategoryDropDownList" class="form-control" runat="server">
                    <asp:ListItem>Select A Category</asp:ListItem>
                    <asp:ListItem>Arts & Design</asp:ListItem>
                    <asp:ListItem>Technology</asp:ListItem>
                    <asp:ListItem>Fun</asp:ListItem>
                    <asp:ListItem>Fantasy</asp:ListItem>
                    <asp:ListItem>Others</asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:RequiredFieldValidator
                ID="RequiredFieldValidator3"
                runat="server"
                ControlToValidate="CategoryDropDownList"
                CssClass="text-danger"
                InitialValue="Select A Category"
                ErrorMessage="Please choose a category!!">
            </asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-sm-3">
                    <div class="input-group">
                        <div class="input-group-addon">Earn$</div>
                        <asp:TextBox ID="CostTextBox" MaxLength="4" class="form-control" placeholder="Amount" runat="server"></asp:TextBox>
                        <div class="input-group-addon">.00</div>
                    </div>
                </div>
                <asp:RangeValidator
                    ID="RangeValidator1"
                    runat="server"
                    ControlToValidate="CostTextBox"
                    ErrorMessage="Product only to be sold from S$0 -1000"
                    MaximumValue="1000"
                    MinimumValue="0"
                    CssClass="text-danger"
                    Type="Integer">
                </asp:RangeValidator>
            </div>
            <asp:RequiredFieldValidator
                ID="RequiredFieldValidator5"
                runat="server"
                ControlToValidate="CostTextBox"
                CssClass="text-danger"
                ErrorMessage="The Cost field is required">
            </asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <asp:Button ID="NextBtn" class="btn btn-primary" runat="server" OnClick="NextBtn_Click" Text="Next" />
        </div>
    </fieldset>
</asp:Content>