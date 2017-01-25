<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyUploads.aspx.cs" Inherits="FileFinder_YJCFINAL.MyUploads" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="container">
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h1>
                            My Account
                            <asp:Button ID="ToCreateUploadBtn" Style="float: right" class="btn btn-success" runat="server" Text="Upload Your Design" OnClick="CreateUpload_Click" />
                    </h1>
                            </div>
                    <!-- .panel-body -->
                    <div class="col-md-6"">
                        <div class="form-group">
                            <div class="panel panel-info">
                                <div class="panel-heading">
                                    <h2 class="panel-title"> 
                                        My Uploads
                                    </h2>
                                </div>
                                 <asp:PlaceHolder ID="PLH" runat="server"></asp:PlaceHolder>
                            </div>
                        </div> 
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="panel panel-info">
                                <div class="panel-heading">
                                    <h2 class="panel-title"> 
                                        My Purchases
                                    </h2>
                                </div>
                                <asp:PlaceHolder ID="PLH2" runat="server"></asp:PlaceHolder>
                            </div>
                        </div>
                    </div>
                   </div>
                </div>
                <!-- /.panel -->
            </div>
            <!-- /.col-lg-12 -->
        </div>
        <!-- /.row -->

        <%--<div class="modal" id="ViewDetail">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Interview Attendance</h4>
                    </div>
                    <div class="modal-body">
                        <asp:GridView ID="GridView2" AutoGenerateColumns="false" class="table table-striped" DataKeyNames="Name" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                <asp:BoundField DataField="Response" HeaderText="Response" SortExpression="Response" />
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="Black" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>--%>
    
</asp:Content>
