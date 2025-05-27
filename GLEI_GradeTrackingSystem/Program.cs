using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;


class Program
{
    static void Main(string[] args)
    {
        using (StreamReader file = new StreamReader("grades.json"))
        {
            string currLine;
            while ((currLine = file.ReadLine()) != null)
            {
                string[] values = currLine.Split(' ');

            }
        }

        Console.WriteLine("\t\t\t\t\t\t~GRADING TRACKING SYSTEM~\t\t\t\t\n");
        Console.WriteLine(" +--------------------------------------------------------------------------------------------------------------------+");
    }

    
}
