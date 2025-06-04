using System;
using System.IO;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;

namespace GLEI_GradeTrackingSystem
{
    class Program
    {
        //Purpose: Check the json string against the schema.
        private static bool ValidateData(string jsonData, string schemaFile, out IList<string> messages)
        {
            string schemaText = File.ReadAllText(schemaFile);
            JSchema jsonRules = JSchema.Parse(schemaText);

            JArray courses = JArray.Parse(jsonData);
            return courses.IsValid(jsonRules, out messages);
        }

        //Purpose: Check if file exists. If it does, populate a string with the json data. If not, then create a new json file to write to or exit.
        public static bool ReadFile(string fileName, ref string jsonObj)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    jsonObj = File.ReadAllText(fileName);
                    return true;
                }

                //if the file doesn't exist, prompt the user with a choice to create a new file or not
                else
                {
                    Console.WriteLine($"Grades data file {fileName} not found. Create new file? (y/n): ");
                    string createFileChoice = Console.ReadLine();
                    while (createFileChoice.ToUpper()[0] != 'Y' && createFileChoice.ToUpper()[0] != 'N')
                    {
                        Console.WriteLine("Invalid input. Create new file?");
                        createFileChoice = Console.ReadLine();
                    }

                    if (createFileChoice.ToUpper()[0] == 'N')
                        Environment.Exit(0);

                    File.Create(fileName).Close();

                    return false;
                }
            }
            
            catch (Exception e) { 
                Console.WriteLine($"Error loading data. Error: {e.Message}");
                return false;
            }
        }

        public static void printMainMenu()
        {
            Console.WriteLine("Press # from the above list to view/edit/delete a specific course.\n");

            Console.WriteLine("Press A to add a new course\n");

            Console.WriteLine("Press X to quit\n");
        }

        static void Main(string[] args)
        {
            string dataPath = "courseData.json";
            string courseData = "";

            string schemaPath = "courseSchema.json";
            string userChoice = " ";
            List<Course> courses = new List<Course>();

            IList<string> messages;

            if (ReadFile(dataPath, ref courseData))
            {
                if (ValidateData(courseData, schemaPath, out messages))
                {
                    var coursesCopy = System.Text.Json.JsonSerializer.Deserialize<List<Course>>(courseData);
                    if (coursesCopy != null)
                        courses = coursesCopy;
                }

                else
                    Console.WriteLine("Invalid Line");
            }

            while (userChoice.ToUpper()[0] != 'X' || string.IsNullOrEmpty(userChoice))
            {
                Console.WriteLine("\t\t\t\t\t    ~GRADING TRACKING SYSTEM~\t\t\t\t\n");
                Console.WriteLine("+--------------------------------------------------------------------------------------------------------------------+");
                Console.WriteLine("|\t\t\t\t\t\tGrades Summary\t\t\t\t\t\t\t     |");
                Console.WriteLine("+--------------------------------------------------------------------------------------------------------------------+");

                //print out all currently saved courses in the file
                if (courses.Count > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("#. Course        Marks Earned      Out Of     Percent");
                    for (int i = 0; i < courses.Count; i++) {
                        Console.WriteLine($"{i + 1}. {courses[i].Code,-13} {courses[i].getTotalMarksEarned(), 12:F2} {courses[i].getTotalWeight(), 11:F2} {courses[i].getCurrentPercentage(), 11:F2}");
                        Console.WriteLine();
                    }
                }

                else
                {
                    Console.WriteLine("There are currently no saved courses");
                }

                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------\n");

                printMainMenu();

                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");

                Console.Write("Enter a command: ");
                userChoice = Console.ReadLine();

                //if user inputs a digit matching a course index, enter the evaluation menu 
                if (char.IsDigit(userChoice[0]))
                    for (int i = 0; i < courses.Count(); i++)
                    {
                        if(int.TryParse(userChoice[0].ToString(), out int choice) && choice == i-1)
                        {
                            //clear console for a cleaner view
                            Console.Clear();
                            Console.WriteLine("\t\t\t\t\t    ~GRADING TRACKING SYSTEM~\t\t\t\t\n");
                            Console.WriteLine("+--------------------------------------------------------------------------------------------------------------------+");
                            Console.WriteLine($"|\t\t\t\t\t{courses[i].Code} Evaluations\t\t\t\t\t\t\t|");
                            Console.WriteLine("+--------------------------------------------------------------------------------------------------------------------+");

                            if (courses[i].Evaluation.Count == 0)
                            {
                                Console.WriteLine($"There are currently no evaluations for {courses[i].Code}");
                            }

                            else
                            {
                                Console.WriteLine("#. Evaluation        Marks Earned      Out Of     Percent        Course Marks          Weight/100");
                                for(int j = 0; j < courses[i].Evaluation.Count; j++) {
                                    Console.WriteLine($"{j + 1}. {courses[i].Evaluation[j].Description,-19} {courses[i].Evaluation[j].EarnedMarks,10:F2} {courses[i].Evaluation[j].OutOf,11:F2} {courses[i].Evaluation[j].percentEarned(), 11:F2} {courses[i].Evaluation[j].courseMarks(),19:F2} {courses[i].Evaluation[j].Weight,19:F2}");
                                }
                            }
                        }
                    }

                
            }
            
        }

    }
}
