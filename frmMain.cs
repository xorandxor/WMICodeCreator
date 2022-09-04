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
using System.Text;

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

        private static string GenerateSQLTable()
        {
            USE[wmi]
GO

/****** Object:  Table [dbo].[table]    Script Date: 9/4/2022 12:56:05 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE[dbo].[table](

    [id][bigint] IDENTITY(1, 1) NOT NULL,

    [dtg] [datetime] NOT NULL,

    [field] [nvarchar] (50) NULL,
 CONSTRAINT[PK_table] PRIMARY KEY CLUSTERED
(

   [id] ASC
)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON[PRIMARY]
) ON[PRIMARY]
GO

ALTER TABLE[dbo].[table] ADD CONSTRAINT[DF_table_dtg]  DEFAULT(getdate()) FOR[dtg]
GO

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

        private void lstClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                // Gets the property qualifiers.
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);

                ManagementClass mc = new ManagementClass(this.cmbNameSpaces.Text, this.lstClasses.SelectedItem.ToString(), op);
                mc.Options.UseAmendedQualifiers = true;
                sb.AppendLine();
                sb.AppendLine("============================================================================");

                sb.AppendLine("== Report for namespace ["+cmbNameSpaces.Text+"] and class ["+lstClasses.Text+"] ==");
                sb.AppendLine("============================================================================");
                sb.AppendLine();

                sb.AppendLine("----------------------------------------------------------------");
                sb.AppendLine("- Class Qualifiers:");
                sb.AppendLine("----------------------------------------------------------------");

                foreach(QualifierData qdc in mc.Qualifiers)
                {
                    sb.AppendLine("    - Class Qualifier: [" + qdc.Name + "] = [" + qdc.Value +"]");
                }


                sb.AppendLine();

                sb.AppendLine("----------------------------------------------------------------");
                sb.AppendLine("- System Properties:");
                sb.AppendLine("----------------------------------------------------------------");

                foreach (PropertyData pds in mc.SystemProperties)
                {
                    sb.AppendLine("  - System Property: [" + pds.Name + "] - CimType = [" + pds.Type + "]");
                    if (pds.Name.ToLower().Contains("genus"))
                    {
                        sb.AppendLine("       - genus: " + pds.Value);
                    }
                    if (pds.Name.ToLower().Contains("_class"))
                    {
                        sb.AppendLine("       - class: " + pds.Value);
                    }
                    if (pds.Name.ToLower().Contains("_superclass"))
                    {
                        sb.AppendLine("       - superclass: " + pds.Value);
                    }
                    if (pds.Name.ToLower().Contains("_dynasty"))
                    {
                        sb.AppendLine("       - dynasty: " + pds.Value);
                    }
                    if (pds.Name.ToLower().Contains("_derivation"))
                    {
                        sb.AppendLine("       - derivation: " + pds.Value);
                    }
                    if (pds.Name.ToLower().Contains("server"))
                    {
                        sb.AppendLine("       - server: " + pds.Value);
                    }
                    if (pds.Name.ToLower().Contains("namespace"))
                    {
                        sb.AppendLine("       - namespace: " + pds.Value);
                    }
                    if (pds.Name.ToLower().Contains("_path"))
                    {
                        sb.AppendLine("       - path: " + pds.Value);
                    }
                    if (pds.Name.ToLower().Contains("_relpath"))
                    {
                        sb.AppendLine("       - relative path: " + pds.Value);
                    }
                    if (pds.Name.ToLower().Contains("_property_c"))
                    {
                        sb.AppendLine("       - number of properties: " + pds.Value.ToString());
                    }


                    sb.AppendLine();

                }

                if (mc.Methods.Count > 0)
                {
                    sb.AppendLine("----------------------------------------------------------------");
                    sb.AppendLine("- Methods:");
                    sb.AppendLine("----------------------------------------------------------------");

                    foreach (MethodData method_data in mc.Methods)
                    {
                        sb.AppendLine("Method Property: [" + method_data.Name + "] ");
                        foreach(QualifierData mqd in method_data.Qualifiers)
                        {
                            sb.AppendLine("   method property qualifier: " + mqd.Name + " = " + mqd.Value);
                        }
                    }
                }
                    sb.AppendLine("----------------------------------------------------------------");
                sb.AppendLine("- Properties:");
                sb.AppendLine("----------------------------------------------------------------");

                foreach (PropertyData dataObject in mc.Properties)
                {

                    sb.AppendLine("Property: [" + dataObject.Name + "] CimType: [" + dataObject.Type.ToString() + "] is Array: [" + dataObject.IsArray.ToString()+"]");
                    foreach (QualifierData tmp in dataObject.Qualifiers)
                    {
                        if (tmp.Name.ToLower() == "key")
                        {
                            sb.AppendLine("** KEY PROPERTY **");
                            sb.AppendLine();
                        }
                    }
                    sb.AppendLine();

                    foreach (QualifierData qd in dataObject.Qualifiers)
                    {
                        sb.AppendLine("    Qualifier: (" + qd.Name + ") = (" + qd.Value + ")");
                        if (qd.Name.ToLower() == "mappingstrings")
                        {
                            sb.AppendLine("            Mapping strings:");
                            string[] vals = (string[])qd.Value;
                            sb.Append("            ");

                            foreach (string s in vals)
                            {
                                sb.Append(s + " | ");
                            }
                            sb.Append("\n");
                        }
                        if (qd.Name.ToLower() == "values")
                        {
                            sb.AppendLine("            Values:");
                            string[] vals = (string[])qd.Value;
                            sb.Append("            ");
                            foreach (string s in vals)
                            {
                                sb.Append(s + " | ");
                            }
                            sb.Append("\n");
                        }
                        if (qd.Name.ToLower() == "valuemap")
                        {
                            sb.AppendLine("            Value Map:");
                            string[] vals = (string[])qd.Value;
                            sb.Append("            ");
                            foreach (string s in vals)
                            {
                                sb.Append(s + " | ");
                            }
                            sb.Append("\n");
                        }

                    }    


                    sb.AppendLine();



//                 this.PropertyList.Items.Add(
  //                      dataObject.Name);
    //                propertyCount++;
                }

                rtbClassReport.Text = sb.ToString();
                rtbClassReport.SelectedText = "Report";
      //          this.PropertyStatus.Text =
      //              propertyCount + " properties found.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
               // this.PropertyStatus.Text = ex.Message;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.cmbNameSpaces.Items.Clear();
            System.Threading.ThreadPool.
                QueueUserWorkItem(
                new System.Threading.WaitCallback(
                this.AddNamespacesToTargetList));
            System.Threading.Thread.Sleep(1000);
            if(cmbNameSpaces.Items.Contains(@"root\CIMV2") )
            {
                cmbNameSpaces.SelectedItem = @"root\CIMV2";
            }
        }
    }
}

