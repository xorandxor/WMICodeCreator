using System;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace WMICodeCreator
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
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
                    this.cmbNameSpaces.Invoke((MethodInvoker)delegate
                    {
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

        private void cmbNameSpaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lstClasses.Items.Clear();
            System.Threading.ThreadPool.
                QueueUserWorkItem(
                new System.Threading.WaitCallback(
                this.AddClassesToList));
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.cmbNameSpaces.Items.Clear();
            System.Threading.ThreadPool.
                QueueUserWorkItem(
                new System.Threading.WaitCallback(
                this.AddNamespacesToTargetList));
            System.Threading.Thread.Sleep(1000);
            if (cmbNameSpaces.Items.Contains(@"root\CIMV2"))
            {
                cmbNameSpaces.SelectedItem = @"root\CIMV2";
            }
        }

        private void Generate_Class_report(object o)
        {
            string nameSpaceName = "";
            string className = "";

            //cross thread update
            this.cmbNameSpaces.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                nameSpaceName = cmbNameSpaces.Text;
            });

            //cross thread update
            this.lstClasses.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                className = lstClasses.SelectedItem.ToString();
            });

            StringBuilder sb = new StringBuilder();
            try
            {
                // Gets the property qualifiers.
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);

                ManagementClass mc = new ManagementClass(nameSpaceName, className, op);
                mc.Options.UseAmendedQualifiers = true;
                sb.AppendLine();
                sb.AppendLine("==================================================================");
                sb.AppendLine("== Report for namespace [" + nameSpaceName + "] and class [" + className + "] ==");
                sb.AppendLine("==================================================================");
                sb.AppendLine();
                sb.AppendLine("----------------------------------------------------------------");
                sb.AppendLine("- Class Qualifiers:");
                sb.AppendLine("----------------------------------------------------------------");

                foreach (QualifierData qdc in mc.Qualifiers)
                {
                    sb.AppendLine("    - Class Qualifier: [" + qdc.Name + "] = [" + qdc.Value + "]");
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
                        foreach (QualifierData mqd in method_data.Qualifiers)
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
                    sb.AppendLine("Property: [" + dataObject.Name + "] CimType: [" + dataObject.Type.ToString() + "] is Array: [" + dataObject.IsArray.ToString() + "]");
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
                }

                //cross thread update
                this.rtbClassReport.Invoke((MethodInvoker)delegate
                {
                    // Running on the UI thread
                    rtbClassReport.Text = sb.ToString();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Generate_CSharp_Class(object o)
        {
            string nameSpaceName = "";
            string className = "";
            string classDescription = "";

            //cross thread update
            this.cmbNameSpaces.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                nameSpaceName = cmbNameSpaces.Text;
            });

            //cross thread update
            this.lstClasses.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                className = lstClasses.SelectedItem.ToString();
            });

            // this will be used for the name of the class in C#
            // it will look something like root_cimv2_win32_process

            string classObjectName = nameSpaceName.Replace("\\", "_") + "_" + className;

            StringBuilder sb = new StringBuilder();
            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
            ManagementClass mc = new ManagementClass(nameSpaceName, className, op);
            mc.Options.UseAmendedQualifiers = true;

            foreach (QualifierData qd in mc.Qualifiers)
            {
                if (qd.Name.ToLower() == "description")
                {
                    classDescription = qd.Value.ToString();
                }
            }

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using System.Management;");
            sb.AppendLine("using System.IO;");
            sb.AppendLine();
            sb.AppendLine("namespace WMIObjects");
            sb.AppendLine("{");

            sb.AppendLine("    <summary>" + classDescription + "</summary>");
            sb.AppendLine("    public class " + classObjectName);
            sb.AppendLine("    {");
            sb.AppendLine("        // List of Key Properties that can be used to uniquely identify this WMI Object");
            sb.AppendLine("        public list<string> KeyProperties = new list<string>();");
            sb.AppendLine();
            foreach (PropertyData propertyData in mc.Properties)
            {
                foreach (QualifierData qd in propertyData.Qualifiers)
                {
                    if (qd.Name.ToLower() == "description")
                    {
                        string desc = qd.Value.ToString();
                        if (desc.Length > 120)
                        {
                            desc = desc.Substring(0, 120);
                        }
                        sb.AppendLine("        // " + desc.Replace("\n", " "));
                    }
                }
                sb.AppendLine("        public " + TypeConvert.CimTypeToSystemType(propertyData.Type.ToString().ToLower()) + " " + propertyData.Name + ";");
                sb.AppendLine();
            }

            sb.AppendLine("    }");

            sb.AppendLine("}");

            //cross thread update
            this.rtbCSharp.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                rtbCSharp.Text = sb.ToString();
            });
        }

        private void Generate_SQL_Table(object o)
        {
            string nameSpaceName = "";
            string className = "";
            string databaseName = "";

            //cross thread update
            this.cmbNameSpaces.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                nameSpaceName = cmbNameSpaces.Text;
            });

            //cross thread update
            this.lstClasses.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                className = lstClasses.SelectedItem.ToString();
            });

            //cross thread update
            this.txtDatabaseName.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                databaseName = txtDatabaseName.Text.ToString();
            });

            StringBuilder sb = new StringBuilder();
            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
            ManagementClass mc = new ManagementClass(nameSpaceName, className, op);
            mc.Options.UseAmendedQualifiers = true;

            sb.AppendLine("USE[" + databaseName + "]");
            sb.AppendLine("GO");
            sb.AppendLine("SET ANSI_NULLS ON");
            sb.AppendLine("GO");
            sb.AppendLine("SET QUOTED_IDENTIFIER ON");
            sb.AppendLine("GO");
            sb.AppendLine("CREATE TABLE[dbo].[" + className + "](");
            sb.AppendLine("    [Id] [bigint] IDENTITY(1, 1) NOT NULL,");
            sb.AppendLine("    [Created] [datetime] NOT NULL,");
            sb.AppendLine("    [Hostname] [nvarchar] (255) NOT NULL,");

            foreach (PropertyData pd in mc.Properties)
            {
                string sqltype = TypeConvert.CimTypeToMSSQLType(pd.Type.ToString());
                if (sqltype == "nvarchar")
                {
                    sb.AppendLine("    [" + pd.Name + "] [nvarchar] (4000) NULL,");
                }
                else
                {
                    sb.AppendLine("    [" + pd.Name + "] [" + sqltype + "] NULL,");
                }
            }
            sb.AppendLine("CONSTRAINT [PK_" + className + "] PRIMARY KEY CLUSTERED");
            sb.AppendLine("(");
            sb.AppendLine("[id] ASC");
            sb.Append(") WITH ( PAD_INDEX=OFF, STATISTICS_NORECOMPUTE=OFF, ");
            sb.Append("IGNORE_DUP_KEY=OFF, ALLOW_ROW_LOCKS=ON, ALLOW_PAGE_LOCKS=ON,");
            sb.AppendLine("OPTIMIZE_FOR_SEQUENTIAL_KEY=OFF) ON [PRIMARY]");
            sb.AppendLine(") ON [PRIMARY]");
            sb.AppendLine("GO");
            sb.AppendLine("ALTER TABLE[dbo].[" + className + "] ADD CONSTRAINT[DF_" + className + "_dtg]  DEFAULT(getdate()) FOR [created]");
            sb.AppendLine("GO");

            //cross thread update
            this.rtbSQLTable.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                rtbSQLTable.Text = sb.ToString();
            });
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void lstClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(
                this.Generate_Class_report));

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(
                this.Generate_SQL_Table));

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(
                this.Generate_CSharp_Class));
        }

        private void rtbSQLTable_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(rtbSQLTable.Text);
            MessageBox.Show("Copied!");
        }

        private void rtbSQLTable_TextChanged(object sender, EventArgs e)
        {
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.cmbNameSpaces.Items.Clear();
            System.Threading.ThreadPool.
                QueueUserWorkItem(
                new System.Threading.WaitCallback(
                this.AddNamespacesToTargetList));
        }
    }
}