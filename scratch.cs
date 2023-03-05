using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.IO;

namespace WMIObjects
{
    /// <summary>
    /// The Win32_Processor class represents a device that is capable of interpreting a sequence of machine instructions 
    /// on a Win32 computer system. On a multiprocessor machine, there will exist one instance of this class for each processor. 
    /// 
    /// </summary>
    public class root_cimv2_win32_processor
    {
        /// List of Key Properties that can be used to uniquely identify this WMI Object
        public List<string> KeyProperties = new List<string>();

        /// <summary>
        /// Processor address width in bits. 
        /// </summary>
        public int AddressWidth;

        /// <summary>
        /// The Architecture property specifies the processor architecture used by this platform. It returns one of the following 
        ///        integer values: 0 - x86  1 - MIPS  2 - Alpha  3 - PowerPC  6 - ia64  9 - x64   
        /// </summary>
        public int Architecture;

        /// <summary>
        /// String number for the asset tag of this processor 
        /// </summary>
        public string AssetTag;

        /// <summary>
        /// The availability and status of the device.  For example, the Availability property indicates that the device is 
        /// running and has full power (value=3), or is in a warning (4), test (5), degraded (10) or power save state (values 
        /// 13-15 and 17). Regarding the power saving states, these are defined as follows: Value 13 ("Power Save - Unknown") 
        /// indicates that the device is known to be in a power save mode, but its exact status in this mode is unknown; 14 
        /// ("Power Save - Low Power Mode") indicates that the device is in a power save state but still functioning, and may 
        /// exhibit degraded performance; 15 ("Power Save - Standby") describes that the device is not functioning but could 
        /// be brought to full power 'quickly'; and value 17 ("Power Save - Warning") indicates that the device is in a warning 
        /// state, though also in a power save mode. 
        /// </summary>
        public int Availability;

        /// <summary>
        /// No description found for this property in WMI
        /// </summary>
        public string Caption;

        /// <summary>
        /// Defines which functions the processor supports. 
        /// </summary>
        public int Characteristics;

        /// <summary>
        /// Indicates the Win32 Configuration Manager error code.  The following values may be returned:  0      This device 
        /// is working properly.  1      This device is not configured correctly.  2      Windows cannot load the driver for 
        /// this device.  3      The driver for this device might be corrupted, or your system may be running low on memory 
        /// or other resources.  4      This device is not working properly. One of its drivers or your registry might be corrupted. 
        ///  5      The driver for this device needs a resource that Windows cannot manage.  6      The boot configuration 
        /// for this device conflicts with other devices.  7      Cannot filter.  8      The driver loader for the device is 
        /// missing.  9      This device is not working properly because the controlling firmware is reporting the resources 
        /// for the device incorrectly.  10     This device cannot start.  11     This device failed.  12     This device cannot 
        /// find enough free resources that it can use.  13     Windows cannot verify this device's resources.  14     This 
        /// device cannot work properly until you restart your computer.  15     This device is not working properly because 
        /// there is probably a re-enumeration problem.  16     Windows cannot identify all the resources this device uses. 
        ///  17     This device is asking for an unknown resource type.  18     Reinstall the drivers for this device.  19 
        ///     Your registry might be corrupted.  20     Failure using the VxD loader.  21     System failure: Try changing 
        /// the driver for this device. If that does not work, see your hardware documentation. Windows is removing this device. 
        ///  22     This device is disabled.  23     System failure: Try changing the driver for this device. If that doesn't 
        /// work, see your hardware documentation.  24     This device is not present, is not working properly, or does not 
        /// have all its drivers installed.  25     Windows is still setting up this device.  26     Windows is still setting 
        /// up this device.  27     This device does not have valid log configuration.  28     The drivers for this device 
        /// are not installed.  29     This device is disabled because the firmware of the device did not give it the required 
        /// resources.  30     This device is using an Interrupt Request (IRQ) resource that another device is using.  31  
        ///    This device is not working properly because Windows cannot load the drivers required for this device. 
        /// </summary>
        public int ConfigManagerErrorCode;

        /// <summary>
        /// Indicates whether the device is using a user-defined configuration. 
        /// </summary>
        public bool ConfigManagerUserConfig;

        /// <summary>
        /// The CpuStatus property specifies the current status of the processor. Changes in status arise from processor usage, 
        /// not the physical condition of the processor. 
        /// </summary>
        public int CpuStatus;

        /// <summary>
        /// CreationClassName indicates the name of the class or the subclass used in the creation of an instance. When used 
        /// with the other key properties of this class, this property allows all instances of this class and its subclasses 
        /// to be uniquely identified. 
        /// </summary>
        public string CreationClassName;

        /// <summary>
        /// The current speed (in MHz) of this processor. 
        /// </summary>
        public int CurrentClockSpeed;

