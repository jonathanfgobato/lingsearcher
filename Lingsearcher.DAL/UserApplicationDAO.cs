using Lingsearcher.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Lingsearcher.DAL
{
    public class UserApplicationDAO
    {
        protected string ConnectionString = "Server=lingsearcher.database.windows.net;Initial Catalog=lingsearcher;MultipleActiveResultSets=true;User ID=gobatoj;Password=wjfo$nfD";

        protected virtual IEnumerable<UserApplication> Query<UserApplication>(string procedure, object parameter = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<UserApplication>(procedure, parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public UserApplication GetEmailByUserSystemId(int userSystemId)
        {
            UserApplication userApplication = Query<UserApplication>($"Spr_GetEmailByUserSystemId", new { userSystemId }).SingleOrDefault();
            return userApplication;
        }
    }
}
