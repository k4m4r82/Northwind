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
    public partial class FrmEntryCategory : Form
    {

        private ICategoryRepository _categoryRepository;
        private Category _category = null;
        private bool isNewData = false;

        public IListener Listener { private get; set; }

        public FrmEntryCategory()
        {
            InitializeComponent();
        }

        public FrmEntryCategory(string header, ICategoryRepository categoryRepository) 
            : this()
        {
            lblHeader.Text = header;
            _categoryRepository = categoryRepository;
            isNewData = true;
        }

        public FrmEntryCategory(string header, Category category, ICategoryRepository categoryRepository)
            : this()
        {
            lblHeader.Text = header;

            _category = category;
            _categoryRepository = categoryRepository;

            txtCategoryName.Text = _category.CategoryName;
            txtDescription.Text = _category.Description;
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (isNewData)
                _category = new Category();

            _category.CategoryName = txtCategoryName.Text;
            _category.Description = txtDescription.Text;

            var result = 0;

            if (isNewData)
                result = _categoryRepository.Save(_category);
            else
                result = _categoryRepository.Update(_category);

            if (result > 0)
            {
                Listener.Ok(this, isNewData, _category);

                if (isNewData)
                {
                    txtCategoryName.Clear();
                    txtDescription.Clear();
                    txtCategoryName.Focus();

                }
                else
                    this.Close();
            }
            else
                MessageBox.Show("Data category gagal disimpan");
        }
    }
}
