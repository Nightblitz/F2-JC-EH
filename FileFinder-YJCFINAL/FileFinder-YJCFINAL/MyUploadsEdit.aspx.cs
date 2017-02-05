using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace FileFinder_YJCFINAL
{
    public partial class MyUploadsEdit : System.Web.UI.Page
    {
        private int GalleryID;
        private string userid = "123";

        //Gallery DB var
        private string DesignName;
        private string Cost;
        private string Description;
        private int CategoryID;

        //GalleryDB var
        private string DecryptDataKey;

        protected void Page_Load(object sender, EventArgs e)
        {
            GalleryID = (int)Session["GalleryID"];

            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["F2DB"].ConnectionString))
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT [DesignName],[Cost],[Description],[CategoryID] FROM [dbo].[Gallery] WHERE [GalleryID]= @GalleryID AND [UserID] = @UserID;";
                cmd.Parameters.Add("@GalleryID", SqlDbType.Int).Value = GalleryID;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userid;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DesignName = reader.GetString(0);
                    Cost = reader.GetString(1);
                    Description = reader.GetString(2);
                    CategoryID = reader.GetInt32(3);
                }
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "SELECT [SecretKey] FROM [dbo].[GallerySecret] WHERE [GalleryID]= @GalleryID;";
                cmd2.Parameters.Add("@GalleryID", SqlDbType.Int).Value = GalleryID;
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteNonQuery();

                reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    DecryptDataKey = reader.GetString(0);
                }
                connection.Close();

                //Decrypt Data
                DesignName = Cryptography.DecryptOfData(DesignName, DecryptDataKey);
                Cost = Cryptography.DecryptOfData(Cost, DecryptDataKey);
                Description = Cryptography.DecryptOfData(Description, DecryptDataKey);

                //Assign Data 
                TitleTextBox.Text= HttpUtility.HtmlEncode(DesignName);
                DescriptionTextBox.Text = HttpUtility.HtmlEncode(Description);
                CostTextBox.Text = HttpUtility.HtmlEncode(Cost);
                CategoryDropDownList.SelectedIndex = CategoryID;

            }
        }

        protected void DoneBtn_Click(object sender, EventArgs e)
        {
            DesignName = TitleTextBox.Text;
            Description = DescriptionTextBox.Text;
            Cost = CostTextBox.Text;
            CategoryID = CategoryDropDownList.SelectedIndex;
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["F2DB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE [dbo].[Gallery] SET [DesignName]= @DesignName , [Description] = @Description, [Cost]=@Cost, [CategoryID] = @CategoryID WHERE [GalleryID] = @GalleryID";
                cmd.Parameters.Add("@DesignName", SqlDbType.NVarChar).Value = DesignName;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
                cmd.Parameters.Add("@Cost", SqlDbType.NVarChar).Value = Cost;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = CategoryID;
                cmd.Parameters.Add("@GallerID", SqlDbType.Int).Value = GalleryID;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                Response.Redirect("MyUploads.aspx");

            }
        }
    }
}