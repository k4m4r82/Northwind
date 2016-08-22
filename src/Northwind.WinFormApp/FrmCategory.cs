using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Northwind.Model;
using Northwind.Repository.Api;
using Northwind.Repository.Service;

namespace Northwind.WinFormApp
{
    public partial class FrmCategory : Form, IListener
    {
        private ICategoryRepository categoryRepository;
        private IList<Category> listOfCategory;

        public FrmCategory()
        {
            InitializeComponent();
            InisialisasiListView();

            // class repository yang lama
            // categoryRepository = new CategoryRepository(); 

            // implementasi class repository yg baru
            categoryRepository = new CategoryRepositoryDapper(); 

            LoadDataCategory();
        }

        private void InisialisasiListView()
        {            
            lvwCategory.View = System.Windows.Forms.View.Details;
            lvwCategory.FullRowSelect = true;
            lvwCategory.GridLines = true;

            lvwCategory.Columns.Add("No.", 30, HorizontalAlignment.Center);
            lvwCategory.Columns.Add("Category Name", 300, HorizontalAlignment.Left);
            lvwCategory.Columns.Add("Description", 325, HorizontalAlignment.Left);
        }

        private void FillToListView(bool isNewData, Category category)
        {
            if (isNewData)
            {
                int noUrut = lvwCategory.Items.Count + 1;

                ListViewItem item = new ListViewItem(noUrut.ToString());
                item.SubItems.Add(category.CategoryName);
                item.SubItems.Add(category.Description);

                lvwCategory.Items.Add(item);
            }
            else
            {
                int row = lvwCategory.SelectedIndices[0];

                ListViewItem itemRow = lvwCategory.Items[row];
                itemRow.SubItems[2].Text = category.CategoryName;
                itemRow.SubItems[3].Text = category.Description;
            }
        }

        private void LoadDataCategory()
        {
            lvwCategory.Items.Clear();

            if (listOfCategory == null)
            {
                listOfCategory = categoryRepository.GetAll();
            }            

            foreach (var category in listOfCategory)
            {
                FillToListView(true, category);
            }
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (!(lvwCategory.SelectedItems.Count > 0))
            {
                MessageBox.Show("Data category belum dipilih");
                return;
            }

            var row = lvwCategory.SelectedIndices[0];
            var category = listOfCategory[row];

            string msg = "Apakah data '" + category.CategoryName + "' ingin dihapus ?";
            if (MessageBox.Show(msg, "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                var result = 0;

                result = categoryRepository.Delete(category);                    

                if (result > 0) // data category berhasil dihapus
                {
                    MessageBox.Show("Data category berhasil dihapus", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    listOfCategory.Remove(category);
                    LoadDataCategory();
                }
                else
                {
                    MessageBox.Show("Data category gagal dihapus", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            var frmEntry = new FrmEntryCategory("Tambah Data", categoryRepository);
            frmEntry.Listener = this;
            frmEntry.ShowDialog();
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            var category = (Category)data;

            if (isNewData)
            {
                listOfCategory.Add(category);                
            }
            else
            {
                var row = lvwCategory.SelectedIndices[0];
                listOfCategory[row] = category;
            }

            LoadDataCategory();
        }

        private void btnPerbaiki_Click(object sender, EventArgs e)
        {
            if (!(lvwCategory.SelectedItems.Count > 0))
            {
                MessageBox.Show("Data category belum dipilih");
                return;
            }

            var row = lvwCategory.SelectedIndices[0];
            var category = listOfCategory[row];

            var frmEntry = new FrmEntryCategory("Tambah Data", category, categoryRepository);
            frmEntry.Listener = this;
            frmEntry.ShowDialog();
        }
    }
}
