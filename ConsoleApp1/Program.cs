using System.Diagnostics;
using System.IO;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string chars = "1234567890";
            Process process;

            for (int i = 0; i < chars.Length; i++)
                for (int j = 1; j < chars.Length; j++)
                    for (int k = 0; k < chars.Length; k++)
                        for (int o = 0; o < chars.Length; o++)
                        {
                            process = new Process
                            {
                                StartInfo = new ProcessStartInfo
                                {
                                    FileName = "cmd.exe",
                                    RedirectStandardInput = true,
                                    UseShellExecute = false
                                }
                            };

                            process.Start();

                            string ps = $"{chars[i]}{chars[j]}{chars[k]}{chars[o]}";
                            string commands = "\"C:\\Program Files\\7-Zip\\7z.exe \" -p" + ps + " x d:\\r\\364901.7z -od:\\r\\r  && taskkill /f /im \"Consoleapp1.exe\"  || RD /s/q d:\\r\\r ";

                            using (StreamWriter pWriter = process.StandardInput)
                            {
                                if (pWriter.BaseStream.CanWrite)
                                    pWriter.WriteLine(commands);
                            }
                            process.WaitForExit();
                        }
        }
    }
}