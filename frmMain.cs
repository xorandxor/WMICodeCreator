using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace WMICodeCreator
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void toolStripButtonGenerate_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }



        //-------------------------------------------------------------------------
        // Calls the AddNamespacesToTargetListRecursive method to start with the
        // "root" namespace.
        //-------------------------------------------------------------------------
        private void AddNamespacesToTargetList(object o)
        {
            AddNamespacesToTargetListRecursive("root");
        }

        //-------------------------------------------------------------------------
        // Adds the namespaces to the TargetClassList_event list on the event tab
        // when the user selects the __Namespace*Event class.
        //-------------------------------------------------------------------------
        private void AddNamespacesToTargetListRecursive(string root)
        {
            try
            {
                // Enumerates all WMI instances of 
                // __namespace WMI class.
                ManagementClass nsClass =
                    new ManagementClass(
                    new ManagementScope(root),
                    new ManagementPath("__namespace"),
                    null);
                foreach (ManagementObject ns in
                    nsClass.GetInstances())
                {
                    // Add namespaces to the list.
                    string namespaceName = root + "\\" + ns["Name"].ToString();

                    //cross thread update
                    this.cmbNameSpaces.Invoke((MethodInvoker)delegate {
                        // Running on the UI thread
                        this.cmbNameSpaces.Items.Add(namespaceName);
                    });

                    
                    AddNamespacesToTargetListRecursive(namespaceName);
                }

            }
            catch (ManagementException e)
            {
                MessageBox.Show("Error creating a list of namespaces: " + e.Message);
            }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.cmbNameSpaces.Items.Clear();
            System.Threading.ThreadPool.
                QueueUserWorkItem(
                new System.Threading.WaitCallback(
                this.AddNamespacesToTargetList));
        }

        private void cmbNameSpaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lstClasses.Items.Clear();
            System.Threading.ThreadPool.
                QueueUserWorkItem(
                new System.Threading.WaitCallback(
                this.AddClassesToList));
        }


        //-------------------------------------------------------------------------
        // Populates the event tab's target class list with classes
        // that contain methods.
        //-------------------------------------------------------------------------
        private void AddClassesToList(object o)
        {
            try
            {
                // Performs WMI object query on the
                // selected namespace.
                string wmiclass = "";
                this.cmbNameSpaces.Invoke((MethodInvoker)delegate
                {
                    wmiclass = cmbNameSpaces.Text;
                });

                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(
                    new ManagementScope(
                    wmiclass),
                    new WqlObjectQuery(
                    "select * from meta_class"),
                    null);
                foreach (ManagementClass wmiClass in
                    searcher.Get())
                {
                    foreach (QualifierData qd in wmiClass.Qualifiers)
                    {
                        // If the class is dynamic or static, add it to the class
                        // list on the query tab.
                        if (qd.Name.Equals("dynamic") || qd.Name.Equals("static"))
                        {
                            this.lstClasses.Invoke((MethodInvoker)delegate
                            {
                                // Running on the UI thread
                                this.lstClasses.Items.Add(wmiClass["__CLASS"].ToString());
                            });

                        }
                    }
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("Error creating a list of classes: " + e.Message);
            }
        }
    }
}

