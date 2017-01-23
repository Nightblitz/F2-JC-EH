using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace FileFinder_YJCFINAL
{
    public partial class MyUploads : System.Web.UI.Page
    {
        private int noOfGallery;

        //Gallery Database var
        private string title;

        private int amount;
        private string desc;

        //File Upload Database var
        private int fileuploadID;

        private int fileuploadsecretID;
        private int fileuploadsecondaryID;
        private int fileuploadsecondarysecretID;
        private string filepathMain;
        private string filepathSec;

        //temp
        private string userid = "123";

        protected void Page_Load(object sender, EventArgs e)
        {
            List<int> GalleryIDList = new List<int>();
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["F2DB"].ConnectionString))
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM [dbo].[Gallery];";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    noOfGallery = reader.GetInt32(0);
                }
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "SELECT [GalleryID] FROM [dbo].[Gallery]";
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteNonQuery();

                reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    GalleryIDList.Add(reader.GetInt32(0));
                }
                connection.Close();

                for (int i = 0; i < noOfGallery; i++)
                {
                    int GalleryID = GalleryIDList[i];

                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.CommandText = "SELECT [DesignName],[Cost],[Description] FROM [dbo].[Gallery] WHERE [GalleryID]= @GalleryID AND [UserID] = @UserID;";
                    cmd3.Parameters.AddWithValue("GalleryID", GalleryID);
                    cmd3.Parameters.AddWithValue("UserID", userid);
                    cmd3.Connection = connection;
                    connection.Open();
                    cmd3.ExecuteNonQuery();

                    reader = cmd3.ExecuteReader();
                    while (reader.Read())
                    {
                        title = reader.GetString(0);
                        amount = reader.GetInt32(1);
                        desc = reader.GetString(2);
                    }
                    connection.Close();

                    Panel panel = new Panel();
                    panel.ID = "EventPanel" + i.ToString();//Remember must put the for loop int in here
                    panel.HorizontalAlign = HorizontalAlign.Left;

                    //Head of panel
                    panel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                    panel.Controls.Add(new LiteralControl("<div class='panel panel-info'>"));
                    panel.Controls.Add(new LiteralControl("<div class='panel-heading'>"));
                    var h2 = new HtmlGenericControl("h2");
                    h2.Attributes.Add("class", "panel-title");
                    h2.InnerHtml = title;
                    panel.Controls.Add(h2);
                    panel.Controls.Add(new LiteralControl("</div>"));

                    //panel Body
                    panel.Controls.Add(new LiteralControl("<div class='panel-body'>"));
                    panel.Controls.Add(new LiteralControl("<div class='row'>"));

                    //Buttons
                    panel.Controls.Add(new LiteralControl("<div class='col-md-3'>"));

                    //Cancel Event button
                    panel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                    Button b = new Button();
                    b.Text = "Remove Design";
                    b.CssClass = "btn btn-primary";
                    b.Click += new EventHandler(b_Click);
                    b.CommandArgument = GalleryID.ToString();
                    panel.Controls.Add(b);
                    panel.Controls.Add(new LiteralControl("</div>"));//row div

                    //View Details Button
                    panel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                    Button b2 = new Button();
                    b2.Text = "Edit Details";
                    b2.CssClass = "btn btn-primary";
                    //b2.Click += new EventHandler(viewdetails_click);
                    //b2.CommandArgument = eventId.ToString();
                    panel.Controls.Add(b2);
                    panel.Controls.Add(new LiteralControl("</div>"));//row div

                    //View Details Button
                    //panel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                    //Button b2 = new Button();
                    //b2.Text = "View Details";
                    //b2.CssClass = "btn btn-primary";
                    //b2.Click += new EventHandler(viewdetails_click);
                    //b2.CommandArgument = eventId.ToString();
                    //panel.Controls.Add(b2);
                    //panel.Controls.Add(new LiteralControl("</div>"));//row div
                    panel.Controls.Add(new LiteralControl("</div>"));//col-md-3 div

                    panel.Controls.Add(new LiteralControl("<div class='col-md-9'>"));

                    //Design Name
                    panel.Controls.Add(new LiteralControl("<div class='row'>"));
                    panel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                    HtmlGenericControl label1 = new HtmlGenericControl("label");
                    label1.Attributes.Add("class", "col-lg-2 control-label");
                    label1.InnerText = "Design Name:";
                    panel.Controls.Add(label1);
                    label1.Controls.Add(new LiteralControl("</label>"));
                    Label l = new Label();
                    l.CssClass = "col-lg-10 control-label";
                    l.Text = title;
                    panel.Controls.Add(l);
                    panel.Controls.Add(new LiteralControl("</div>"));//form group div
                    panel.Controls.Add(new LiteralControl("</div>"));//row div

                    //Cost
                    panel.Controls.Add(new LiteralControl("<div class='row'>"));
                    panel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                    HtmlGenericControl label2 = new HtmlGenericControl("label");
                    label2.Attributes.Add("class", "col-lg-2 control-label");
                    label2.InnerText = "Cost:";
                    label2.Controls.Add(new LiteralControl("</label>"));
                    panel.Controls.Add(label2);
                    Label l2 = new Label();
                    l2.CssClass = "col-lg-10 control-label";
                    l2.Text = "S$" + amount.ToString() + ".00";
                    panel.Controls.Add(l2);
                    panel.Controls.Add(new LiteralControl("</div>"));//form group div
                    panel.Controls.Add(new LiteralControl("</div>"));//row div

                    //Description
                    panel.Controls.Add(new LiteralControl("<div class='row'>"));
                    panel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                    HtmlGenericControl label3 = new HtmlGenericControl("label");
                    label3.Attributes.Add("class", "col-lg-2 control-label");
                    label3.InnerText = "Description:";
                    label3.Controls.Add(new LiteralControl("</label>"));
                    panel.Controls.Add(label3);
                    Label l3 = new Label();
                    l3.CssClass = "col-lg-10 control-label";
                    l3.Text = desc;
                    panel.Controls.Add(l3);
                    panel.Controls.Add(new LiteralControl("</div>"));//form group div
                    panel.Controls.Add(new LiteralControl("</div>"));//row div

                    panel.Controls.Add(new LiteralControl("</div>"));//col-md-9 div

                    panel.Controls.Add(new LiteralControl("<div class='col-md-4'>"));

                    panel.Controls.Add(new LiteralControl("</div>"));//col-md-2 div

                    panel.Controls.Add(new LiteralControl("</div>"));//row div
                    panel.Controls.Add(new LiteralControl("</div>"));//panel body div
                    panel.Controls.Add(new LiteralControl("</div>"));//panel panel info div
                    panel.Controls.Add(new LiteralControl("</div>"));//form group div
                    PLH.Controls.Add(panel);
                }
            }
        }

        private void b_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string id = button.CommandArgument;
            deleteFromDatabase(id);
        }

        private void deleteFromDatabase(String gid)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["F2DB"].ConnectionString))
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT [FileUploadID],[FileUploadSecondaryID] FROM [dbo].[Gallery] WHERE [GalleryID]= @GalleryID AND [UserID] = @UserID;";
                cmd.Parameters.AddWithValue("GalleryID", gid);
                cmd.Parameters.AddWithValue("UserID", userid);
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    fileuploadID = reader.GetInt32(0);
                    fileuploadsecondaryID = reader.GetInt32(1);
                }
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "SELECT [FileUploadSecretID] FROM [dbo].[FileUpload] WHERE [FileUploadID]= @FileUploadID AND [UserID] = @UserID;";
                cmd2.Parameters.AddWithValue("FileUploadID", fileuploadID);
                cmd2.Parameters.AddWithValue("UserID", userid);
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteNonQuery();

                reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    fileuploadsecretID = reader.GetInt32(0);
                }
                connection.Close();

                SqlCommand cmd3 = new SqlCommand();
                cmd3.CommandText = "SELECT [FileUploadSecondarySecretID] FROM [dbo].[FileUploadSecondary] WHERE [FileUploadSecondaryID]= @FileUploadSecondaryID AND [UserID] = @UserID;";
                cmd3.Parameters.AddWithValue("FileUploadSecondaryID", fileuploadsecondaryID);
                cmd3.Parameters.AddWithValue("UserID", userid);
                cmd3.Connection = connection;
                connection.Open();
                cmd3.ExecuteNonQuery();

                reader = cmd3.ExecuteReader();
                while (reader.Read())
                {
                    fileuploadsecondarysecretID = reader.GetInt32(0);
                }
                connection.Close();

                

                SqlCommand cmd4 = new SqlCommand();
                cmd4.CommandText = "DELETE FROM [dbo].[FileUpload] WHERE [FileUploadID] = @FileUploadID AND [UserID] = @UserID;";
                cmd4.Parameters.AddWithValue("FileUploadID", fileuploadID);
                cmd4.Parameters.AddWithValue("UserID", userid);
                cmd4.Connection = connection;
                connection.Open();
                cmd4.ExecuteNonQuery();
                connection.Close();

                SqlCommand cmd5 = new SqlCommand();
                cmd5.CommandText = "DELETE FROM [dbo].[FileUploadSecondary] WHERE [FileUploadSecondaryID] = @FileUploadSecondaryID AND [UserID] = @UserID;";
                cmd5.Parameters.AddWithValue("FileUploadSecondaryID", fileuploadsecondaryID);
                cmd5.Parameters.AddWithValue("UserID", userid);
                cmd5.Connection = connection;
                connection.Open();
                cmd5.ExecuteNonQuery();
                connection.Close();

                SqlCommand cmd6 = new SqlCommand();
                cmd6.CommandText = "DELETE FROM [dbo].[FileUploadSecret] WHERE [FileUploadSecretID] = @FileUploadSecretID;";
                cmd6.Parameters.AddWithValue("FileUploadSecretID", fileuploadsecretID);
                cmd6.Connection = connection;
                connection.Open();
                cmd6.ExecuteNonQuery();
                connection.Close();

                SqlCommand cmd7 = new SqlCommand();
                cmd7.CommandText = "DELETE FROM [dbo].[FileUploadSecondarySecret] WHERE [FileUploadSecondarySecretID] = @FileUploadSecondarySecretID;";
                cmd7.Parameters.AddWithValue("FileUploadSecondarySecretID", fileuploadsecondarysecretID);
                cmd7.Connection = connection;
                connection.Open();
                cmd7.ExecuteNonQuery();
                connection.Close();

                SqlCommand cmd8 = new SqlCommand();
                cmd8.CommandText = "DELETE FROM [dbo].[Gallery] WHERE [GalleryID] = @galleryID;";
                cmd8.Parameters.AddWithValue("GalleryID", gid);
                cmd8.Connection = connection;
                connection.Open();
                cmd8.ExecuteNonQuery();
                connection.Close();
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void CreateUpload_Click(object sender, EventArgs e)
        {
            Response.Redirect("SharingCreate.aspx");
        }
    }
}