        /// <summary>
        /// The CurrentVoltage specifies the voltage of the processor. bits 0-6 of the field contain the processor's current 
        /// voltage times 10. This value is only set when SMBIOS designates a voltage value. For specific values, see VoltageCaps. 
        /// Example: field value for a processor voltage of 1.8 volts would be 92h = 80h + (1.8 x 10) = 80h + 18 = 80h + 12h. 
        /// </summary>
        public int CurrentVoltage;

        /// <summary>
        /// Processor data width in bits. 
        /// </summary>
        public int DataWidth;

        /// <summary>
        /// No description found for this property in WMI
        /// </summary>
        public string Description;

        /// <summary>
        /// The DeviceID property contains a string uniquely identifying the processor with other devices on the system. 
        /// </summary>
        public string DeviceID;

        /// <summary>
        /// ErrorCleared is a boolean property indicating that the error reported in LastErrorCode property is now cleared. 
        /// </summary>
        public bool ErrorCleared;

        /// <summary>
        /// ErrorDescription is a free-form string supplying more information about the error recorded in LastErrorCode property, 
        /// and information on any corrective actions that may be taken. 
        /// </summary>
        public string ErrorDescription;

        /// <summary>
        /// The ExtClock property specifies the external clock frequency. If the frequency is unknown this property is set 
        /// to null. 
        /// </summary>
        public int ExtClock;

        /// <summary>
        /// The processor family type. For example, values include "Pentium(R) processor with MMX(TM) technology" (14) and 
        /// "68040" (96). 
        /// </summary>
        public int Family;

        /// <summary>
        /// No description found for this property in WMI
        /// </summary>
        public DateTime InstallDate;

        /// <summary>
        /// The L2CacheSize property specifies the size of the processor's Level 2 cache. A Level 2 cache is an external memory 
        /// area that has a faster access times than the main RAM memory. 
        /// </summary>
        public int L2CacheSize;

        /// <summary>
        /// The L2CacheSpeed property specifies the clockspeed of the processor's Level 2 cache. A Level 2 cache is an external 
        /// memory area that has a faster access times than the main RAM memory. 
        /// </summary>
        public int L2CacheSpeed;

        /// <summary>
        /// The L3CacheSize property specifies the size of the processor's Level 3 cache. A Level 3 cache is an external memory 
        /// area that has a faster access times than the main RAM memory. 
        /// </summary>
        public int L3CacheSize;

        /// <summary>
        /// The L3CacheSpeed property specifies the clockspeed of the processor's Level 3 cache. A Level 3 cache is an external 
        /// memory area that has a faster access times than the main RAM memory. 
        /// </summary>
        public int L3CacheSpeed;

        /// <summary>
        /// LastErrorCode captures the last error code reported by the logical device. 
        /// </summary>
        public int LastErrorCode;

        /// <summary>
        /// The Level property further defines the processor type. The value  depends on the architecture of the processor. 
        /// </summary>
        public int Level;

        /// <summary>
        /// The LoadPercentage property specifies each processor's load capacity averaged over the last second. The term 'processor 
        /// loading' refers to the total computing burden each processor carries at one time. 
        /// </summary>
        public int LoadPercentage;

        /// <summary>
        /// The Manufacturer property specifies the name of the processor's manufacturer. Example: GenuineSilicon 
        /// </summary>
        public string Manufacturer;

        /// <summary>
        /// The maximum speed (in MHz) of this processor. 
        /// </summary>
        public int MaxClockSpeed;

        /// <summary>
        /// No description found for this property in WMI
        /// </summary>
        public string Name;

        /// <summary>
        /// The NumberOfCores property contains a Processor's total number of cores. e.g dual core machine will have NumberOfCores 
        /// = 2. 
        /// </summary>
        public int NumberOfCores;

        /// <summary>
        /// Number of enabled cores per processor socket. 
        /// </summary>
        public int NumberOfEnabledCore;

        /// <summary>
        /// The NumberOfLogicalProcessors property specifies the total number of logical processors. 
        /// </summary>
        public int NumberOfLogicalProcessors;

        /// <summary>
        /// A string describing the processor family type - used when the family property is set to 1 ("Other"). This string 
        /// should be set to NULL when the family property is any value other than 1. 
        /// </summary>
        public string OtherFamilyDescription;

        /// <summary>
        /// String number for the part number of this processor This value is set by the manufacturer and normally not changeable. 
        /// </summary>
        public string PartNumber;

        /// <summary>
        /// Indicates the Win32 Plug and Play device ID of the logical device.  Example: *PNP030b 
        /// </summary>
        public string PNPDeviceID;

        /// <summary>
        /// Indicates the specific power-related capabilities of the logical device. The array values, 0="Unknown", 1="Not 
        /// Supported" and 2="Disabled" are self-explanatory. The value, 3="Enabled" indicates that the power management features 
        /// are currently enabled but the exact feature set is unknown or the information is unavailable. "Power Saving Modes 
        /// Entered Automatically" (4) describes that a device can change its power state based on usage or other criteria. 
        /// "Power State Settable" (5) indicates that the SetPowerState method is supported. "Power Cycling Supported" (6) 
        /// indicates that the SetPowerState method can be invoked with the PowerState input variable set to 5 ("Power Cycle"). 
        /// "Timed Power On Supported" (7) indicates that the SetPowerState method can be invoked with the PowerState input 
        /// variable set to 5 ("Power Cycle") and the Time parameter set to a specific date and time, or interval, for power-on. 
        /// </summary>
        public int PowerManagementCapabilities;

