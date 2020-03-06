using System;
using System.Windows.Forms;
using Thomerson.Secret.Core;
using Thomerson.Secret.Model;

namespace Thomerson.Secret
{
    public partial class FMMain : Form
    {
        public FMMain()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.Secret;
            Init();
        }

        private SecretCore SecretCore { get; } = new SecretCore();

        private void Init()
        {
            SQLiteCore.Init(CoreCommon.DBName);
            dv_Main.DoubleClick += Dv_Main_DoubleClick;
            if (string.IsNullOrWhiteSpace(AppCommon.CurrentUserId) || string.IsNullOrWhiteSpace(AppCommon.CurrentPassword))
            {
                using (var fmLogin = new FMLogin())
                {
                    fmLogin.ShowDialog();
                }
            }
            Refesh();
        }


        private void Dv_Main_DoubleClick(object sender, EventArgs e)
        {
            var row = GetFocusRow();
            if (row != null)
            {
                var id = row.Id;
                if (!string.IsNullOrWhiteSpace(id))
                {
                    using (var edit = new FmUserEdit(id))
                    {
                        edit.ShowDialog();
                        Refesh();
                    }
                }
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            using (var edit = new FmUserEdit(null))
            {
                edit.ShowDialog();
                Refesh();
            }
        }


        private UserSecret GetFocusRow()
        {
            var focusRow = this.dv_Main.CurrentRow;
            if (focusRow != null)
            {
                return focusRow.DataBoundItem as UserSecret;
            }
            return null;
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            var row = GetFocusRow();
            if (row != null)
            {
                var id = row.Id;
                if (!string.IsNullOrWhiteSpace(id))
                {
                    SecretCore.Delete(id);
                    Refesh();
                }
            }
        }

        private void Refesh()
        {
            this.dv_Main.DataSource = SecretCore.GetAll(AppCommon.CurrentPassword);
        }
    }
}
