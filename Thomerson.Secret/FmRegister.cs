using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thomerson.Secret.Core;
using Thomerson.Secret.Model;

namespace Thomerson.Secret
{
    public partial class FmRegister : Form
    {
        public FmRegister()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            txt_userId.KeyDown += txt_KeyDown;
            txt_password.KeyDown += txt_KeyDown;
            txt_password2.KeyDown += txt_KeyDown;
        }


        private UserCore UserCore { get; } = new UserCore();

        private void btn_OK_Click(object sender, EventArgs e)
        {
            var userId = txt_userId.Text.Trim();
            if (userId.Length < 6)
            {
                MessageBox.Show("用户名至少6个字符");
                return;
            }
            var password = txt_password.Text.Trim();
            var password2 = txt_password2.Text.Trim();
            if (password != password2)
            {
                MessageBox.Show("两次输入的密码不一致，请重新输入");
                return;
            }
            var regex = new Regex(@"
                    (?=.*[0-9])                     #必须包含数字
                    (?=.*[a-z])
                    (?=.*[A-Z])                  #必须包含小写和大写字母
                    (?=([\x21-\x7e]+)[^a-zA-Z0-9])  #必须包含特殊符号
                    .{8,30}                         #至少8个字符，最多30个字符
                    ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (!regex.IsMatch(password))
            {
                MessageBox.Show("密码不符合规则，请重新设计密码");
                return;
            }


            UserCore.CreateUser();
            new SecretCore().Create();

            var user = new User()
            {
                Salt = Guid.NewGuid().ToString().Replace("-", ""),
                UserID = userId
            };
            user.Password = EncodeCore.MD5Encoding(user.UserID, password, user.Salt);
            UserCore.Insert(user);
            this.Close();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                btn_OK_Click(null, null);
            }
        }
    }
}
