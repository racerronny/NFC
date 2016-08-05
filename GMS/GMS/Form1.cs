using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GMS
{
    public partial class Form1 : Form
    {

        private Visitor currentVisitor;

        public Form1()
        {
            InitializeComponent();
            Visitor.InitializeDB();
        }

        private void LoadAll()
        {
            List<Visitor> visitors = Visitor.GetVisitors();

            lvUsers.Items.Clear();

            foreach (Visitor v in visitors)
            {

                ListViewItem item = new ListViewItem(new String[] { v.Id.ToString(), v.Name, v.Purpose });
                item.Tag = v;

                lvUsers.Items.Add(item);

            }
        }

        private void btnLoadAll_Click(object sender, EventArgs e)
        {
            LoadAll();
        }

        private void lvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count > 0)
            {
                ListViewItem item = lvUsers.SelectedItems[0];
                currentVisitor = (Visitor)item.Tag;

                int id = currentVisitor.Id;
                String n = currentVisitor.Name;
                String p = currentVisitor.Purpose;

                txtName.Text = n;
                txtId.Text = id.ToString();
                txtPurpose.Text = p;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            String n = txtName.Text;
            String p = txtPurpose.Text;

            if (String.IsNullOrEmpty(n) || String.IsNullOrEmpty(p))
            {
                MessageBox.Show("It's empty");
                return;
            }

            currentVisitor = Visitor.Insert(n, p);

            LoadAll();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            String n = txtName.Text;
            String p = txtPurpose.Text;

            if (String.IsNullOrEmpty(n) || String.IsNullOrEmpty(p))
            {
                MessageBox.Show("It's empty");
                return;
            }

            currentVisitor.Update(n, p);

            LoadAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentVisitor == null)
            {
                MessageBox.Show("No guest selected!");
                return;
            }

            currentVisitor.Delete();

            LoadAll();
        }

    }
}
