using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantPointOfSell.Services
{
   public static class dbConnection
    {
        public static string BuildEntityConnection()
        {
            IDbConnection connection = new SqlConnection(connectioninfo());
            var dbName = connection.Database;
            DbConnection connection1 = new SqlConnection(connectioninfo());
            var server = connection1.DataSource;
            var originalConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RPOSDBEntities"].ConnectionString;
            var ecsBuilder = new EntityConnectionStringBuilder(originalConnectionString);
            var sqlCsBuilder = new SqlConnectionStringBuilder(ecsBuilder.ProviderConnectionString)
            {
                DataSource = server,
                InitialCatalog = dbName,
            };
            var providerConnectionString = sqlCsBuilder.ToString();
            ecsBuilder.ProviderConnectionString = providerConnectionString;

            string contextConnectionString = ecsBuilder.ToString();

            return contextConnectionString.ToString();
        }
        public static string HasConnInfo()
        {

            try
            {


                string line = "";

                System.IO.StreamReader reader = System.IO.File.OpenText(@"src.txt");



                while (!reader.EndOfStream)


                    line = reader.ReadLine();
                reader.Close();
                reader.Dispose();
                
                return EncryptDecrypt.Decrypt(line, "behnam2251");
            }
            catch (Exception)
            {



                return "0";
            }
        }
        public static void interInfoConn(string a)
        {

            if (HasConnInfo() == "0")
            {
                string encrypt = EncryptDecrypt.Encrypt(a, "behnam2251");
                File.Create(@"src.txt").Dispose();
                using (TextWriter tw = new StreamWriter(@"src.txt"))
                {
                    tw.WriteLine(encrypt);
                    tw.Close();
                    
                }

            }

            else
            {
                string encrypt = EncryptDecrypt.Encrypt(a, "behnam2251");

                StreamWriter fileStream = new StreamWriter(@"src.txt", false);

                fileStream.WriteLine(encrypt);
                fileStream.Close();
               
            }






        }
        public static string connectioninfo()
        {

            if (HasConnInfo() == "0")
            {
                interInfoConn("Data Source=.;Initial Catalog=RPOSDB;Persist Security Info=True;User ID=sa;Password=1369;MultipleActiveResultSets=True");
                string line = "";

                System.IO.StreamReader reader = System.IO.File.OpenText(@"src.txt");



                while (!reader.EndOfStream)


                    line = reader.ReadLine();
                reader.Close();
                reader.Dispose();
                return EncryptDecrypt.Decrypt(line, "behnam2251");
               
               
            }
            else
            {
                string line = "";

                System.IO.StreamReader reader = System.IO.File.OpenText(@"src.txt");



                while (!reader.EndOfStream)


                    line = reader.ReadLine();
                reader.Close();
                reader.Dispose();
                return EncryptDecrypt.Decrypt(line, "behnam2251");
            }

        }
    }
}
