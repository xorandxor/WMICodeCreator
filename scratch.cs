using System;
using System.IO;
using System.Text;
using System.Management;
using System.Collections.Generic;

namespace Manage
{
    /// <summary>
    /// The Win32_Process class represents a sequence of events on a Win32 system. Any sequence consisting of the interaction 
    /// of one or more processors or interpreters, some executable code, and a set of inputs, is a descendent (or member) 
    /// of this class. Example: A client application running on a Win32 system. 
    /// </summary>
    public class root_cimv2_win32_process
    {
        /// List of Key Properties that can be used to uniquely identify this WMI Object
        public List<string> KeyProperties = new List<string>();

        /// <summary>

        /// No description found in WMI

        /// </summary>
        public string Caption;

        /// <summary>
        /// The CommandLine property specifies the command line used to start a particular process, if applicable. 
        /// </summary>
        public string CommandLine;

        /// <summary>
        /// CreationClassName indicates the name of the class or the subclass used in the creation of an instance. When used 
        /// with the other key properties of this class, this property allows all instances of this class and its subclasses 
        /// to be uniquely identified. 
        /// </summary>
        public string CreationClassName;

        /// <summary>
        /// Time that the process began executing. 
        /// </summary>
        public DateTime CreationDate;

        /// <summary>
        /// CSCreationClassName contains the scoping computer system's creation class name. 
        /// </summary>
        public string CSCreationClassName;

        /// <summary>
        /// The scoping computer system's name. 
        /// </summary>
        public string CSName;

        /// <summary>

        /// No description found in WMI

        /// </summary>
        public string Description;

        /// <summary>
        /// The ExecutablePath property indicates the path to the executable file of the process. Example: C:\WINDOWS\EXPLORER.EXE 
        /// </summary>
        public string ExecutablePath;

        /// <summary>
        /// Indicates the current operating condition of the process. Values include ready (2), running (3), and blocked (4), 
        /// among others. 
        /// </summary>
        public int ExecutionState;

        /// <summary>
        /// A string used to identify the process. A process ID is a kind of process handle. 
        /// </summary>
        public string Handle;

        /// <summary>
        /// The HandleCount property specifies the total number of handles currently open by this process. This number is the 
        /// sum of the handles currently open by each thread in this process. A handle is used to examine or modify the system 
        /// resources. Each handle has an entry in an internally maintained table. These entries contain the addresses of the 
        /// resources and the means to identify the resource type. 
        /// </summary>
        public int HandleCount;

        /// <summary>

        /// No description found in WMI

        /// </summary>
        public DateTime InstallDate;

        /// <summary>
        /// Time in kernel mode, in 100 nanoseconds. If this information is not available, a value of 0 should be used. 
        /// </summary>
        public int KernelModeTime;

        /// <summary>
        /// The MaximumWorkingSetSize property indicates the maximum working set size of the process. The working set of a 
        /// process is the set of memory pages currently visible to the process in physical RAM. These pages are resident and 
        /// available for an application to use without triggering a page fault. Example: 1413120. 
        /// </summary>
        public int MaximumWorkingSetSize;

        /// <summary>
        /// The MinimumWorkingSetSize property indicates the minimum working set size of the process. The working set of a 
        /// process is the set of memory pages currently visible to the process in physical RAM. These pages are resident and 
        /// available for an application to use without triggering a page fault. Example: 20480. 
        /// </summary>
        public int MinimumWorkingSetSize;

        /// <summary>

        /// No description found in WMI

        /// </summary>
        public string Name;

        /// <summary>
        /// The scoping operating system's creation class name. 
        /// </summary>
        public string OSCreationClassName;

        /// <summary>
        /// The scoping operating system's name. 
        /// </summary>
        public string OSName;

        /// <summary>
        /// The OtherOperationCount property specifies the number of I/O operations performed, other than read and write operations. 
        /// </summary>
        public int OtherOperationCount;

        /// <summary>
        /// The OtherTransferCount property specifies the amount of data transferred during operations other than read and 
        /// write operations. 
        /// </summary>
        public int OtherTransferCount;

        /// <summary>
        /// The PageFaults property indicates the number of page faults generated by the process. Example: 10 
        /// </summary>
        public int PageFaults;

        /// <summary>
        /// The PageFileUsage property indicates the amountof page file space currently being used by the process. Example: 
        /// 102435 
        /// </summary>
        public int PageFileUsage;

        /// <summary>
        /// The ParentProcessId property specifies the unique identifier of the process that created this process. Process 
        /// identifier numbers are reused, so they only identify a process for the lifetime of that process. It is possible 
        /// that the process identified by ParentProcessId has terminated, so ParentProcessId may not refer to an running process. 
        /// It is also possible that ParentProcessId incorrectly refers to a process which re-used that process identifier. 
        /// The CreationDate property can be used to determine whether the specified parent was created after this process 
        /// was created. 
        /// </summary>
        public int ParentProcessId;

