using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLEI_GradeTrackingSystem
{

    class Evaluation
    {
        public string EvaluationName { get; set; } = "";
        public double OutOfEval {  get; set; }
        public double Weight { get; set; }
        public double MarksEarnedEval { get; set; }
    }

    class Courses
    {
        public string CourseName { get; set; } = "";
        public double MarksEarned { get; set; }
        public double OutOf {  get; set; }
        public double Percent {  get; set; }

        public List<Evaluation> Evaluation { get; set; } = new();
  
    }
}
