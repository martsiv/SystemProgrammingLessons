using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; // for working with processes
using System.Reflection;

namespace _01_Processes.Model
{
    public class ProcessEntity
    {
        public int Id { get; set; }
        public string ProcessName { get; set; }
        public TimeOnly StartTime { get; set; }
        public string StartInfo { get; set; }
        public string BasePriority { get; set; }
        public string PriorityClass { get; set; }
        void dd()
        {
            Process[] processes = Process.GetProcesses();
            Console.WriteLine(processes[0].Id);
            Console.WriteLine(processes[0].ProcessName);
            Console.WriteLine(processes[0].StartTime);
            Console.WriteLine(processes[0].StartInfo);
            Console.WriteLine(processes[0].BasePriority);
            Console.WriteLine(processes[0].PriorityClass);
            
        }
    }
}