        /// <summary>
        /// The PeakPageFileUsage property indicates the maximum amount of page file space  used during the life of the process. 
        /// Example: 102367 
        /// </summary>
        public int PeakPageFileUsage;

        /// <summary>
        /// The PeakVirtualSize property specifies the maximum virtual address space the process has used at any one time. 
        /// Use of virtual address space does not necessarily imply corresponding use of either disk or main memory pages. 
        /// However, virtual space is finite, and by using too much, the process might limit its ability to load libraries. 
        /// </summary>
        public int PeakVirtualSize;

        /// <summary>
        /// The PeakWorkingSetSize property indicates the peak working set size of the process. Example: 1413120 
        /// </summary>
        public int PeakWorkingSetSize;

        /// <summary>
        /// The Priority property indicates the scheduling priority of the process within the operating system. The higher 
        /// the value, the higher priority the process receives. Priority values can range from 0 (lowest priority) to 31 (highest 
        /// priority). Example: 7. 
        /// </summary>
        public int Priority;

        /// <summary>
        /// The PrivatePageCount property specifies the current number of pages allocated that are accessible only to this 
        /// process  
        /// </summary>
        public int PrivatePageCount;

        /// <summary>
        /// The ProcessId property contains the global process identifier that can be used to identify a process. The value 
        /// is valid from the creation of the process until the process is terminated. 
        /// </summary>
        public int ProcessId;

        /// <summary>
        /// The QuotaNonPagedPoolUsage property indicates the quota amount of non-paged pool usage for the process. Example: 
        /// 15 
        /// </summary>
        public int QuotaNonPagedPoolUsage;

        /// <summary>
        /// The QuotaPagedPoolUsage property indicates the quota amount of paged pool usage for the process. Example: 22 
        /// </summary>
        public int QuotaPagedPoolUsage;

        /// <summary>
        /// The QuotaPeakNonPagedPoolUsage property indicates the peak quota amount of non-paged pool usage for the process. 
        /// Example: 31 
        /// </summary>
        public int QuotaPeakNonPagedPoolUsage;

        /// <summary>
        /// The QuotaPeakPagedPoolUsage property indicates the peak quota amount of paged pool usage for the process.  Example: 
        /// 31 
        /// </summary>
        public int QuotaPeakPagedPoolUsage;

        /// <summary>
        /// The ReadOperationCount property specifies the number of read operations performed. 
        /// </summary>
        public int ReadOperationCount;

        /// <summary>
        /// The ReadTransferCount property specifies the amount of data read. 
        /// </summary>
        public int ReadTransferCount;

        /// <summary>
        /// The SessionId property specifies the unique identifier that is generated by the operating system when the session 
        /// is created. A session spans a period of time from log in to log out on a particular system. 
        /// </summary>
        public int SessionId;

        /// <summary>

        /// No description found in WMI

        /// </summary>
        public string Status;

        /// <summary>
        /// Time that the process was stopped or terminated. 
        /// </summary>
        public DateTime TerminationDate;

        /// <summary>
        /// The ThreadCount property specifies the number of active threads in this process. An instruction is the basic unit 
        /// of execution in a processor, and a thread is the object that executes instructions. Every running process has at 
        /// least one thread. This property is for computers running Windows NT only. 
        /// </summary>
        public int ThreadCount;

        /// <summary>
        /// Time in user mode, in 100 nanoseconds. If this information is not available, a value of 0 should be used. 
        /// </summary>
        public int UserModeTime;

        /// <summary>
        /// The VirtualSize property specifies the current size in bytes of the virtual address space the process is using. 
        /// Use of virtual address space does not necessarily imply corresponding use of either disk or main memory pages. 
        /// Virtual space is finite, and by using too much, the process can limit its ability to load libraries. 
        /// </summary>
        public int VirtualSize;

        /// <summary>
        /// The WindowsVersion property indicates the version of Windows in which the process is running. Example: 4.0 
        /// </summary>
        public string WindowsVersion;

        /// <summary>
        /// The amount of memory in bytes that a process needs to execute efficiently, for an operating system that uses page-based 
        /// memory management. If an insufficient amount of memory is available (< working set size), thrashing will occur. 
        /// If this information is not known, NULL or 0 should be entered.  If this data is provided, it could be monitored 
        /// to understand a process' changing memory requirements as execution proceeds. 
        /// </summary>
        public int WorkingSetSize;

        /// <summary>
        /// The WriteOperationCount property specifies the number of write operations performed. 
        /// </summary>
        public int WriteOperationCount;

        /// <summary>
        /// The WriteTransferCount property specifies the amount of data written. 
        /// </summary>
        public int WriteTransferCount;

    }
}
