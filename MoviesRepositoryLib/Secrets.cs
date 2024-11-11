using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRepositoryLib
{
    public class Secrets
    {
        public static string ConnectionString =
            "Server=tcp:anbo-databaseserver.database.windows.net,1433;Initial Catalog=anbobase;Persist Security Info=False;User ID=anbo;Password=Hemmelig14!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public static string ConnectionStringSimply =
          "Data Source=mssql7.unoeuro.com;Initial Catalog=anbo_zealand_dk_db_anbo_base;Persist Security Info=True;User ID=anbo_zealand_dk;Password=FEkfzc2ndpg6DraxB4H3;TrustServerCertificate=True";
        // https://stackoverflow.com/questions/17615260/the-certificate-chain-was-issued-by-an-authority-that-is-not-trusted-when-conn/70850834#70850834
    }
}
