using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ConflictsBackup
{
    class Program
    {
        private static string[] ourFoldes = { "Assets", "Packages", "ProjectSettings" };
        
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
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

            cmd.StandardInput.WriteLine("cd " + currentDirectory);
            cmd.StandardInput.WriteLine("git diff --name-only --diff-filter=U");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            
            cmd.WaitForExit();
            string[] conflictsPaths = cmd.StandardOutput.ReadToEnd()
                .Replace("\r", "")
                .Split("\n")
                .Where(str => ourFoldes.Any(str.StartsWith) && File.Exists(str))
                .ToArray();
            
            foreach (string path in conflictsPaths)
                File.Copy(path, path + ".backup", true);
            
            List<string> tmpList = conflictsPaths.ToList();
            tmpList.Insert(0, "python govno");
            conflictsPaths = tmpList.ToArray();
            
            File.WriteAllLines(currentDirectory + "\\.git\\backupsConflicts", conflictsPaths.Select(str => str + ".backup"), Encoding.UTF8);
            
            Console.WriteLine("Backup success");
        }
    }
}