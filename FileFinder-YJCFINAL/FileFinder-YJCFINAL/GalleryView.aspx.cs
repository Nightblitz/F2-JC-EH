using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace FileFinder_YJCFINAL
{
    public partial class GalleryView : System.Web.UI.Page
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("jasdh7834y8hfeur73rsharks214");
        //temp
        private string userid = "123";
        private string gid = "45";

        //Gallery Database var
        private string title;
        private string amount;
        private string desc;
        private string DecryptDataKeyGallery;

        //UploadedImage [Main]
        //private byte[] ImgMain;

        //UploadedImage Sec
        private byte[] ImgSec;

        //FileUploadMain Database var
        private int fileuploadID;
        //private string filetypeMain;
        //private string filepathMain;
        
        private string filesizeMain;
        private string filenameMain;
        private int fileuploadsecretID;
        //private string embeddedsecrettextMain;
        //private string embeddedsecrettextkeyMain;
        private string DecryptDataKeyMain;

        //FileUploadSecondary Database  var
        private int fileuploadsecondaryID;
        private string filetypeSec;
        //private string filepathSec;
        
        private int fileuploadsecondarysecretID;
        private string embeddedsecrettextSec;
        private string embeddedsecrettextkeySec;
        public string bg;
        private string DecryptDataKeySec;

        //Review Database
        private string ReviewContent;
        private string ReviewUserID;
        private string ReviewTimeStamp;



        protected void Page_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["F2DB"].ConnectionString))
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT [DesignName],[FileUploadID],[FileUploadSecondaryID],[Cost],[Description] FROM [dbo].[Gallery] WHERE [GalleryID]= @GalleryID AND [UserID] = @UserID;";
                cmd.Parameters.Add("@GalleryID", SqlDbType.Int).Value = gid;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userid;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    title = reader.GetString(0);
                    fileuploadID = reader.GetInt32(1);
                    fileuploadsecondaryID = reader.GetInt32(2);
                    amount = reader.GetString(3);
                    desc = reader.GetString(4);
                }
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "SELECT [FileSize],[MediaName],[FileUploadSecretID] FROM [dbo].[FileUpload] WHERE [FileUploadID]= @FileUploadID AND [UserID] = @UserID;";
                cmd2.Parameters.Add("@FileUploadID", SqlDbType.Int).Value = fileuploadID;
                cmd2.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userid;
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteNonQuery();

                reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    //Image
                    //long length = reader.GetBytes(0, 0, null, 0, 0);
                    //Byte[] buffer = new Byte[length];
                    //reader.GetBytes(0, 0, buffer, 0, (int)length);
                    //ImgMain = buffer;

                    //filetypeMain = reader.GetString(1);
                    filesizeMain = reader.GetString(0);
                    filenameMain = reader.GetString(1);
                    fileuploadsecretID = reader.GetInt32(2);
                }
                connection.Close();

                SqlCommand cmd3 = new SqlCommand();
                cmd3.CommandText = "SELECT [ImgData],[FileType],[FileUploadSecondarySecretID] FROM [dbo].[FileUploadSecondary] WHERE [FileUploadSecondaryID]= @FileUploadSecondaryID AND [UserID] = @UserID;";
                cmd3.Parameters.Add("@FileUploadSecondaryID", SqlDbType.Int).Value = fileuploadsecondaryID;
                cmd3.Parameters.Add("@UserID", SqlDbType.Int).Value = userid;
                cmd3.Connection = connection;
                connection.Open();
                cmd3.ExecuteNonQuery();

                reader = cmd3.ExecuteReader();
                while (reader.Read())
                {
                    //Image
                    long length = reader.GetBytes(0, 0, null, 0, 0);
                    Byte[] buffer = new Byte[length];
                    reader.GetBytes(0, 0, buffer, 0, (int)length);
                    ImgSec = buffer;

                    filetypeSec = reader.GetString(1);
                    fileuploadsecondarysecretID = reader.GetInt32(2);
                }
                connection.Close();

                SqlCommand cmd4 = new SqlCommand();
                cmd4.CommandText = "SELECT [SecretKey] FROM [dbo].[FileUploadSecret] WHERE [FileUploadSecretID]= @FileUploadSecretID;";
                cmd4.Parameters.Add("@FileUploadSecretID", SqlDbType.Int).Value = fileuploadsecretID;
                cmd4.Connection = connection;
                connection.Open();
                cmd4.ExecuteNonQuery();

                reader = cmd4.ExecuteReader();
                while (reader.Read())
                {
                    DecryptDataKeyMain = reader.GetString(0);
                }
                connection.Close();

                SqlCommand cmd5 = new SqlCommand();
                cmd5.CommandText = "SELECT [EmbeddedSecretText],[EmbeddedSecretTextKey],[SecretKey] FROM [dbo].[FileUploadSecondarySecret] WHERE [FileUploadSecondarySecretID]= @FileUploadSecondarySecretID;";
                cmd5.Parameters.Add("@FileUploadSecondarySecretID", SqlDbType.Int).Value = fileuploadsecondarysecretID;
                cmd5.Connection = connection;
                connection.Open();
                cmd5.ExecuteNonQuery();

                reader = cmd5.ExecuteReader();
                while (reader.Read())
                {
                    embeddedsecrettextSec = reader.GetString(0);
                    embeddedsecrettextkeySec = reader.GetString(1);
                    DecryptDataKeySec = reader.GetString(2);
                }
                connection.Close();

                SqlCommand cmd6 = new SqlCommand();
                cmd6.CommandText = "SELECT [SecretKey] FROM [dbo].[GallerySecret] WHERE [GalleryID]= @GalleryID;";
                cmd6.Parameters.Add("@GalleryID", SqlDbType.Int).Value = gid;
                cmd6.Connection = connection;
                connection.Open();
                cmd6.ExecuteNonQuery();

                reader = cmd6.ExecuteReader();
                while (reader.Read())
                {
                    DecryptDataKeyGallery = reader.GetString(0);
                }
                connection.Close();


                //Review Database
                int noOfReview = 0;
                List<int> ReviewIDList = new List<int>();
                SqlCommand cmd7 = new SqlCommand();
                cmd7.CommandText = "SELECT COUNT(*) FROM [dbo].[Review] WHERE [GalleryID] = @GalleryID;";
                cmd7.Parameters.Add("@GalleryID", SqlDbType.Int).Value = gid;
                cmd7.Connection = connection;
                connection.Open();
                cmd7.ExecuteNonQuery();

                reader = cmd7.ExecuteReader();
                while (reader.Read())
                {
                    noOfReview = reader.GetInt32(0);
                }
                connection.Close();

                SqlCommand cmd8 = new SqlCommand();
                cmd8.CommandText = "SELECT [ReviewID] FROM [dbo].[Review] WHERE [GalleryID] = @GalleryID;";
                cmd8.Parameters.Add("@GalleryID", SqlDbType.Int).Value = gid;
                cmd8.Connection = connection;
                connection.Open();
                cmd8.ExecuteNonQuery();

                reader = cmd8.ExecuteReader();
                while (reader.Read())
                {
                    ReviewIDList.Add(reader.GetInt32(0));
                }
                connection.Close();

                for (int i = 0; i < noOfReview; i++)
                {
                    int ReviewID = ReviewIDList[i];

                    SqlCommand cmd9 = new SqlCommand();
                    cmd9.CommandText = "SELECT [Content],[UserID],[TimeStamp] FROM [dbo].[Review] WHERE [GalleryID]= @GalleryID AND [ReviewID] = @ReviewID;";
                    cmd9.Parameters.Add("@GalleryID", SqlDbType.Int).Value = gid;
                    cmd9.Parameters.Add("@ReviewID", SqlDbType.Int).Value = ReviewID;
                    cmd9.Connection = connection;
                    connection.Open();
                    cmd8.ExecuteNonQuery();

                    reader = cmd9.ExecuteReader();
                    while (reader.Read())
                    {
                        ReviewContent = reader.GetString(0);
                        ReviewUserID = reader.GetString(1);
                        ReviewTimeStamp = reader.GetString(2);
                    }
                    connection.Close();

                    Panel panel = new Panel();
                    panel.ID = "ReviewPanel" + i.ToString();//Remember must put the for loop int in here
                    panel.HorizontalAlign = HorizontalAlign.Left;

                    //Head of review 
                    panel.Controls.Add(new LiteralControl("<div class='container'>"));
                    panel.Controls.Add(new LiteralControl("<div class=col-sm-1>"));
                    panel.Controls.Add(new LiteralControl("<div class='thumbnail'>"));
                    var img = new HtmlGenericControl("img");
                    img.Attributes.Add("class", "img-responsive user-photo");
                    img.Attributes.Add("src", "http://placehold.it/64x64");
                    panel.Controls.Add(img);
                    panel.Controls.Add(new LiteralControl("</div>"));// <!--/thumbnail div-->
                    panel.Controls.Add(new LiteralControl("</div>")); //<!--/col-sm-1 div-->

                    //panel Body of review
                    panel.Controls.Add(new LiteralControl("<div class='col-sm-5'>"));
                    panel.Controls.Add(new LiteralControl("<div class='panel panel-info'>"));
                    panel.Controls.Add(new LiteralControl("<div class='panel-heading'>"));
                    HtmlGenericControl Label1 = new HtmlGenericControl("strong");
                    //Label1.Attributes.Add("class", "media-heading");
                    Label1.InnerText = "Monster";
                    panel.Controls.Add(Label1);
                    HtmlGenericControl Label2 = new HtmlGenericControl("span");
                    Label2.InnerText = HttpUtility.HtmlEncode(" posted " + ReviewTimeStamp);
                    Label2.Attributes.Add("class","text-muted");
                    panel.Controls.Add(Label2);
                    panel.Controls.Add(new LiteralControl("</div>"));// <!-- /panel-heading -->

                    panel.Controls.Add(new LiteralControl("<div class='panel-body'>"));
                    HtmlGenericControl Label3 = new HtmlGenericControl("label");
                    Label3.InnerText = HttpUtility.HtmlEncode(ReviewContent);
                    panel.Controls.Add(Label3);

                    panel.Controls.Add(new LiteralControl("</div>"));//<!--/panel body-->
                    panel.Controls.Add(new LiteralControl("</div>"));//<!--/panel panel default-->
                    panel.Controls.Add(new LiteralControl("</div>"));//<!--/col-sm-5-->
                    panel.Controls.Add(new LiteralControl("</div>"));//<!--/row div-->

                    PlaceHolderReview.Controls.Add(panel);
                }
                //Decryption of DATA
                title = Cryptography.DecryptOfData(title, DecryptDataKeyGallery);
                amount = Cryptography.DecryptOfData(amount, DecryptDataKeyGallery);
                desc = Cryptography.DecryptOfData(desc, DecryptDataKeyGallery);

                filesizeMain = Cryptography.DecryptOfData(filesizeMain,DecryptDataKeyMain);
                filenameMain = Cryptography.DecryptOfData(filenameMain, DecryptDataKeyMain);

                filetypeSec = Cryptography.DecryptOfData(filetypeSec,DecryptDataKeySec);
                embeddedsecrettextSec = Cryptography.DecryptOfData(embeddedsecrettextSec, DecryptDataKeySec);
                embeddedsecrettextkeySec = Cryptography.DecryptOfData(embeddedsecrettextkeySec, DecryptDataKeySec);

                DesignTitleLabel.Text = HttpUtility.HtmlEncode(title);
                NameLabel.Text = "Blah Blah need to change";
                Titlelabel2.Text = HttpUtility.HtmlEncode(title);
                PriceLabel.Text = HttpUtility.HtmlEncode("S$" + amount);
                DescriptionLabel.Text = HttpUtility.HtmlEncode(desc);
                Label1.Text = HttpUtility.HtmlEncode(title);
                Label2.Text = HttpUtility.HtmlEncode(filesizeMain + "KB");

                //Image 
                if (ImgSec!=null)
                {
                   
                    //System.Drawing.Image picMain = ImageStenography.byteArrayToImage(ImgMain);
                    System.Drawing.Image picSec = ImageStenography.byteArrayToImage(ImgSec);
                    //Bitmap bmpMain = new Bitmap(picMain);
                    Bitmap bmpSec = new Bitmap(picSec);

                    //Extraction of secret text
                    //string ExtractedTextMain = ImageStenography.extractText(bmpMain);
                    string ExtractedTextSec = ImageStenography.extractText(bmpSec);

                    //Decrytion of secret text
                    //string plainExtractedTextMain = ImageStenography.DecryptImageAesIntoString(ExtractedTextMain, embeddedsecrettextkeyMain);
                    string plainExtractedTextSec = ImageStenography.DecryptImageSecretTextAesIntoString(ExtractedTextSec, embeddedsecrettextkeySec);
                    //string originalPlainTextMain = ImageStenography.DecryptImageAesIntoString(embeddedsecrettextMain, embeddedsecrettextkeyMain);
                    string originalPlainTextSec = ImageStenography.DecryptImageSecretTextAesIntoString(embeddedsecrettextSec, embeddedsecrettextkeySec);

                    if (originalPlainTextSec == plainExtractedTextSec)
                    {

                        using (MemoryStream ms = new MemoryStream())
                        {
                            bmpSec.Save(ms, ImageFormat.Png);
                            byte[] byteImageSec = ms.ToArray();
                            string base64StringImageSec = Convert.ToBase64String(byteImageSec);
                            SecImage.ImageUrl = "data:image/png;base64," + base64StringImageSec;
                        }
                    }
                }

            }
        }
  

        protected void Download_PurcahseBtn_Click(object sender, EventArgs e)
        {

        }

        protected void PostBtn_Click(object sender, EventArgs e)
        {
            Panel panel = new Panel();
            panel.HorizontalAlign = HorizontalAlign.Left;

            //Head of review 
            panel.Controls.Add(new LiteralControl("<div class='row'>"));
            panel.Controls.Add(new LiteralControl("<div class=col-sm-2>"));
            panel.Controls.Add(new LiteralControl("<div class='thumbnail'>"));
            var img = new HtmlGenericControl("img");
            img.Attributes.Add("class", "img-responsive user-photo");
            img.Attributes.Add("src", "http://placehold.it/64x64");
            panel.Controls.Add(img);
            panel.Controls.Add(new LiteralControl("</div>"));// <!--/thumbnail div-->
            panel.Controls.Add(new LiteralControl("</div>")); //<!--/col-sm-1 div-->

            //panel Body of review
            panel.Controls.Add(new LiteralControl("<div class='col-sm-6'>"));
            panel.Controls.Add(new LiteralControl("<div class='panel panel-default'>"));
            panel.Controls.Add(new LiteralControl("<div class='panel-heading'>"));

            HtmlGenericControl Label1 = new HtmlGenericControl("strong");
            Label1.InnerText = "Monster";
            panel.Controls.Add(Label1);

            HtmlGenericControl Label2 = new HtmlGenericControl("span");
            Label2.InnerText = "posted " + DateTime.Now.ToString();
            Label2.Attributes.Add("class", "text-muted");
            panel.Controls.Add(Label2);

            panel.Controls.Add(new LiteralControl("</div>"));// <!-- /panel-heading -->

            panel.Controls.Add(new LiteralControl("<div class='panel-body'>"));
            HtmlGenericControl Label3 = new HtmlGenericControl("label");
            Label3.InnerText = ReviewTextArea.Text;
            panel.Controls.Add(Label3);

            panel.Controls.Add(new LiteralControl("</div>"));//<!--/panel body-->
            panel.Controls.Add(new LiteralControl("</div>"));//<!--/panel panel default-->
            panel.Controls.Add(new LiteralControl("</div>"));//<!--/col-sm-5-->
            panel.Controls.Add(new LiteralControl("</div>"));//<!--/row div-->

            PlaceHolderReview.Controls.Add(panel);
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["F2DB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO [dbo].[Review] ([Content],[UserID],[TimeStamp],[GalleryID]) VALUES (@Content,@UserID,@TimeStamp,@GalleryID);";
                cmd.Parameters.Add("@Content", SqlDbType.NVarChar).Value = ReviewTextArea.Text;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userid;
                cmd.Parameters.Add("@TimeStamp", SqlDbType.NVarChar).Value = DateTime.Now.ToString();
                cmd.Parameters.Add("@GalleryID", SqlDbType.NVarChar).Value = gid;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

            }
        }
    }
}