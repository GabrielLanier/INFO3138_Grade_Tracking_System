using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
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

        

        Console.WriteLine("\t\t\t\t\t\t~GRADING TRACKING SYSTEM~\t\t\t\t\n");
        Console.WriteLine(" +--------------------------------------------------------------------------------------------------------------------+");



        
    }
}
