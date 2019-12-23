using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thomerson.Secret.Model;

namespace Thomerson.Secret.Core
{
    public class SecretCore
    {
        public void Create()
        {
            var sql = $"create table {CoreCommon.Table_Secret} ({nameof(UserSecret.Id)} varchar(128),{nameof(UserSecret.UserID)} varchar(128),{nameof(UserSecret.Password)} varchar(128), {nameof(UserSecret.Salt)} varchar(128), {nameof(UserSecret.Type)} varchar(128), {nameof(UserSecret.Remark)} varchar(256))";
            SQLiteCore.CreateTable(sql);
        }

        public void Insert(UserSecret model)
        {
            model.Id = Guid.NewGuid().ToString().Replace("-", "");
            var sql = $@"INSERT INTO {CoreCommon.Table_Secret}({nameof(UserSecret.Id)},{nameof(UserSecret.UserID)},{nameof(UserSecret.Password)},{nameof(UserSecret.Salt)},{nameof(UserSecret.Type)},{nameof(UserSecret.Remark)})
                        VALUES('{model.Id}','{model.UserID}','{model.Password}','{model.Salt}','{model.Type}','{model.Remark}'); ";
            SQLiteCore.ExecuteNonQuery(sql);
        }

        public UserSecret Get(string id)
        {
            var sql = $"SELECT * FROM {CoreCommon.Table_Secret} WHERE {nameof(UserSecret.Id)} = '{id}'";
            var result = SQLiteCore.SqlRow(sql);
            if (result != null && result.Length == 6)
            {
                var model = new UserSecret()
                {
                    Id = result[0],
                    UserID = result[1],
                    Password = result[2],
                    Salt = result[3],
                    Type = result[4],
                    Remark = result[5]
                };
                return model;
            }
            return null;
        }

        public List<UserSecret> GetAll(string currentPassword)
        {
            var sql = $"SELECT * FROM {CoreCommon.Table_Secret}";

            return SQLiteCore.SqlTable(sql)?.AsEnumerable().Select(s => new UserSecret()
            {
                Id = s.Field<string>($"{nameof(UserSecret.Id)}"),
                UserID = s.Field<string>($"{nameof(UserSecret.UserID)}"),
                //Password = s.Field<string>($"{nameof(UserSecret.Password)}"),
                Password = EncodeCore.DesEncrypt(s.Field<string>($"{nameof(UserSecret.Password)}"), EncodeCore.MD5Encoding(s.Field<string>($"{nameof(UserSecret.UserID)}"), s.Field<string>($"{nameof(UserSecret.Salt)}"), currentPassword)),
                Salt = s.Field<string>($"{nameof(UserSecret.Salt)}"),
                Type = s.Field<string>($"{nameof(UserSecret.Type)}"),
                Remark = s.Field<string>($"{nameof(UserSecret.Remark)}")
            })?.ToList();
        }

        public void Update(UserSecret model)
        {
            var sql = $@"UPDATE {CoreCommon.Table_Secret} SET 
                        {nameof(UserSecret.UserID)} = '{model.UserID}',
                        {nameof(UserSecret.Password)} = '{model.Password}',
                        {nameof(UserSecret.Salt)} = '{model.Salt}',
                        {nameof(UserSecret.Type)} = '{model.Type}',
                        {nameof(UserSecret.Remark)} = '{model.Remark}'
                        WHERE {nameof(UserSecret.Id)} = '{model.Id}'
                        ";
            SQLiteCore.ExecuteNonQuery(sql);
        }

        public void Delete(string id)
        {
            var sql = $" DELETE FROM {CoreCommon.Table_Secret} WHERE {nameof(UserSecret.Id)} = '{id}'";
            SQLiteCore.ExecuteNonQuery(sql);
        }
    }
}
