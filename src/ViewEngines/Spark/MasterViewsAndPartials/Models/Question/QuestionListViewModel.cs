using System.Collections.Generic;

namespace MasterAndPartialViews.Models.Question
{
    public class QuestionListViewModel
    {
        public List<string> Questions = new List<string>
        {
            "What is the answer to life, the universe, and everything?",
            "What is the first decimal place of Pi?"
        };
    }
}