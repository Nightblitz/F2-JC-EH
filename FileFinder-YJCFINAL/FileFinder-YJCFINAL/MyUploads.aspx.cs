using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace FileFinder_YJCFINAL
{
    public partial class MyUploads : System.Web.UI.Page
    {
        private int noOfGallery;
        private int noOfUserProdLink;

        //Gallery Database var
        private string title;

        private int amount;
        private string desc;
        private string DecryptDataKeyGallery;

        //File Upload Database var
        private int fileuploadID;

        private byte[] ImgDataMain;
        private int fileuploadsecretID;
        private int fileuploadsecondaryID;
        private int fileuploadsecondarysecretID;

        //FileUpload Secret database var
        private string embeddedsecrettextMain;

        private string embeddedsecrettextkeyMain;
        private string DecryptDataKeyMain;

        //MY PURACHASES
        private byte[] ImgDataDownload;
        private string mediaNameDownload;
        private int fileuploadsecretDownloadID;
        private string embeddedsecrettextDownload;
        private string embeddedsecrettextkeyDownload;
        private string DecryptDataKeyDownload;

        //temp
        private string userid = "123";

        protected void Page_Load(object sender, EventArgs e)
        {
            List<int> GalleryIDList = new List<int>();
            List<int> FileUploadIDList = new List<int>();
            List<int> FileUploadIDDownloadList = new List<int>();

            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["F2DB"].ConnectionString))
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM [dbo].[Gallery] WHERE [UserID]= @UserID;";
                cmd.Parameters.AddWithValue("UserID", userid);
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
                cmd2.CommandText = "SELECT [GalleryID],[FileUploadID] FROM [dbo].[Gallery]  WHERE [UserID]= @UserID;";
                cmd2.Parameters.AddWithValue("UserID", userid);
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteNonQuery();

                reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    GalleryIDList.Add(reader.GetInt32(0));
                    FileUploadIDList.Add(reader.GetInt32(1));
                }
                connection.Close();

                for (int i = 0; i < noOfGallery; i++)
                {
                    int GalleryID = GalleryIDList[i];
                    int FileUploadID = FileUploadIDList[i];

                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.CommandText = "SELECT [DesignName] FROM [dbo].[Gallery] WHERE [GalleryID]= @GalleryID AND [UserID] = @UserID;";
                    cmd3.Parameters.AddWithValue("GalleryID", GalleryID);
                    cmd3.Parameters.AddWithValue("UserID", userid);
                    cmd3.Connection = connection;
                    connection.Open();
                    cmd3.ExecuteNonQuery();

                    reader = cmd3.ExecuteReader();
                    while (reader.Read())
                    {
                        title = reader.GetString(0);
                    }
                    connection.Close();

                    SqlCommand cmd4 = new SqlCommand();
                    cmd4.CommandText = "SELECT [ImgData],[FileUploadSecretID] FROM [dbo].[FileUpload] WHERE [FileUploadID]= @FileUploadID AND [UserID] = @UserID;";
                    cmd4.Parameters.AddWithValue("FileUploadID", FileUploadID);
                    cmd4.Parameters.AddWithValue("UserID", userid);
                    cmd4.Connection = connection;
                    connection.Open();
                    cmd4.ExecuteNonQuery();

                    reader = cmd4.ExecuteReader();
                    while (reader.Read())
                    {
                        //Image
                        long length = reader.GetBytes(0, 0, null, 0, 0);
                        Byte[] buffer = new Byte[length];
                        reader.GetBytes(0, 0, buffer, 0, (int)length);
                        ImgDataMain = buffer;

                        fileuploadsecretID = reader.GetInt32(1);
                    }
                    connection.Close();

                    SqlCommand cmd5 = new SqlCommand();
                    cmd5.CommandText = "SELECT [EmbeddedSecretText],[EmbeddedSecretTextKey],[SecretKey] FROM [dbo].[FileUploadSecret] WHERE [FileUploadSecretID]= @FileUploadSecretID;";
                    cmd5.Parameters.AddWithValue("FileUploadSecretID", fileuploadsecretID);
                    cmd5.Connection = connection;
                    connection.Open();
                    cmd5.ExecuteNonQuery();

                    reader = cmd5.ExecuteReader();
                    while (reader.Read())
                    {
                        embeddedsecrettextMain = reader.GetString(0);
                        embeddedsecrettextkeyMain = reader.GetString(1);
                        DecryptDataKeyMain = reader.GetString(2);
                    }
                    connection.Close();

                    SqlCommand cmd6 = new SqlCommand();
                    cmd6.CommandText = "SELECT [SecretKey] FROM [dbo].[GallerySecret] WHERE [GalleryID]= @GalleryID;";
                    cmd6.Parameters.AddWithValue("GalleryID", GalleryID);
                    cmd6.Connection = connection;
                    connection.Open();
                    cmd6.ExecuteNonQuery();

                    reader = cmd6.ExecuteReader();
                    while (reader.Read())
                    {
                        DecryptDataKeyGallery = reader.GetString(0);
                    }
                    connection.Close();

                    title = Cryptography.DecryptOfData(title, DecryptDataKeyGallery);
                    embeddedsecrettextMain = Cryptography.DecryptOfData(embeddedsecrettextMain, DecryptDataKeyMain);
                    embeddedsecrettextkeyMain = Cryptography.DecryptOfData(embeddedsecrettextkeyMain, DecryptDataKeyMain);

                    if (ImgDataMain != null)
                    {
                        System.Drawing.Image picMain = byteArrayToImage(ImgDataMain);
                        Bitmap bmpMain = new Bitmap(picMain);

                        //Extraction of secret text
                        string ExtractedTextMain = ImageStenography.extractText(bmpMain);

                        //Decrytion of secret text
                        string plainExtractedTextMain = ImageStenography.DecryptImageSecretTextAesIntoString(ExtractedTextMain, embeddedsecrettextkeyMain);
                        string originalPlainTextMain = ImageStenography.DecryptImageSecretTextAesIntoString(embeddedsecrettextMain, embeddedsecrettextkeyMain);

                        if (originalPlainTextMain == plainExtractedTextMain)
                        {
                            string imgData;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                bmpMain.Save(ms, ImageFormat.Png);
                                byte[] byteImageMain = ms.ToArray();
                                string base64StringImageMain = Convert.ToBase64String(byteImageMain);
                                imgData = "data:image/png;base64," + base64StringImageMain;
                            }
                            //My Uploads
                            Panel panel = new Panel();
                            panel.ID = "EventPanel" + i.ToString();//Remember must put the for loop int in here
                            panel.HorizontalAlign = HorizontalAlign.Left;

                            //panel Body
                            panel.Controls.Add(new LiteralControl("<div class='panel-body'>"));
                            panel.Controls.Add(new LiteralControl("<div class='row'>"));

                            //Buttons
                            panel.Controls.Add(new LiteralControl("<div class='col-md-6'>"));

                            var img = new HtmlGenericControl("img");
                            img.Attributes.Add("class", "img-responsive user-photo");

                            //string base64String = Convert.ToBase64String(ImgDataMain, 0, ImgDataMain.Length);
                            img.Attributes.Add("src", imgData);
                            panel.Controls.Add(img);

                            panel.Controls.Add(new LiteralControl("</div>"));//col-md-3 div

                            panel.Controls.Add(new LiteralControl("<div class='col-md-6'>"));

                            //Design Name
                            panel.Controls.Add(new LiteralControl("<div class='row'>"));
                            panel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                            HtmlGenericControl label1 = new HtmlGenericControl("label");
                            label1.Attributes.Add("class", "col-lg-6 control-label");
                            label1.InnerText = "Design Name:";
                            panel.Controls.Add(label1);
                            label1.Controls.Add(new LiteralControl("</label>"));
                            Label l = new Label();
                            l.CssClass = "col-lg-6 control-label";
                            l.Text = HttpUtility.HtmlEncode(title);
                            panel.Controls.Add(l);
                            panel.Controls.Add(new LiteralControl("</div>"));//form group div
                            panel.Controls.Add(new LiteralControl("</div>"));//row div

                            panel.Controls.Add(new LiteralControl("<div class='row'>"));

                            //Cancel Event button
                            panel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                            Button b = new Button();
                            b.Text = "Remove Design";
                            b.CssClass = "btn btn-primary";
                            b.Click += new EventHandler(b_Click);
                            b.CommandArgument = GalleryID.ToString();
                            panel.Controls.Add(b);
                            panel.Controls.Add(new LiteralControl("</div>"));//form group div

                            //Edit Button
                            panel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                            Button b2 = new Button();
                            b2.Text = "Edit Details";
                            b2.CssClass = "btn btn-primary";
                            b2.Click += new EventHandler(editdetails_click);
                            b2.CommandArgument = GalleryID.ToString();
                            panel.Controls.Add(b2);
                            panel.Controls.Add(new LiteralControl("</div>"));//form group div

                            panel.Controls.Add(new LiteralControl("</div>"));//Row div

                            panel.Controls.Add(new LiteralControl("</div>"));//col-md-3 div

                            panel.Controls.Add(new LiteralControl("</div>"));//row div
                            panel.Controls.Add(new LiteralControl("</div>"));//panel body div
                            panel.Controls.Add(new LiteralControl("<hr />"));
                            PLH.Controls.Add(panel);
                        }
                    }
                }

                //SqlCommand cmd7 = new SqlCommand();
                //cmd7.CommandText = "SELECT COUNT(*) FROM [dbo].[UserProdLink] WHERE [UserID]= @UserID;";
                //cmd7.Parameters.AddWithValue("UserID", userid);
                //cmd7.Connection = connection;
                //connection.Open();
                //cmd7.ExecuteNonQuery();

                //reader = cmd7.ExecuteReader();
                //while (reader.Read())
                //{
                //    noOfUserProdLink = reader.GetInt32(0);
                //}
                //connection.Close();

                //SqlCommand cmd8 = new SqlCommand();
                //cmd8.CommandText = "SELECT [FileUploadID] FROM [dbo].[Gallery] WHERE [UserID]= @UserID;";
                //cmd8.Parameters.AddWithValue("UserID", userid);
                //cmd8.Connection = connection;
                //connection.Open();
                //cmd8.ExecuteNonQuery();

                //reader = cmd8.ExecuteReader();
                //while (reader.Read())
                //{
                //    FileUploadIDDownloadList.Add(reader.GetInt32(0));
                //}
                //connection.Close();

                //for (int y = 0; y < noOfUserProdLink; y++)
                //{
                //    int FileUploadDownloadID = FileUploadIDList[y];

                //    SqlCommand cmd9 = new SqlCommand();
                //    cmd9.CommandText = "SELECT [ImgData],[FileUploadSecretID],[MediaName] FROM [dbo].[FileUpload] WHERE [FileUploadID]= @FileUploadID AND [UserID] = @UserID;";
                //    cmd9.Parameters.AddWithValue("FileUploadID", FileUploadDownloadID);
                //    cmd9.Parameters.AddWithValue("UserID", userid);
                //    cmd9.Connection = connection;
                //    connection.Open();
                //    cmd9.ExecuteNonQuery();

                //    reader = cmd9.ExecuteReader();
                //    while (reader.Read())
                //    {
                //        //Image
                //        long length = reader.GetBytes(0, 0, null, 0, 0);
                //        Byte[] buffer = new Byte[length];
                //        reader.GetBytes(0, 0, buffer, 0, (int)length);
                //        ImgDataDownload = buffer;

                //        fileuploadsecretDownloadID = reader.GetInt32(1);
                //        mediaNameDownload = reader.GetString(2);
                //    }
                //    connection.Close();

                //    SqlCommand cmd10 = new SqlCommand();
                //    cmd10.CommandText = "SELECT [EmbeddedSecretText],[EmbeddedSecretTextKey],[SecretKey] FROM [dbo].[FileUploadSecret] WHERE [FileUploadSecretID]= @FileUploadSecretID;";
                //    cmd10.Parameters.AddWithValue("FileUploadSecretID", fileuploadsecretDownloadID);
                //    cmd10.Connection = connection;
                //    connection.Open();
                //    cmd10.ExecuteNonQuery();

                //    reader = cmd10.ExecuteReader();
                //    while (reader.Read())
                //    {
                //        embeddedsecrettextDownload = reader.GetString(0);
                //        embeddedsecrettextkeyDownload = reader.GetString(1);
                //        DecryptDataKeyDownload = reader.GetString(2);
                //    }
                //    connection.Close();

                //    embeddedsecrettextDownload = Cryptography.DecryptOfData(embeddedsecrettextDownload, DecryptDataKeyDownload);
                //    embeddedsecrettextkeyDownload = Cryptography.DecryptOfData(embeddedsecrettextkeyDownload, DecryptDataKeyDownload);

                //    if (ImgDataDownload != null)
                //    {
                //        System.Drawing.Image picDownload = byteArrayToImage(ImgDataDownload);
                //        Bitmap bmpDownload = new Bitmap(picDownload);

                //        //Extraction of secret text
                //        string ExtractedTextDownload = ImageStenography.extractText(bmpDownload);

                //        //Decrytion of secret text
                //        string plainExtractedTextDownload = ImageStenography.DecryptImageSecretTextAesIntoString(ExtractedTextDownload, embeddedsecrettextkeyDownload);
                //        string originalPlainTextDownload = ImageStenography.DecryptImageSecretTextAesIntoString(embeddedsecrettextDownload, embeddedsecrettextkeyDownload);

                //        if (originalPlainTextDownload == plainExtractedTextDownload)
                //        {
                //            string imgDataDownload;
                //            using (MemoryStream ms = new MemoryStream())
                //            {
                //                bmpDownload.Save(ms, ImageFormat.Png);
                //                byte[] byteImageMain = ms.ToArray();
                //                string base64StringImageDownload = Convert.ToBase64String(byteImageMain);
                //                imgDataDownload = "data:image/png;base64," + base64StringImageDownload;
                //            }

                //            //My Downloads
                //            Panel panel2 = new Panel();
                //            panel2.ID = "EventPanel" + y.ToString();//Remember must put the for loop int in here
                //            panel2.HorizontalAlign = HorizontalAlign.Left;

                //            //panel Body
                //            panel2.Controls.Add(new LiteralControl("<div class='panel-body'>"));
                //            panel2.Controls.Add(new LiteralControl("<div class='row'>"));

                //            //Buttons
                //            panel2.Controls.Add(new LiteralControl("<div class='col-md-6'>"));

                //            var imgDownloads = new HtmlGenericControl("img");
                //            imgDownloads.Attributes.Add("class", "img-responsive user-photo");
                //            imgDownloads.Attributes.Add("src", imgDataDownload);
                //            panel2.Controls.Add(imgDownloads);

                //            panel2.Controls.Add(new LiteralControl("</div>"));//col-md-3 div

                //            panel2.Controls.Add(new LiteralControl("<div class='col-md-6'>"));

                //            //Design Name
                //            panel2.Controls.Add(new LiteralControl("<div class='row'>"));
                //            panel2.Controls.Add(new LiteralControl("<div class='form-group'>"));
                //            HtmlGenericControl DownloadName = new HtmlGenericControl("label");
                //            DownloadName.Attributes.Add("class", "col-lg-6 control-label");
                //            DownloadName.InnerText = "Design Name:";
                //            panel2.Controls.Add(DownloadName);
                //            DownloadName.Controls.Add(new LiteralControl("</label>"));
                //            Label x = new Label();
                //            x.CssClass = "col-lg-6 control-label";
                //            x.Text = HttpUtility.HtmlEncode(title);
                //            panel2.Controls.Add(x);
                //            panel2.Controls.Add(new LiteralControl("</div>"));//form group div
                //            panel2.Controls.Add(new LiteralControl("</div>"));//row div

                //            panel2.Controls.Add(new LiteralControl("<div class='row'>"));

                //            //Download Event button
                //            panel2.Controls.Add(new LiteralControl("<div class='form-group'>"));
                //            Button download = new Button();
                //            download.Text = "Download";
                //            download.CssClass = "btn btn-primary";
                //            string combinedText = ImgDataDownload.ToString() + ";" + mediaNameDownload;
                //            download.Click += new EventHandler(download_Click);
                //            download.CommandArgument = combinedText;
                //            panel2.Controls.Add(download);
                //            panel2.Controls.Add(new LiteralControl("</div>"));//form group div

                //            panel2.Controls.Add(new LiteralControl("</div>"));//Row div

                //            panel2.Controls.Add(new LiteralControl("</div>"));//col-md-3 div

                //            panel2.Controls.Add(new LiteralControl("</div>"));//row div
                //            panel2.Controls.Add(new LiteralControl("</div>"));//panel body div
                //            panel2.Controls.Add(new LiteralControl("<hr />"));
                //            PLH2.Controls.Add(panel2);
                //        }
                //    }
                //}
            }
        }

        private void editdetails_click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int id = Convert.ToInt32(button.CommandArgument);

            Session["GalleryID"] = id;
            Response.Redirect("MyUploadsEdit.aspx");
        }

        private void download_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string data = button.CommandArgument;

            string[] values = data.Split(';');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
            }
            string compressedFileName = "ASPJ-FileFinder";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + compressedFileName + ".zip");
            Response.ContentType = "application/zip";
            using (var zipStream = new ZipOutputStream(Response.OutputStream))
            {
                byte[] fileBytes = Encoding.ASCII.GetBytes(values[0]);
                string filename = values[1];
                var fileEntry = new ZipEntry(filename)
                {
                    Size = fileBytes.Length
                };
                zipStream.PutNextEntry(fileEntry);
                zipStream.Write(fileBytes, 0, fileBytes.Length);

                zipStream.Flush();
                zipStream.Close();
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
            Response.Redirect("MyUploads.aspx");
        }

        protected void CreateUpload_Click(object sender, EventArgs e)
        {
            Response.Redirect("SharingCreate.aspx");
        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
    }
}