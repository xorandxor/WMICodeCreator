using System;
using System.Collections.Generic;
using System.Management;

namespace WMICodeCreator
{
    public class Methods_
    {
        public string Name { get; set; }
        public string Origin { get; set; }
        public int Count { get; set; } = 0;
        public List<Properties_> InParams { get; set; } = new List<Properties_>();
        public List<Properties_> Outparams { get; set; } = new List<Properties_>();
        public List<Qualifiers_> Qualifiers { get; set; } = new List<Qualifiers_>();
    }

    public class Properties_
    {
        public string CimType { get; set; }
        public bool IsArray { get; set; }
        public bool IsLocal { get; set; }
        public string Name { get; set; }
        public string Origin { get; set; }
        public List<Qualifiers_> Qualifiers { get; set; } = new List<Qualifiers_>();
        public string SQLType { get; set; }
        public string SystemType { get; set; }
        public object Value { get; set; }
    }

    public class Qualifiers_
    {

        public bool PropagatesToSubclass { get; set; } = false;
        public bool PropagatesToinstance { get; set; } = false;

        public bool IsLocal { get; set; } = false;
        public string Name { get; set; } = "";
        public object Value { get; set; } = new object();
    }

    public class SystemProperties_
    {
        public bool IsArray { get; set; }
         public bool IsLocal { get; set; }
        public string Name { get; set; }
        public string Origin { get; set; }
        public List<Qualifiers_> Qualifiers { get; set; } = new List<Qualifiers_>();
        public object Value { get; set; }
    }

    public class WmiClass
    {
        public WmiClass(ManagementClass mc)
        {
            foreach(PropertyData pd in mc.SystemProperties)
            {
                SystemProperties_ sProps = new SystemProperties_();
                sProps.Name = pd.Name;
                sProps.Value = pd.Value;
                sProps.IsArray = pd.IsArray;
                sProps.IsLocal = pd.IsLocal;
                sProps.Origin = pd.Origin;
                foreach(QualifierData qd in pd.Qualifiers)
                {
                    Qualifiers_ q = new Qualifiers_();
                    q.Name = qd.Name;
                    q.Value = qd.Value;
                    q.IsLocal = qd.IsLocal;
                    q.PropagatesToSubclass = qd.PropagatesToSubclass;
                    q.PropagatesToinstance = qd.PropagatesToInstance;
                    this.Qualifiers.Add(q);
                }
                this.SystemProperties.Add(sProps);
                
            }
            foreach (QualifierData qd in mc.Qualifiers)
            {
                Qualifiers_ q = new Qualifiers_();

                q.Name = qd.Name;
                q.IsLocal = qd.IsLocal;
                q.Value = qd.Value;
                q.PropagatesToSubclass = qd.PropagatesToSubclass;
                q.PropagatesToinstance = qd.PropagatesToInstance;

                this.Qualifiers.Add(q);
            }

            foreach (PropertyData pd in mc.Properties)
            {
                Properties_ p = new Properties_();
                p.Value = pd.Value;
                p.IsArray = pd.IsArray;
                p.CimType = Convert.ToString(pd.Type);
                p.SystemType = TypeConvert.CimTypeToSystemType(pd.Type.ToString().ToLower());
                p.SQLType = TypeConvert.CimTypeToMSSQLType(pd.Type.ToString().ToLower());
                p.Origin = Convert.ToString(pd.Origin);
                p.Name = pd.Name;
                foreach (QualifierData qd in pd.Qualifiers)
                {
                    Qualifiers_ q = new Qualifiers_();

                    q.Name = qd.Name;
                    q.IsLocal = qd.IsLocal;
                    q.PropagatesToSubclass = qd.PropagatesToSubclass;
                    q.PropagatesToinstance = qd.PropagatesToInstance;
                    q.Value = qd.Value;
                    p.Qualifiers.Add(q);
                }
                p.Value = pd.Value;
                this.Properties.Add(p);
            }

            foreach (MethodData md in mc.Methods)
            {
                Methods_ m = new Methods_();

                foreach (QualifierData qd in md.Qualifiers)
                {
                    Qualifiers_ q = new Qualifiers_();
                    q.Name = qd.Name;
                    q.IsLocal = qd.IsLocal;
                    q.Value = qd.Value;
                    q.PropagatesToSubclass = qd.PropagatesToSubclass;
                    q.PropagatesToinstance = qd.PropagatesToInstance;

                    this.Qualifiers.Add(q);
                }

                m.Name = md.Name;
                m.Origin = md.Origin;

                if (md.InParameters != null)
                {
                    foreach (PropertyData pd in md.InParameters.Properties)
                    {
                        Properties_ p = new Properties_();
                        p.Name = pd.Name;
                        p.IsLocal = pd.IsLocal;
                        p.Value = pd.Value;

                        foreach (QualifierData qd in pd.Qualifiers)
                        {
                            Qualifiers_ q = new Qualifiers_();
                            q.Name = qd.Name;
                            q.IsLocal = qd.IsLocal;
                            q.Value = qd.Value;
                            p.Qualifiers.Add(q);
                        }
                        m.InParams.Add(p);
                    }
                }
                if (md.OutParameters != null)
                {
                    foreach (PropertyData pd in md.OutParameters.Properties)
                    {
                        Properties_ p = new Properties_();
                        p.Name = pd.Name;
                        p.IsLocal = pd.IsLocal;
                        p.Value = pd.Value;

                        foreach (QualifierData qd in pd.Qualifiers)
                        {
                            Qualifiers_ q = new Qualifiers_();
                            q.Name = qd.Name;
                            q.IsLocal = qd.IsLocal;
                            q.Value = qd.Value;
                            p.Qualifiers.Add(q);
                        }
                        m.Outparams.Add(p);
                    }
                }
                this.Methods.Add(m);

            }
        }

        public List<Properties_> Properties { get; set; } = new List<Properties_>();

        public List<Qualifiers_> Qualifiers { get; set; } = new List<Qualifiers_>();

        public List<SystemProperties_> SystemProperties { get; set; } = new List<SystemProperties_>();

        public List<Methods_> Methods { get; set; } = new List<Methods_>();

        public static void non()
        {
            ManagementClass mc = new ManagementClass();
            foreach (MethodData md in mc.Methods)
            {

            }
            // mc.SystemProperties

            //mc.Qualifiers = new List<string>();
            //mc.Properties = new List<string>();

            ////foreach property
            //foreach (QualifierData qd in mc.Qualifiers)
            //{
            //    qd.Value = true;
            //    qd.IsLocal = true;
            //    qd.Name = qd.Value;
            //}

            //mc.Methods = new List<string>();
            //mc.Path
        }
    }

}
