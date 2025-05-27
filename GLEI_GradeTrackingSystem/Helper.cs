using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GLEI_GradeTrackingSystem
{

    class Evaluation
    {
        [Required]
        public string Description { get; set; } = "";

        private double _weight;
        [Required]
        public double Weight
        {
            get => _weight;
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(Weight), "Weight must be between 0 and 100.");
                }
                _weight = value;
            }
        }

        private double _outOf;
        [Required]
        public double OutOf
        {
            get => _outOf;
            set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(OutOf), "Out of must be equal or greater than 0.");
                }
            }
        }

        private double _earnedMarks;
        [Required]
        public double EarnedMarks 
        {
            get => _earnedMarks;
            set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(EarnedMarks), "Earned marks must be equal or greater than 0.");
                }
            }
        }
    }

    class Course
    {
        [Required]

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                if(!Regex.IsMatch(value, @"^[A-Z]{4}-\d{4}$"))
                {
                    throw new ArgumentException("Course name must be in the format ABCD-1234");
                }
            }
        }
        public List<Evaluation> Evaluation { get; set; } = new();
  
    }
}
