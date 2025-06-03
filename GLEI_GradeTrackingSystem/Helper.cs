using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GLEI_GradeTrackingSystem
{

    class Evaluation
    {
        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; } = "";

        [Required]
        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        [Required]
        [JsonPropertyName("outOf")]
        public double OutOf { get; set;} 

        [Required]
        [JsonPropertyName("earnedMarks")]
        public double EarnedMarks { get; set; }
    }

    class Course
    {
        [Required]
        [JsonPropertyName("code")]
        public string? Code {  get; set; }

        [Required]
        [JsonPropertyName("evaluation")]
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

        public double getCurrentPercentage(){ return (getTotalMarksEarned()/getTotalWeight())*100; }
    }
}
