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
        private string gid = "24";

        //Gallery Database var
        private string title;
        private int amount;
        private string desc;

        //UploadedImage [Main]
        private byte[] ImgMain;

        //UploadedImage Sec
        private byte[] ImgSec;

        //FileUploadMain Database var
        private int fileuploadID;
        private string filetypeMain;
        //private string filepathMain;
        
        private string filesizeMain;
        private string filenameMain;
        private int fileuploadsecretID;
        private string embeddedsecrettextMain;
        private string embeddedsecrettextkeyMain;

        //FileUploadSecondary Database  var
        private int fileuploadsecondaryID;
        private string filetypeSec;
        //private string filepathSec;
        
        private int fileuploadsecondarysecretID;
        private string embeddedsecrettextSec;
        private string embeddedsecrettextkeySec;
        public string bg;

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
                    amount = reader.GetInt32(3);
                    desc = reader.GetString(4);
                }
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "SELECT [ImgData],[FileType],[FileSize],[MediaName],[FileUploadSecretID] FROM [dbo].[FileUpload] WHERE [FileUploadID]= @FileUploadID AND [UserID] = @UserID;";
                cmd2.Parameters.Add("@FileUploadID", SqlDbType.Int).Value = fileuploadID;
                cmd2.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userid;
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteNonQuery();

                reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    //Image
                    long length = reader.GetBytes(0, 0, null, 0, 0);
                    Byte[] buffer = new Byte[length];
                    reader.GetBytes(0, 0, buffer, 0, (int)length);
                    ImgMain = buffer;

                    filetypeMain = reader.GetString(1);
                    filesizeMain = reader.GetString(2);
                    filenameMain = reader.GetString(3);
                    fileuploadsecretID = reader.GetInt32(4);
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
                cmd4.CommandText = "SELECT [EmbeddedSecretText],[EmbeddedSecretTextKey] FROM [dbo].[FileUploadSecret] WHERE [FileUploadSecretID]= @FileUploadSecretID;";
                cmd4.Parameters.Add("@FileUploadSecretID", SqlDbType.Int).Value = fileuploadsecretID;
                cmd4.Connection = connection;
                connection.Open();
                cmd4.ExecuteNonQuery();

                reader = cmd4.ExecuteReader();
                while (reader.Read())
                {
                    embeddedsecrettextMain = reader.GetString(0);
                    embeddedsecrettextkeyMain = reader.GetString(1);
                }
                connection.Close();

                SqlCommand cmd5 = new SqlCommand();
                cmd5.CommandText = "SELECT [EmbeddedSecretText],[EmbeddedSecretTextKey] FROM [dbo].[FileUploadSecondarySecret] WHERE [FileUploadSecondarySecretID]= @FileUploadSecondarySecretID;";
                cmd5.Parameters.Add("@FileUploadSecondarySecretID", SqlDbType.Int).Value = fileuploadsecondarysecretID;
                cmd5.Connection = connection;
                connection.Open();
                cmd5.ExecuteNonQuery();

                reader = cmd5.ExecuteReader();
                while (reader.Read())
                {
                    embeddedsecrettextSec = reader.GetString(0);
                    embeddedsecrettextkeySec = reader.GetString(1);
                }
                connection.Close();


                //Review Database
                int noOfReview = 0;
                List<int> ReviewIDList = new List<int>();
                SqlCommand cmd6 = new SqlCommand();
                cmd6.CommandText = "SELECT COUNT(*) FROM [dbo].[Review] WHERE [GalleryID] = @GalleryID;";
                cmd6.Parameters.Add("@GalleryID", SqlDbType.Int).Value = gid;
                cmd6.Connection = connection;
                connection.Open();
                cmd6.ExecuteNonQuery();

                reader = cmd6.ExecuteReader();
                while (reader.Read())
                {
                    noOfReview = reader.GetInt32(0);
                }
                connection.Close();

                SqlCommand cmd7 = new SqlCommand();
                cmd7.CommandText = "SELECT [ReviewID] FROM [dbo].[Review] WHERE [GalleryID] = @GalleryID;";
                cmd7.Parameters.Add("@GalleryID", SqlDbType.Int).Value = gid;
                cmd7.Connection = connection;
                connection.Open();
                cmd7.ExecuteNonQuery();

                reader = cmd7.ExecuteReader();
                while (reader.Read())
                {
                    ReviewIDList.Add(reader.GetInt32(0));
                }
                connection.Close();

                for (int i = 0; i < noOfReview; i++)
                {
                    int ReviewID = ReviewIDList[i];

                    SqlCommand cmd8 = new SqlCommand();
                    cmd8.CommandText = "SELECT [Content],[UserID],[TimeStamp] FROM [dbo].[Review] WHERE [GalleryID]= @GalleryID AND [ReviewID] = @ReviewID;";
                    cmd8.Parameters.Add("@GalleryID", SqlDbType.Int).Value = gid;
                    cmd8.Parameters.Add("@ReviewID", SqlDbType.Int).Value = ReviewID;
                    cmd8.Connection = connection;
                    connection.Open();
                    cmd8.ExecuteNonQuery();

                    reader = cmd8.ExecuteReader();
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
                    panel.Controls.Add(new LiteralControl("<div class='row'>"));
                    panel.Controls.Add(new LiteralControl("<div class=col-sm-1>"));
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
                    //Label1.Attributes.Add("class", "media-heading");
                    Label1.InnerText = "Monster";
                    panel.Controls.Add(Label1);
                    HtmlGenericControl Label2 = new HtmlGenericControl("span");
                    Label2.InnerText = "posted "+ ReviewTimeStamp;
                    Label2.Attributes.Add("class","text-muted");
                    panel.Controls.Add(Label2);
                    panel.Controls.Add(new LiteralControl("</div>"));// <!-- /panel-heading -->

                    panel.Controls.Add(new LiteralControl("<div class='panel-body'>"));
                    HtmlGenericControl Label3 = new HtmlGenericControl("label");
                    Label3.InnerText = ReviewContent;
                    panel.Controls.Add(Label3);

                    panel.Controls.Add(new LiteralControl("</div>"));//<!--/panel body-->
                    panel.Controls.Add(new LiteralControl("</div>"));//<!--/panel panel default-->
                    panel.Controls.Add(new LiteralControl("</div>"));//<!--/col-sm-5-->
                    panel.Controls.Add(new LiteralControl("</div>"));//<!--/row div-->

                    PlaceHolderReview.Controls.Add(panel);
                }

                DesignTitleLabel.Text = title;
                NameLabel.Text = "Blah Blah need to change";
                Titlelabel2.Text = title;
                PriceLabel.Text = "S$" + amount.ToString();
                DescriptionLabel.Text = desc;
                Label1.Text = title;
                Label2.Text = filesizeMain + "KB";

                //Image 
                if (ImgMain!=null && ImgSec!=null)
                {
                    //imageMain = File.ReadAllBytes(filepathMain);
                    //imageSec = File.ReadAllBytes(filepathSec);
                    System.Drawing.Image picMain = byteArrayToImage(ImgMain);
                    System.Drawing.Image picSec = byteArrayToImage(ImgSec);
                    Bitmap bmpMain = new Bitmap(picMain);
                    Bitmap bmpSec = new Bitmap(picSec);

                    //Extraction of secret text
                    string ExtractedTextMain = Cryptography.extractText(bmpMain);
                    string ExtractedTextSec = Cryptography.extractText(bmpSec);

                    //Decrytion of secret text
                    string plainExtractedTextMain = DecryptImageAesIntoString(ExtractedTextMain, embeddedsecrettextkeyMain);
                    string plainExtractedTextSec = DecryptImageAesIntoString(ExtractedTextSec, embeddedsecrettextkeySec);
                    string originalPlainTextMain = DecryptImageAesIntoString(embeddedsecrettextMain, embeddedsecrettextkeyMain);
                    string originalPlainTextSec = DecryptImageAesIntoString(embeddedsecrettextSec, embeddedsecrettextkeySec);

                    if (originalPlainTextMain == plainExtractedTextMain && originalPlainTextSec == plainExtractedTextSec)
                    {
                        //Displaying of sec Image
                        //string blank = @"C:\Users\User\Source\Repos\F2\F2\Images\Blank.gif";
                        using (MemoryStream ms = new MemoryStream())
                        {
                            //Graphics gra = Graphics.FromImage(bmpSec);
                            //Bitmap blankImg = new Bitmap(blank);
                            //blankImg = new Bitmap(blankImg, 1000, 1000);
                            //blankImg.MakeTransparent();
                            //gra.DrawImage(blankImg, new Point(0, 0));


                            bmpSec.Save(ms, ImageFormat.Png);
                            byte[] byteImageSec = ms.ToArray();
                            string base64StringImageSec = Convert.ToBase64String(byteImageSec);
                            SecImage.ImageUrl = "data:image/png;base64," + base64StringImageSec;
                            //SecImage.Style["background:url"] = "data:image/png;base64," + base64StringImageSec;
                            //SecImage.Style.Add("background-image", "" + returncolor + "");
                        }
                    }
                }

            }
        }
        //Decryption for the secet text in image
        public static string DecryptImageAesIntoString(string cipherText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create the streams used for decryption.
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }
        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
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
            panel.Controls.Add(new LiteralControl("<div class=col-sm-1>"));
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