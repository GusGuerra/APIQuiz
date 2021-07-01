using Xunit;
using System.Threading.Tasks;
using APIQuiz.Models;

namespace APIQuiz.Services.Tests
{
    public class GameServiceTests
    {

        private readonly GameService gameService;

        public GameServiceTests()
        {
            gameService = new();
        }

        [Fact]
        [Trait("GameService","ViewNew")]
        public async Task ViewNew_Simple_Test()
        {
            var question = await gameService.ViewNew(1, true);

            Assert.IsType<Question>(question);
            Assert.Equal(1, question.Id);
        }

        [Fact]
        [Trait("GameService","ViewNew")]
        public async Task ViewNew_ReUseQuestion_Test()
        {
            var newQuestion = await gameService.ViewNew(1, true);
            var existingQuestion = await gameService.ViewNew(1, false);

            Assert.Equal(existingQuestion, newQuestion);
        }
    }
}