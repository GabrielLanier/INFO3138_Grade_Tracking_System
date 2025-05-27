using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;


class Program
{
    static void Main(string[] args)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "grades.json");
        try
        {
            string gradeFile = File.ReadAllText(filePath);
        }

        catch (Exception e){
            Console.Error.WriteLine(e.Message);
        }

        

        Console.WriteLine("\t\t\t\t\t\t~GRADING TRACKING SYSTEM~\t\t\t\t\n");
        Console.WriteLine(" +--------------------------------------------------------------------------------------------------------------------+");



        
    }
}
