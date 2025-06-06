/*
 * Names:   Gabriel Lanier Dugand, Evelyn Infante Lumbreras
 * Purpose: Helper file that holds the class definitions needed in the main file.
 * Date:    June 5, 2025
 */


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GLEI_GradeTrackingSystem
{
    class Evaluation
    {
        [Required]
        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [Required]
        [JsonProperty("weight")]
        public double Weight { get; set; }

        [Required]
        [JsonProperty("outOf")]
        public double OutOf { get; set;} 

        [Required]
        [JsonProperty("earnedMarks")]
        public double EarnedMarks { get; set; }

        //Purpose: Calculate percent achieved in the evaluation
        public double percentEarned() => EarnedMarks / OutOf;

        //Purpose: Calculate the course marks achieved from the evaluation
        public double courseMarks() => Weight * percentEarned(); 
    }

    class Course
    {
        [Required]
        [JsonProperty("code")]
        public string? Code {  get; set; }

        [Required]
        [JsonProperty("evaluation")]
        public List<Evaluation> Evaluation { get; set; } = new();

        //Purpose: Calculate total percentage achieved in the course so far 
        public double getTotalMarksEarned()
        {
            double totalMarksEarned = 0;
            foreach (Evaluation evaluation in Evaluation)
            {
                totalMarksEarned += (evaluation.EarnedMarks/evaluation.OutOf)*evaluation.Weight;
            }

            return totalMarksEarned;
        }

        //Purpose: Calculate total percentage possible in the course so far 
        public double getTotalWeight()
        {
            double totalWeight = 0;
            foreach (Evaluation evaluation in Evaluation)
            {
                totalWeight += evaluation.Weight;
            }

            return totalWeight;
        }

        //Purpose: Calculate the grade achieved out of the evaluations completed so far 
        public double getCurrentPercentage() => (getTotalMarksEarned() / getTotalWeight()) * 100.0;
    }
}