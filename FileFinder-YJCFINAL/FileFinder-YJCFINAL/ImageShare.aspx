<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImageShare.aspx.cs" Inherits="FileFinder_YJCFINAL.ImageShare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="panel-body">
            <div class="col-lg-12">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h1>Files - How to prepare Your Design!!
                        </h1>
                    </div>
                    <div class="panel-body">
                        <div class="col-xs-8 col-sm-6">
                            <div class="form-group">
                                <h2>Things to remember:</h2>
                                <ul>
                                    <li>We accept only common file tpes like Png </li>
                                    <li>Only 1 file is allowed to be uploaded</li>
                                    <li>5MB is the maximum total file size</li>
                                </ul>
                            </div>
                        </div>
                        <div class="col-xs-8 col-sm-6">
                            <div class="panel panel-info">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Details of Design</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-2">
                                                <asp:Label ID="Label1" runat="server" Text="Title:"></asp:Label>
                                            </div>
                                            <asp:Label ID="DesignTitleLabel" class="col-lg-10 control-label" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-2">
                                                <asp:Label ID="Label2" runat="server" Text="Category:"></asp:Label>
                                            </div>
                                            <asp:Label ID="CategoryLabel" class="col-lg-10 control-label" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-2">
                                                <asp:Label ID="label3" runat="server" Text="Cost:"></asp:Label>
                                            </div>
                                            <asp:Label ID="CostLabel" class="col-lg-10 control-label" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-2">
                                                <asp:Label ID="Label4" runat="server" Text="Seller:"></asp:Label>
                                            </div>
                                            <asp:Label ID="SellerLabel" class="col-lg-10 control-label" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-8 col-sm-6">
                                <label for="inputMainFile">Main File</label>
                                <asp:FileUpload class="custom-file-input" ID="FileUploadMain" runat="server" />
                                <asp:Button ID="PreviewBtnMain" Style="margin: 5px;" class="btn btn-success" runat="server" OnClick="btnPreviewMain_Click" Text="Preview" />
                                <asp:Button ID="DeleteBtnMain" Style="margin: 5px;" class="btn btn-danger" Text="Delete" runat="server" OnClick="btnDeleteMain_Click" />
                                <div class="form-group">
                                    <asp:Image ID="MainUploadedImage" BorderColor="Black" BorderStyle="Solid" runat="server" />
                                    <%-- <video id="VidZone" runat="server" width="320" height="240" autoplay>
                                    </video>--%>
                                </div>
                                <div class="row">
                                    <asp:Label ID="StatusLabelMain" Style="font-size: medium" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div id="ImgSecDisplayPanel" runat="server" class="col-xs-8 col-sm-6">
                                <div class="panel panel-danger">
                                    <div class="panel-heading">
                                        <h3 class="panel-title">Display Images
                                        </h3>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <label for="XtraStuff">**Images here will be displayed for customers</label>
                                        </div>
                                        <div class="form-group">
                                            <asp:Image ID="SecondaryUploadedImage" BorderColor="Black" BorderStyle="Solid" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="ShareBtn" class="btn btn-warning" runat="server" Text="Share" OnClick="ShareBtn_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>