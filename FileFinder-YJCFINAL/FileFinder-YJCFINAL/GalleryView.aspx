<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GalleryView.aspx.cs" Inherits="FileFinder_YJCFINAL.GalleryView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .mask {
            width: 500px;
            height: 300px;
            background-image: url('../Images/Blank.gif');
            position: absolute;
            top: 134px;
            opacity: 0;
        }

        .thumbnail {
            padding: 0px;
        }

        .panel {
            position: relative;
        }

            .panel > .panel-heading:after, .panel > .panel-heading:before {
                position: absolute;
                top: 11px;
                left: -16px;
                right: 100%;
                width: 0;
                height: 0;
                display: block;
                content: " ";
                border-color: transparent;
                border-style: solid solid outset;
                pointer-events: none;
            }

            .panel > .panel-heading:after {
                border-width: 7px;
                border-right-color: #f7f7f7;
                margin-top: 1px;
                margin-left: 2px;
            }

            .panel > .panel-heading:before {
                border-right-color: #ddd;
                border-width: 8px;
            }
    </style>
    <hr />
    <div class="container">

        <div class="row">
            <div class="col-lg-8">
                <!-- Title -->
                <asp:Label ID="DesignTitleLabel" Font-Size="XX-Large" runat="server"></asp:Label>

                <!-- Author -->
                <h4>Designed by
                    <asp:Label ID="NameLabel" Font-Size="Large" runat="server"></asp:Label>
                </h4>
                <hr>
                <!-- Preview Image -->
                <div class="well">

                    <asp:Image ID="SecImage" class="imagediv" Height="300px" Width="500" runat="server" />
                    <div class="mask">
                        <img alt="" src="../Images/Blank.gif" />
                    </div>
                    <hr />
                    <div class="caption-full">
                        <asp:Label ID="PriceLabel" Font-Size="Larger" class="pull-right" runat="server"></asp:Label>
                        <asp:Label ID="Titlelabel2" Font-Size="Larger" runat="server"></asp:Label>

                        <p>
                            <asp:Label ID="DescriptionLabel" runat="server"></asp:Label>
                    </div>
                    <div class="ratings">
                        <p>
                            <span class="glyphicon glyphicon-star"></span>
                            <span class="glyphicon glyphicon-star"></span>
                            <span class="glyphicon glyphicon-star"></span>
                            <span class="glyphicon glyphicon-star"></span>
                            <span class="glyphicon glyphicon-star-empty"></span>
                            4.0 stars
                        </p>
                    </div>
                </div>

                <hr>
                <!-- Comments Form -->
                <div class="well">
                    <h4>Leave a Review:</h4>

                    <div class="form-group">
                        <asp:TextBox ID="ReviewTextArea" class="form-control" Rows="3" runat="server"></asp:TextBox>
                    </div>
                    <asp:Button ID="PostBtn" class="btn btn-primary" runat="server" Text="Post" OnClick="PostBtn_Click" />
                </div>

                <hr>

                <!-- Posted Comments -->

                <!-- Comment -->
                <asp:PlaceHolder ID="PlaceHolderReview" runat="server"></asp:PlaceHolder>
                <%--<div class="media">
                    <a class="pull-left" href="#">
                        <img class="media-object" src="http://placehold.it/64x64" alt="">
                    </a>
                    <div class="media-body">
                        <h4 class="media-heading">Start Bootstrap
                            <small>August 25, 2014 at 9:30 PM</small>
                        </h4>
                        Cras sit amet nibh libero, in gravida nulla. Nulla vel metus scelerisque ante sollicitudin commodo. Cras purus odio, vestibulum in vulputate at, tempus viverra turpis. Fusce condimentum nunc ac nisi vulputate fringilla. Donec lacinia congue felis in faucibus.
                    </div>
                </div>

                <!-- Comment -->
                <div class="media">
                    <a class="pull-left" href="#">
                        <img class="media-object" src="http://placehold.it/64x64" alt="">
                    </a>
                    <div class="media-body">
                        <h4 class="media-heading">Start Bootstrap
                            <small>August 25, 2014 at 9:30 PM</small>
                        </h4>
                        Cras sit amet nibh libero, in gravida nulla. Nulla vel metus scelerisque ante sollicitudin commodo. Cras purus odio, vestibulum in vulputate at, tempus viverra turpis. Fusce condimentum nunc ac nisi vulputate fringilla. Donec lacinia congue felis in faucibus.
                        <!-- Nested Comment -->
                        <div class="media">
                            <a class="pull-left" href="#">
                                <img class="media-object" src="http://placehold.it/64x64" alt="">
                            </a>
                            <div class="media-body">
                                <h4 class="media-heading">Nested Start Bootstrap
                                    <small>August 25, 2014 at 9:30 PM</small>
                                </h4>
                                Cras sit amet nibh libero, in gravida nulla. Nulla vel metus scelerisque ante sollicitudin commodo. Cras purus odio, vestibulum in vulputate at, tempus viverra turpis. Fusce condimentum nunc ac nisi vulputate fringilla. Donec lacinia congue felis in faucibus.
                            </div>
                        </div>
                        <!-- End Nested Comment -->
                    </div>
                </div>--%>
            </div>

            <!-- Blog Sidebar Widgets Column -->
            <div class="col-md-4">

                <!-- Blog Search Well -->
                <div class="well" style="background-color: darkorange;">
                    <h4>Item Search</h4>
                    <div class="input-group">
                        <input type="text" class="form-control">
                        <span class="input-group-btn">
                            <button class="btn btn-default" type="button">
                                <span class="glyphicon glyphicon-search"></span>
                            </button>
                        </span>
                    </div>
                    <!-- /.input-group -->
                </div>
                <div class="form-group">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4>Design Files</h4>
                        </div>
                        <!-- .panel-body -->
                        <div class="form-group">

                            <label class="col-lg-4 control-label" for="inputTitle">Design Title:</label>
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label class="col-lg-4 control-label" for="inputTitle">File Size:</label>
                            <asp:Label ID="Label2" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>

                <!-- Side Widget Well -->
                <div class="well">
                    <div class="form-group" style="text-align: center">
                        <label for="input1">Avaliable For Purchase</label>
                    </div>
                    <asp:Button ID="AddToCartBtn" class="btn btn-primary btn-block" runat="server" Text="Add To cart" OnClick="Download_PurcahseBtn_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>