        /// <summary>
        /// Boolean indicating that the Device can be power managed - ie, put into a power save state. This boolean does not 
        /// indicate that power management features are currently enabled, or if enabled, what features are supported. Refer 
        /// to the PowerManagementCapabilities array for this information. If this boolean is false, the integer value 1, for 
        /// the string, "Not Supported", should be the only entry in the PowerManagementCapabilities array. 
        /// </summary>
        public bool PowerManagementSupported;

        /// <summary>
        /// The ProcessorId property contains processor-specific information that describes the processor's features. For x86 
        /// class CPUs, the field's format depends on the processor's support of the CPUID instruction. If the instruction 
        /// is supported, the ProcessorId property contains two DWORD-formatted values. The first (offsets 08h-0Bh) is the 
        /// EAX value returned by a CPUID instruction with input EAX set to 1. The second (offsets 0Ch-0Fh) is the EDX value 
        /// returned by that instruction. Only the first two bytes of the ProcessorID property are significant (all others 
        /// are set to 0) and contain (in WORD-format) the contents of the DX register at CPU reset. 
        /// </summary>
        public string ProcessorId;

        /// <summary>
        /// The ProcessorType property specifies the processor's primary function. 
        /// </summary>
        public int ProcessorType;

        /// <summary>
        /// The Revision property specifies the system's architecture-dependent revision level. The meaning of this value depends 
        /// on the architecture of the processor. It contains the same values as the "Version" member, but in a numerical format. 
        /// </summary>
        public int Revision;

        /// <summary>
        /// A free form string describing the role of the processor - for example, "Central Processor"' or "Math Processor" 
        /// </summary>
        public string Role;

        /// <summary>
        /// The SecondLevelAddressTranslationExtensions property determines whether the processor supports address translation 
        /// extensions used for virtualization. 
        /// </summary>
        public bool SecondLevelAddressTranslationExtensions;

        /// <summary>
        /// String number for the serial number of this processor This value is set by the manufacturer and normally not changeable. 
        /// </summary>
        public string SerialNumber;

        /// <summary>
        /// The SocketDesignation property contains the type of chip socket used on the circuit. Example: J202 
        /// </summary>
        public string SocketDesignation;

        /// <summary>
        /// No description found for this property in WMI
        /// </summary>
        public string Status;

        /// <summary>
        /// StatusInfo is a string indicating whether the logical device is in an enabled (value = 3), disabled (value = 4) 
        /// or some other (1) or unknown (2) state. If this property does not apply to the logical device, the value, 5 ("Not 
        /// Applicable"), should be used. 
        /// </summary>
        public int StatusInfo;

        /// <summary>
        /// Stepping is a free-form string indicating the revision level of the processor within the processor family. 
        /// </summary>
        public string Stepping;

        /// <summary>
        /// The scoping System's CreationClassName. 
        /// </summary>
        public string SystemCreationClassName;

        /// <summary>
        /// The scoping System's Name. 
        /// </summary>
        public string SystemName;

        /// <summary>
        /// Number of threads per processor socket. 
        /// </summary>
        public int ThreadCount;

        /// <summary>
        /// A globally unique identifier for the processor.  This identifier may only be unique within a processor family. 
        /// </summary>
        public string UniqueId;

        /// <summary>
        /// CPU socket information including data on how this Processor can be upgraded (if upgrades are supported). This property 
        /// is an integer enumeration. 
        /// </summary>
        public int UpgradeMethod;

        /// <summary>
        /// The Version property specifies an architecture-dependent processor revision number. Note: This member is not used 
        /// in Windows 95. Example: Model 2, Stepping 12. 
        /// </summary>
        public string Version;

        /// <summary>
        /// The VirtualizationFirmwareEnabled property determines whether the Firmware has enabled virtualization extensions. 
        /// </summary>
        public bool VirtualizationFirmwareEnabled;

        /// <summary>
        /// The VMMonitorModeExtensions property determines whether the processor supports Intel or AMD Virtual Machine Monitor 
        /// extensions. 
        /// </summary>
        public bool VMMonitorModeExtensions;

        /// <summary>
        /// The VoltageCaps property specifies the voltage capabilities of the processor. Bits 0-3 of the field represent specific 
        /// voltages that the processor socket can accept. All other bits should be set to zero. The socket is configurable 
        /// if multiple bits are being set. For a range of voltages see CurrentVoltage. If the property is NULL, then the voltage 
        /// capabilities are unknown. 
        /// </summary>
        public int VoltageCaps;

    }
}
