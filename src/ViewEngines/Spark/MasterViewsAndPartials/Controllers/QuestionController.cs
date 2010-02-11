using MasterAndPartialViews.Models.Question;

namespace MasterAndPartialViews.Controllers
{
    public class QuestionController
    {
        public QuestionListViewModel Ask()
        {
            return new QuestionListViewModel();
        }

        public AnswerViewModel Answer(QuestionInputModel model)
        {
            return new AnswerViewModel(model.Question);
        }
    }
}