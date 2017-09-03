using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchApp
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnDoSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtLocalization.Text) && Directory.Exists(txtLocalization.Text))
            {
                string localization = txtLocalization.Text;
                string filter = !string.IsNullOrWhiteSpace(txtFilter.Text) ? txtFilter.Text : "*";

                IList<string> files = Directory.GetFiles(localization, filter, SearchOption.AllDirectories)
                                                    .Where(file => !new FileInfo(file).Attributes.HasFlag(FileAttributes.System | FileAttributes.Hidden))
                                                    .ToList();

                cbxLstItemsFound.DataSource = files;
            }
            else
            {
                MessageBox.Show("Type some location.", "Woops", MessageBoxButtons.OK);
            }
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cbxLstItemsFound.Items.Count; i++)
            {
                cbxLstItemsFound.SetItemChecked(i, true);
            }
        }

        private void btnSaveFiles_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDestination.Text))
            {
                string destinationPath = txtDestination.Text;
                IList<string> checkedFiles = new List<string>();

                for (int i = 0; i < cbxLstItemsFound.Items.Count; i++)
                {
                    if (cbxLstItemsFound.GetItemChecked(i))
                    {
                        checkedFiles.Add((string)cbxLstItemsFound.Items[i]);
                    }
                }

                foreach (string sourceFile in checkedFiles)
                {
                    FileInfo file = new FileInfo(sourceFile);
                    string directoryDestination = file.DirectoryName.Replace("\\", "-").Substring(3);
                    directoryDestination = Path.Combine(destinationPath, directoryDestination);

                    if (!Directory.Exists(directoryDestination))
                    {
                        Directory.CreateDirectory(directoryDestination);
                    }

                    string destinationFile = Path.Combine(directoryDestination, file.Name);
                    File.Copy(sourceFile, destinationFile, false);
                }

                MessageBox.Show("Copy finished", "Hey Ya", MessageBoxButtons.OK);

                cbxLstItemsFound.DataSource = null;
            }
            else
            {
                MessageBox.Show("Type some destination.", "Woops", MessageBoxButtons.OK);
            }
        }
    }
}
