using System;
using System.IO;
using System.Text.Json;


class Program
{
    static void Main(string[] args)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "grades.json");
        if (File.Exists(filePath)){
            string gradeFile = File.ReadAllText(filePath);

            
        }

        else
        {
            Console.WriteLine("file not found in current directory");
        }

        
    }
}
