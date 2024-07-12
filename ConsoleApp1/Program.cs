using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к файлу архива (например, d:\\r\\12345.7z):");
            string archivePath = Console.ReadLine();

            Console.WriteLine("Введите путь для распаковки (например, d:\\r\\r):");
            string outputPath = Console.ReadLine();

            string chars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            int maxLength = 4; // Максимальная длина комбинации

            for (int length = 1; length <= maxLength; length++)
                GenerateCombinations(chars, new StringBuilder(), length, archivePath, outputPath);
        }

        static void GenerateCombinations(string chars, StringBuilder current, int maxLength, string archivePath, string outputPath)
        {
            if (current.Length == maxLength)
            {
                StartProcess(current.ToString(), archivePath, outputPath);
                Console.WriteLine(current.ToString());
                return;
            }

            for (int i = 0; i < chars.Length; i++)
            {
                current.Append(chars[i]);
                GenerateCombinations(chars, current, maxLength, archivePath, outputPath);
                current.Length--; // Удаление последнего символа
            }
        }

        static void StartProcess(string ps, string archivePath, string outputPath)
        {
            string commands = $"\"C:\\Program Files\\7-Zip\\7z.exe \" -p" + ps + " x " + archivePath + " -o" + outputPath + " && taskkill /f /im \"Consoleapp1.exe\"  || RD /s/q d:\\r\\r ";

            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true // скрыть окно консоли
                }
            };

            process.Start();

            using (StreamWriter pWriter = process.StandardInput)
            {
                if (pWriter.BaseStream.CanWrite)
                {
                    pWriter.WriteLine(commands);
                    pWriter.Close();
                }
            }

            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // Фильтрация вывода
            FilterOutput(result);

        }

        static void FilterOutput(string output)
        {
            // Пример фильтрации: выводим только строки, содержащие "Extracting"
            using (StringReader reader = new StringReader(output))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    if (line.Contains("Extracting"))
                        Console.WriteLine(line);
            }
        }
    }
}