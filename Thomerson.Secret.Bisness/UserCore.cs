using Thomerson.Secret.Model;

namespace Thomerson.Secret.Core
{
    public class UserCore
    {
        public void CreateUser()
        {
            var sql = $"create table {CoreCommon.Table_User} ({nameof(User.UserID)} varchar(128), {nameof(User.Password)} varchar(128), {nameof(User.Salt)} varchar(128))";
            SQLiteCore.CreateTable(sql);
        }

        public void Insert(User user)
        {
            var sql = $@"INSERT INTO {CoreCommon.Table_User}({nameof(User.UserID)},{nameof(User.Password)},{nameof(User.Salt)})
                        VALUES('{user.UserID}','{user.Password}','{user.Salt}'); ";
            SQLiteCore.ExecuteNonQuery(sql);
        }

        public User GetUser(string userid)
        {
            var sql = $"select * from {CoreCommon.Table_User} where {nameof(User.UserID)} = '{userid}' ";
            var result = SQLiteCore.SqlRow(sql);
            if (result.Length == 3)
            {
                return new User()
                {
                    UserID = result[0],
                    Password = result[1],
                    Salt = result[2]
                };
            }
            return null;
        }

    }
}
