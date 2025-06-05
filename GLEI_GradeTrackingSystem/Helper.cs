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

        public double percentEarned(){ return EarnedMarks / OutOf; }

        public double courseMarks() { return Weight*percentEarned(); }  
    }

    class Course
    {
        [Required]
        [JsonProperty("code")]
        public string? Code {  get; set; }

        [Required]
        [JsonProperty("evaluation")]
        public List<Evaluation> Evaluation { get; set; } = new();

        //Calculate total percentage achieved in the course so far 
        public double getTotalMarksEarned()
        {
            double totalMarksEarned = 0;
            foreach (Evaluation evaluation in Evaluation)
            {
                totalMarksEarned += (evaluation.EarnedMarks/evaluation.OutOf)*evaluation.Weight;
            }

            return totalMarksEarned;
        }

        //Calculate total percentage possible in the course so far 
        public double getTotalWeight()
        {
            double totalWeight = 0;
            foreach (Evaluation evaluation in Evaluation)
            {
                totalWeight += evaluation.Weight;
            }

            return totalWeight;
        }

        public double getCurrentPercentage(){ return (getTotalMarksEarned()/getTotalWeight())*100.0; }
    }
}
