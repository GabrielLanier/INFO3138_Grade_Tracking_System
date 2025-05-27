using System;
using System.IO;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace GLEI_GradeTrackingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            using FileStream file = File.OpenRead("courses.json");
           
            var deCourses = JsonSerializer.Deserialize<List<Course>>(file);

            foreach (var c in deCourses)
            {
                
            }
  

            Console.WriteLine("\t\t\t\t\t    ~GRADING TRACKING SYSTEM~\t\t\t\t\n");
            Console.WriteLine(" +--------------------------------------------------------------------------------------------------------------------+");
            Console.WriteLine(" |\t\t\t\t\t\tGrades Summary\t\t\t\t\t\t\t      |");
            Console.WriteLine(" +--------------------------------------------------------------------------------------------------------------------+");
        }

    }
}
