namespace MasterAndPartialViews.Models.Question
{
    public class AnswerViewModel
    {
        public AnswerViewModel(string question)
        {
            Answer = GetAnswer(question);
        }

        public string Answer { get; set; }

        private string GetAnswer(string question)
        {
            switch (question)
            {
                case "What is the answer to life, the universe, and everything?":
                    return "42";
                case "What is the first decimal place of Pi?":
                    return "1";
                default:
                    return "I used to know that one. Hmm, let's see...";
            }
        }
    }
}