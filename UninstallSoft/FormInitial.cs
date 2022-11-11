using Microsoft.Win32;
using System.Diagnostics;

namespace UninstallSoft
{
    public partial class FormInitial : Form
    {
        static List<string> Programs = new List<string>();
        public FormInitial()
        {
            InitializeComponent();
            GetProducts();
        }
        public void GetProducts()
        {

            RegistryKey regkey = Registry.LocalMachine
                .OpenSubKey("SOFTWARE").OpenSubKey("Microsoft").OpenSubKey("Windows")
                .OpenSubKey("CurrentVersion").OpenSubKey("Uninstall");


            foreach (string productId in regkey.GetSubKeyNames())
            {

                RegistryKey productkey = regkey.OpenSubKey(productId);
                var displayName = productkey.GetValue("DisplayName");
                var displayVersion = productkey.GetValue("DisplayVersion");
                var publisher = productkey.GetValue("Publisher");
                var uninstallString = productkey.GetValue("UninstallString");

                string displayNameValue = displayName != null ? displayName.ToString() : string.Empty;
                string displayVersionValue = displayVersion != null ? displayVersion.ToString() : string.Empty;
                string publisherValue = publisher != null ? publisher.ToString() : string.Empty;
                string uninstallStringValue = uninstallString != null ? uninstallString.ToString() : string.Empty;

                if (!string.IsNullOrEmpty(displayNameValue) && !string.IsNullOrEmpty(uninstallStringValue))
                {
                    //TreeNode subNode = new TreeNode("Version:  " + displayVersionValue + "         Publisher Value:  " + publisherValue);
                    //TreeNode newNode = new TreeNode(displayNameValue, new TreeNode[] { subNode });
                    TreeNode newNode = new TreeNode(displayNameValue + "        Version-" + displayVersionValue);
                    treeView1.Nodes.Add(newNode);
                    Programs.Add(uninstallStringValue);
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnUninstall.Enabled = true;
            //string x = treeView1.SelectedNode.Text;
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            int selectIndex = treeView1.SelectedNode.Index;
            uninstallProgram(Programs[selectIndex]);
        }

        private static void uninstallProgram(string program)
        {
            const string msiexec = "MsiExec.exe";

            DialogResult result = MessageBox.Show("Are you sure to uninstall the program?", "Warning!", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (result == DialogResult.Yes)
            {
                if (program.ToLower().Contains(msiexec.ToLower()))
                {
                    ProcessStartInfo info = new ProcessStartInfo(
                        msiexec, program.ToLower().Replace(msiexec.ToLower(), string.Empty));
                    Process.Start(info);
                }
                else
                {
                    Process.Start(program.ToLower().Replace(msiexec.ToLower(), string.Empty));
                }
            }

        }

    }
}