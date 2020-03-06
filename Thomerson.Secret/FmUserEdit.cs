using System;
using System.Windows.Forms;
using Thomerson.Secret.Core;
using Thomerson.Secret.Model;

namespace Thomerson.Secret
{
    public partial class FmUserEdit : Form
    {
        private FmUserEdit()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.Secret;
        }

        private string Id { get; set; }
        private string Salt { get; set; }
        public FmUserEdit(string id)
        {
            this.Id = id;
            InitializeComponent();
            Init();
        }

        private SecretCore SecretCore { get; } = new SecretCore();
        private void Init()
        {
            if (Id != null)
            {
                var model = SecretCore.Get(Id);
                if (model != null)
                {
                    Set(model);
                    this.txt_userid.ReadOnly = true;
                }
            }
            else
            {
                this.Salt = Guid.NewGuid().ToString().Replace("-", "");
            }
        }

        private UserSecret Get()
        {
            var model = new UserSecret()
            {
                Id = this.Id,
                UserID = this.txt_userid.Text.Trim(),
                Password = this.txt_password.Text.Trim(),
                Salt = this.Salt,
                Type = this.txt_type.Text.Trim(),
                Remark = this.txt_remark.Text.Trim()
            };
            var key = EncodeCore.MD5Encoding(model.UserID, model.Salt,AppCommon.CurrentPassword);
            model.Password = EncodeCore.DesEncrypt(model.Password, key);
            return model;
        }

        private void Set(UserSecret model)
        {
            this.txt_userid.Text = model.UserID;
            var key = EncodeCore.MD5Encoding(model.UserID, model.Salt, AppCommon.CurrentPassword);
            this.txt_password.Text = EncodeCore.DesDecrypt(model.Password, key);
            this.Salt = model.Salt;
            this.txt_type.Text = model.Type;
            this.txt_remark.Text = model.Remark;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            var model = Get();
            if (string.IsNullOrEmpty(this.Id))
            {
                SecretCore.Insert(model);
            }
            else
            {
                SecretCore.Update(model);
            }
            this.Close();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
