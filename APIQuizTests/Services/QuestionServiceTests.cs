using Xunit;
using System.Threading.Tasks;
using APIQuiz.Models;

namespace APIQuiz.Services.Tests
{
    public class QuestionServiceTests
    {

        private readonly QuestionService questionService;

        public QuestionServiceTests()
        {
            questionService = new();
        }

        [Fact]
        [Trait("QuestionService","ViewNew")]
        public async Task ViewNew_Simple_Test()
        {
            var question = await questionService.ViewNew(1);

            Assert.IsType<Question>(question);
            Assert.Equal(1, question.Id);
        }
    }
}