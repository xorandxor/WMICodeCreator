using System;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace WMICodeCreator
{
    /// <summary>
    ///
    /// </summary>
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private static string GetValue(PropertyData pd)
        {
            string output = pd.Name + ": ";

            try
            {
                if (pd.IsArray)
                {
                    if (pd.Value == null)
                    {
                        output = "";
                    }
                    else
                    {
                        //uint16 doesnt play nice - there is one in every group
                        if (pd.Type.ToString().ToLower().Contains("uint16"))
                        {
                            UInt16[] nexus = (UInt16[])pd.Value;
                            foreach (UInt16 i in nexus)
                            {
                                output += "" + i.ToString() + ", ";
                            }
                        }

                        //numeric array
                        else if (pd.Type.ToString().ToLower().Contains("int") | (pd.Type.ToString().ToLower().Contains("real")))
                        {
                            double[] nexus = (double[])pd.Value;
                            foreach (double d in nexus)
                            {
                                output += "" + d.ToString() + ", ";
                            }
                        }
                        // string array
                        else if (pd.Type.ToString().ToLower().Contains("string"))
                        {
                            string[] nexus = (string[])pd.Value;
                            foreach (string s in nexus)
                            {
                                output += "" + s.ToString() + ", ";
                            }
                        }

                        //object array
                        else if (pd.Type.ToString().ToLower().Contains("object"))
                        {
                            object[] nexus = (object[])pd.Value;
                            foreach (object s in nexus)
                            {
                                output += "" + s.ToString() + ", ";
                            }
                        }
                    }
                }
                else if (pd.IsArray == false)
                {
                    if (pd.Value == null)
                    {
                        output = "";
                    }
                    else
                    {
                        //numeric
                        if (pd.Type.ToString().ToLower().Contains("int") | (pd.Type.ToString().ToLower().Contains("real")))
                        {
                            output += "" + pd.Value.ToString() + " ";
                        }
                        // string
                        else if (pd.Type.ToString().ToLower().Contains("string"))
                        {
                            {
                                output += "" + pd.Value.ToString() + " ";
                            }
                        }

                        //object
                        else if (pd.Type.ToString().ToLower().Contains("object"))
                        {
                            {
                                output += "" + pd.Value.ToString() + " ";
                            }
                        }
                    }
                }
            }
            catch (System.NullReferenceException nre)
            {
                output = pd.Name + ": null";
            }
            catch (Exception ex)
            {
                output = ex.Message;
            }
            return output;
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

        private string Generate_Class_Description(string desc)
        {
            string d = "    /// <summary>\n";
            d += "    /// ";
            int thisline = 0;
            foreach (string word in desc.Replace("\n", " ").Split(' '))
            {
                d += word + " ";
                thisline += word.Length + 1;
                if (thisline > 110)
                {
                    d += "\n";
                    d += "    /// ";
                    thisline = 0;
                }
            }
            d += "\n";
            d += "    /// </summary>\n";
            return d;
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
            classObjectName = classObjectName.ToLower();

            StringBuilder sb = new StringBuilder();
            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
            ManagementClass mc = new ManagementClass(nameSpaceName, className, op);
            mc.Options.UseAmendedQualifiers = true;


            WmiClass wmic = new WmiClass(mc);



            foreach (QualifierData qd in mc.Qualifiers)
            {
                if (qd.Name.ToLower() == "description")
                {
                    classDescription = qd.Value.ToString();
                }
            }

            sb.AppendLine("using System;");
            sb.AppendLine("using System.IO;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using System.Management;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine("namespace Manage");
            sb.AppendLine("{");
            /// <summary>
            ///
            /// </summary>
            sb.Append(Generate_Class_Description(classDescription));
            sb.AppendLine("    public class " + classObjectName);
            sb.AppendLine("    {");
            sb.AppendLine("        /// List of Key Properties that can be used to uniquely identify this WMI Object");
            sb.AppendLine("        public List<string> KeyProperties = new List<string>();");
            sb.AppendLine();
            foreach (PropertyData propertyData in mc.Properties)
            {
                bool desc_set = false;
                foreach (QualifierData qd in propertyData.Qualifiers)
                {
                    if (qd.Name.ToLower() == "description")
                    {
                        string desc = qd.Value.ToString();
                        sb.Append(Generate_Field_Description(desc.Replace("\n", " ")));
                        desc_set = true;
                    }
                }
                if (!desc_set)
                {
                    sb.AppendLine("        /// <summary>\n");
                    sb.AppendLine("        /// No description found in WMI\n");
                    sb.AppendLine("        /// </summary>");
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

        private string Generate_Fancy_Class_Header(string classname)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("    // ~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-");
            sb.AppendLine("    // ~-<>-~ " + classname);
            sb.AppendLine("    // ~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-~-<>-");
            return sb.ToString();
        }

        /// <summary>
        /// Formats the description of a class into multi line format
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string Generate_Field_Description(string desc)
        {
            string d = "        /// <summary>\n";
            d += "        /// ";
            int thisline = 0;
            string[] words = desc.Split(' ');
            for (int i = 0; i < desc.Split(' ').Length; i++)
            //            foreach (string word in desc.Split(' '))
            {
                d += words[i] + " ";
                thisline += words[i].Length + 1;
                if (thisline > 110 && i + 1 < words.Length)
                {
                    d += "\n";
                    d += "        /// ";
                    thisline = 0;
                }
            }
            d += "\n";
            d += "        /// </summary>\n";
            return d;
        }

        private void Generate_Nom_string(object o)
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

                // get the managementclass obj
                ManagementClass mc = new ManagementClass(nameSpaceName, className, op);
                mc.Options.UseAmendedQualifiers = true;

                // get collection of instances
                ManagementObjectCollection mob = mc.GetInstances();
                try
                {
                    sb.AppendLine();
                    sb.AppendLine("// ==================================================================");
                    sb.AppendLine("// ");//;
                    sb.AppendLine("// Report for [" + nameSpaceName + " \\ " + className + "] ");
                    sb.AppendLine("// ");
                    sb.AppendLine("// ==================================================================");
                    sb.AppendLine();
                    sb.AppendLine("----------------------------------------------------------------");
                    sb.AppendLine("- Instances: " + mob.Count.ToString());
                    sb.AppendLine("----------------------------------------------------------------");

                    foreach (ManagementObject mo in mob.Cast<ManagementObject>())
                    {
                        sb.AppendLine();
                        foreach (PropertyData pd in mo.Properties)
                        {
                            string val = GetValue(pd);
                            if (val != "")
                            {
                                sb.AppendLine(GetValue(pd));
                            }
                            //if (pd.IsArray)
                            //{
                            //    sb.Append(pd.Name + " = [array]: ");

                            //    sb.Append("[");

                            //    if (pd.Value != null)
                            //    {
                            //        try
                            //        {
                            //            foreach (string s in (string[])pd.Value)
                            //            {
                            //                sb.Append(s + " | ");
                            //            }
                            //        }
                            //        catch (System.InvalidCastException jank)
                            //        {
                            //            sb.Append("Trying to cast to an array failed. Raw Data:");
                            //            sb.AppendLine(Convert.ToString(pd.Value));
                            //        }
                            //    }
                            //    else
                            //    {
                            //        sb.AppendLine("Property value is null");
                            //    }
                            //    sb.AppendLine("]");
                            //}
                            //else
                            //{
                            //    if (pd.Value != null)
                            //    {
                            //        sb.AppendLine(pd.Name + " = " + pd.Value);
                            //    }
                            //    else { sb.AppendLine(pd.Name + " = Null value"); }
                            //}
                        }
                        sb.AppendLine("--------------------------------------------------------");

                        sb.AppendLine();
                    }

                    //cross thread update
                    this.richTextBox1.Invoke((MethodInvoker)delegate
                    {
                        // Running on the UI thread
                        richTextBox1.Text = sb.ToString();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        /// <summary>
        /// Formats the description of a property into multi line format
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string Generate_Property_Description(string desc)
        {
            string description = "";

            return description;
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(
                this.Generate_Nom_string));
        }

        private void rtbCSharp_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(rtbCSharp.Text);
            MessageBox.Show("copied!");
        }

        private void rtbCSharp_TextChanged(object sender, EventArgs e)
        {
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

        private void toolStripButtonGenerate_Click(object sender, EventArgs e)
        {
        }
    }
}
