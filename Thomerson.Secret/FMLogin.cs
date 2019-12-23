using System;
using System.Drawing;
using System.Windows.Forms;
using Thomerson.Secret.Core;

namespace Thomerson.Secret
{
    public partial class FMLogin : Form
    {
        public FMLogin()
        {
            InitializeComponent();
            Init();
        }

        private UserCore UserCore { get; set; } = new UserCore();
        private void Init()
        {
            txt_userid.TextChanged += txt_TextChanged;
            txt_password.TextChanged += txt_TextChanged;
            txt_userid.KeyDown += txt_KeyDown;
            txt_password.KeyDown += txt_KeyDown;

            if (!SQLiteCore.tabbleIsExist(CoreCommon.Table_User))
            {
                this.Hide();
                using (var register = new FmRegister())
                {
                    register.ShowDialog();
                }
            }
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            var userid = txt_userid.Text.Trim();
            if (string.IsNullOrWhiteSpace(userid))
            {
                txt_userid.Focus();
                txt_userid.BackColor = Color.Red;
                return;
            }

            var password = txt_password.Text.Trim();
            if (string.IsNullOrWhiteSpace(password))
            {
                txt_password.Focus();
                txt_password.BackColor = Color.Red;
                return;
            }

            var user = UserCore.GetUser(userid);
            if (user != null)
            {
                var encode = EncodeCore.MD5Encoding(user.UserID, password, user.Salt);
                if (encode == user.Password)
                {
                    //验证成功
                    AppCommon.CurrentUserId = user.UserID;
                    AppCommon.CurrentPassword = password;
                    this.Close();
                }
            }
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            txt_userid.BackColor = Color.White;
            txt_password.BackColor = Color.White;
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                btn_Login_Click(null, null);
            }
        }
    }
}
