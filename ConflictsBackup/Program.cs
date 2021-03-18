using System;
using System.Diagnostics;
using System.IO;

namespace ConflictsBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory(); // директория из консоли
            Process cmd = new ()
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            cmd.Start();

            cmd.StandardInput.WriteLine("cd ");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }
    }
}