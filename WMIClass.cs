using System;
using System.Management;

namespace WMICodeCreator
{
    public class WMIClass
    {
        /// <summary>
        /// if the constructor is called with the necessary info then the managementclass is
        /// instantiated and this value is set to true
        /// </summary>
        public bool isInitialized = false;

        /// <summary>
        /// If the constructor is called with the namespoacename and class then the
        /// managementclass is instiantiated with all info filled
        /// </summary>
        public System.Management.ManagementClass thisInstance = new ManagementClass();

        /// <summary>
        /// The text description of the WMI Class.
        /// Occasionally, on lew rent jobs this is blank
        /// so we set it to "No descriptipn found in WMI"
        /// </summary>
        private string classDescription = "No description found in WMI";

        /// <summary>
        /// The count of class Properties
        /// </summary>
        private int propertyCount = 0;

        /// <summary>
        /// The count of class Qualifiers
        /// </summary>
        private int qualifierCount = 0;

        public WMIClass(){} // short and sweet

        public WMIClass(string paramNameSpaceName, string paramClassName)
        {
            this.Name_Space = paramNameSpaceName;
            this.Class_Name = paramClassName;
            this.thisInstance = new ManagementClass(Name_Space, this.Class_Name, new ObjectGetOptions(null, TimeSpan.MaxValue, true));
            this.isInitialized = true;
            // I should dazzle you here but I aint. I am a pleasure denyer.
        }

        /// <summary>
        /// The WMI CLass - e.g. WIN32_COMPUTERSYSTEM or whatever
        /// </summary>
        public string Class_Name { get; set; }

        /// <summary>
        /// The text description of the WMI Class.
        /// Occasionally, on lew rent jobs this is blank
        /// so we set it to "No descriptipn found in WMI"
        /// </summary>
        public string Class_Description { get => classDescription; set => classDescription = value; }

        /// <summary>
        /// The WMI Namespace, usually Root\\CIMV2 or the MSFT one
        /// </summary>
        public string Name_Space { get; set; }
        /// <summary>
        /// The count of class Properties
        /// </summary>
        public int Property_Count { get => propertyCount; set => propertyCount = value; }

        /// <summary>
        /// The count of Class Qualifiers 
        /// </summary>
        public int Qualifier_Count { get => qualifierCount; set => qualifierCount = value; }

        /// <summary>
        /// dummy method so I can see the various fields in system.management.managementclass
        /// </summary>
        public void doShit()
        {
            //ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);

            ManagementClass mc = new ManagementClass("root\\cimv2", "win32_BIOS", new ObjectGetOptions(null, System.TimeSpan.MaxValue, true));
            mc.Options.UseAmendedQualifiers = true;
            
            //mc.pr
        }
    }
}