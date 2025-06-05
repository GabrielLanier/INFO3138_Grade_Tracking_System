/*
 * Names:   Gabriel Lanier Dugand, Evelyn Infante Lumbreras
 * Purpose: Create a grade tracking system that validates data read and input from/into a JSON file against a schema. Functions include: creating a new file if one not present and
 *          adding, deleting, and editing courses/evaluations.
 * Date:    June 5, 2025
 */

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
        private static bool ValidateInputFile(string jsonData, string schemaFile, out IList<string> messages)
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
                    string createFileChoice = Console.ReadLine().ToUpper();
                    while (createFileChoice[0] != 'Y' && createFileChoice[0] != 'N')
                    {
                        Console.WriteLine("Invalid input. Create new file? (y/n)");
                        createFileChoice = Console.ReadLine().ToUpper();
                    }

                    if (createFileChoice[0] == 'N')
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

        public static void printEvaluationListMenu()
        {
            Console.WriteLine("\nPress D to delete this course.\n");

            Console.WriteLine("Press A to add an evaluation. \n");

            Console.WriteLine("Press # from the above list to edit/delete a specific evaluation. \n");

            Console.WriteLine("Press X to return to the main menu. \n");
        }

        public static void printEvaluationMenu()
        {
            Console.WriteLine("\nPress D to delete this evaluation.\n");

            Console.WriteLine("Press E to edit this evaluation. \n");

            Console.WriteLine("Press X to return to the main menu. \n");
        }

        public static bool ValidateAgainstSchema(List<Course> courses, string schemaPath, out IList<string> messages)
        {
            string json = JsonConvert.SerializeObject(courses);
            JToken token = JToken.Parse(json);

            string schemaJson = File.ReadAllText(schemaPath);
            JSchema schema = JSchema.Parse(schemaJson);
          
            return token.IsValid(schema, out messages);;
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
                if (ValidateInputFile(courseData, schemaPath, out messages))
                {
                    var coursesCopy = JsonConvert.DeserializeObject<List<Course>>(courseData);
                    if (coursesCopy != null)
                        courses = coursesCopy;
                }

                else
                    Console.WriteLine("Invalid Line");
            }

            while (userChoice[0] != 'X')
            {
                Console.Clear();
                Console.WriteLine("\t\t\t\t\t    ~GRADING TRACKING SYSTEM~\t\t\t\t\n");
                Console.WriteLine("+--------------------------------------------------------------------------------------------------------------------+");
                Console.WriteLine("|\t\t\t\t\t\tGrades Summary\t\t\t\t\t\t\t     |");
                Console.WriteLine("+--------------------------------------------------------------------------------------------------------------------+");

                //print out all currently saved courses in the file
                if (courses.Count > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("#. Course        Marks Earned      Out Of     Percent");
                    for (int i = 0; i < courses.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {courses[i].Code,-13} {courses[i].getTotalMarksEarned(),12:F2} {courses[i].getTotalWeight(),11:F2} {courses[i].getCurrentPercentage(),11:F2}");
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
                userChoice = Console.ReadLine().ToUpper();

                //if user inputs a digit matching a course index, enter the evaluation menu 
                if (string.IsNullOrEmpty(userChoice))
                {
                    userChoice = " ";
                    continue;
                }

                else if (char.IsDigit(userChoice[0]))
                    for (int i = 0; i < courses.Count(); i++)
                    {
                        if (int.TryParse(userChoice[0].ToString(), out int choice) && choice == i + 1)
                        {
                            while (userChoice[0] != 'X')
                            {
                                //clear console for a cleaner view
                                Console.Clear();
                                Console.WriteLine("\t\t\t\t\t    ~GRADING TRACKING SYSTEM~\t\t\t\t\n");
                                Console.WriteLine("+--------------------------------------------------------------------------------------------------------------------+");
                                Console.WriteLine($"|\t\t\t\t\t\t{courses[i].Code} Evaluations\t\t\t\t\t\t\t     |");
                                Console.WriteLine("+--------------------------------------------------------------------------------------------------------------------+");

                                if (courses[i].Evaluation.Count == 0)
                                {
                                    Console.WriteLine($"There are currently no evaluations for {courses[i].Code}");
                                }

                                else
                                {
                                    Console.WriteLine("#. Evaluation        Marks Earned      Out Of     Percent        Course Marks          Weight/100");
                                    for (int j = 0; j < courses[i].Evaluation.Count; j++)
                                    {
                                        Console.WriteLine($"{j + 1}. {courses[i].Evaluation[j].Description,-19} {courses[i].Evaluation[j].EarnedMarks,10:F2} {courses[i].Evaluation[j].OutOf,11:F2} {courses[i].Evaluation[j].percentEarned() * 100,11:F2} {courses[i].Evaluation[j].courseMarks(),19:F2} {courses[i].Evaluation[j].Weight,19:F2}\n");
                                    }
                                }

                                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                                printEvaluationListMenu();
                                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                                Console.Write("Enter a command: ");
                                userChoice = Console.ReadLine().ToUpper();

                                //Delete the currently selected course
                                if (userChoice[0] == 'D')
                                {
                                    Console.Write("Are you sure you want to delete this course? (y/n): ");
                                    userChoice = Console.ReadLine();
                                    if ((userChoice.ToUpper()[0] == 'Y'))
                                    {
                                        courses.Remove(courses[i]);
                                        break;
                                    }
                                }

                                //add an evaluation to the specified course
                                else if (userChoice[0] == 'A')
                                {
                                    Evaluation newEvaluation = new Evaluation();
                                    Course temp = new Course();
                                    Console.Write("Enter a description: ");
                                    newEvaluation.Description = Console.ReadLine();

                                    Console.Write("Enter the 'out-of' mark: ");
                                    double outOf;
                                    while (!double.TryParse(Console.ReadLine(), out outOf))
                                    {
                                        Console.Write("Please enter number for the 'out-of' mark:");
                                    }
                                    newEvaluation.OutOf = outOf;

                                    Console.Write("Enter the % weight: ");
                                    double weight;
                                    while (!double.TryParse(Console.ReadLine(), out weight))
                                    {
                                        Console.Write("Please enter number for the 'weight' percentage:");
                                    }
                                    newEvaluation.Weight = weight;

                                    Console.Write("Enter marks earned or press ENTER to skip: ");
                                    double marksEarned = 0.0;
                                    string input = Console.ReadLine();

                                    if (!string.IsNullOrEmpty(input))
                                    {
                                        while (!double.TryParse(input, out marksEarned))
                                        {
                                            Console.Write("Enter a float number for marks earned: ");
                                            input = Console.ReadLine();
                                        }

                                        newEvaluation.EarnedMarks = marksEarned;
                                    }

                                    temp.Code = "TEST-1111";
                                    temp.Evaluation.Add(newEvaluation);

                                    List<Course> tempList = new List<Course>() { temp };

                                    if (ValidateAgainstSchema(tempList, schemaPath, out messages))
                                    {
                                        courses[i].Evaluation.Add(newEvaluation);
                                        Console.WriteLine("Evaluation added.");
                                    }
                                    else
                                        Console.WriteLine("Invalid data entered into evaluation. Evaluation not added.");

                                    Console.WriteLine("Press enter to go back to Main Menu");
                                    Console.ReadLine();
                                }

                                else if (char.IsDigit(userChoice[0]))
                                    for (int k = 0; k < courses[i].Evaluation.Count; k++)
                                    {
                                        if (int.TryParse(userChoice.ToString(), out int evalChoice) && evalChoice == k + 1)
                                        {
                                            while (userChoice[0] != 'X')
                                            {
                                                Console.Clear();
                                                Console.WriteLine("\t\t\t\t\t    ~GRADING TRACKING SYSTEM~\t\t\t\t\n");
                                                Console.WriteLine("+--------------------------------------------------------------------------------------------------------------------+");
                                                Console.WriteLine($"|\t\t\t\t\t{courses[i].Evaluation[k].Description}\t\t\t\t\t\t\t     |");
                                                Console.WriteLine("+--------------------------------------------------------------------------------------------------------------------+");

                                                Console.WriteLine("Marks Earned      Out Of     Percent        Course Marks          Weight/100");
                                                Console.WriteLine($"{courses[i].Evaluation[k].EarnedMarks,12:F2} {courses[i].Evaluation[k].OutOf,11:F2} {courses[i].Evaluation[k].percentEarned() * 100,11:F2} {courses[i].Evaluation[k].courseMarks(),19:F2} {courses[i].Evaluation[k].Weight,19:F2}\n");
                                                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");

                                                printEvaluationMenu();

                                                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");

                                                Console.Write("Enter a command: ");
                                                userChoice = Console.ReadLine().ToUpper();

                                                //Delete the currently selected course
                                                if (userChoice[0] == 'D')
                                                {
                                                    Console.Write("Are you sure you want to delete this evaluation? (y/n): ");
                                                    userChoice = Console.ReadLine();
                                                    if ((userChoice.ToUpper()[0] == 'Y'))
                                                    {
                                                        courses[i].Evaluation.Remove(courses[i].Evaluation[k]);
                                                        break;
                                                    }
                                                }

                                                //edit this evaluation
                                                else if (userChoice[0] == 'E')
                                                {
                                                    Evaluation newEvaluation = new Evaluation();
                                                    Course temp = new Course();
                                                    Console.Write("Enter a new description: ");
                                                    newEvaluation.Description = Console.ReadLine();

                                                    Console.Write("Enter the 'out-of' mark: ");
                                                    double outOf;
                                                    while (!double.TryParse(Console.ReadLine(), out outOf))
                                                    {
                                                        Console.Write("Please enter number for the 'out-of' mark:");
                                                    }
                                                    newEvaluation.OutOf = outOf;

                                                    Console.Write("Enter the % weight: ");
                                                    double weight;
                                                    while (!double.TryParse(Console.ReadLine(), out weight))
                                                    {
                                                        Console.Write("Please enter number for the 'weight' percentage:");
                                                    }
                                                    newEvaluation.Weight = weight;

                                                    Console.Write("Enter marks earned or press ENTER to skip: ");
                                                    double marksEarned = 0.0;
                                                    string input = Console.ReadLine();

                                                    if (!string.IsNullOrEmpty(input))
                                                    {
                                                        while (!double.TryParse(input, out marksEarned))
                                                        {
                                                            Console.Write("Enter a float number for marks earned: ");
                                                            input = Console.ReadLine();
                                                        }

                                                        newEvaluation.EarnedMarks = marksEarned;
                                                    }

                                                    temp.Code = "TEST-1111";
                                                    temp.Evaluation.Add(newEvaluation);

                                                    List<Course> tempList = new List<Course>() { temp };

                                                    if (ValidateAgainstSchema(tempList, schemaPath, out messages))
                                                    {

                                                        courses[i].Evaluation[k] = newEvaluation;
                                                        
                                                        Console.WriteLine("Evaluation updated.");
                                                    }
                                                    else
                                                        Console.WriteLine("Invalid data entered into evaluation. Evaluation not added.");

                                                    Console.WriteLine("Press enter to go back to Main Menu");
                                                    Console.ReadLine();
                                                }

                                            }
                                            userChoice = " ";
                                        }
                                    }
                            }

                            userChoice = " ";
                        }
                    }

                //add a new course to the list, but not to the file
                else if (userChoice[0] == 'A')
                {
                    Course newCourse;

                    do
                    {
                        newCourse = new Course();
                        Console.Write("Enter a course code: ");
                        newCourse.Code = Console.ReadLine();

                        List<Course> tempCourseList = new List<Course> { newCourse };

                        if (ValidateAgainstSchema(tempCourseList, schemaPath, out messages))
                        {
                            courses.Add(newCourse);
                            Console.WriteLine("Course added.");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid course data: ");
                            foreach (var msg in messages) { Console.WriteLine(msg); }
                        }

                    } while (true);

                    Console.WriteLine("Press enter to go back to Main Menu");
                    Console.ReadLine();

                }


            }

            //export data to json file
            string endData = JsonConvert.SerializeObject(courses);
            File.WriteAllText(dataPath, endData);


        }

    }
}
