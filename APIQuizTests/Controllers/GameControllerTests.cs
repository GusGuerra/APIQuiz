using APIQuiz.Controllers;
using APIQuiz.Models;
using APIQuiz.Util;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace APIQuiz.Controllers.Tests
{
    public class GameControllerTests
    {
        private GameController gameController;
        private PlayerController playerController;
        public GameControllerTests()
        {
            gameController = new();
            playerController = new();
        }

        [Fact]
        [Trait("GameController", "GetAnswer")]
        public async Task GetAnswer_Valid_Test()
        {
            Player player = new() { Name = "new_player_name" };
            _ = playerController.CreateNewPlayer(player);
            _ = await gameController.View("question", 0, player.Id, true);

            PlayerAnswer playerAnswer = new() { Answer = "True" };
            var getAnswerResult = gameController.GetAnswer(player.Id, playerAnswer);
            var okResult = getAnswerResult as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        [Trait("GameController", "View")]
        public async Task View_QuestionValid_TestAsync()
        {
            Player player = new() { Name = "new_player_name" };
            playerController.CreateNewPlayer(player);
            var viewResult = await gameController.View("question", 0, player.Id, true) as OkObjectResult;
            var question = viewResult.Value;

            Assert.IsType<PlayerFriendlyQuestion>(question);
        }

        [Fact]
        [Trait("GameController", "View")]
        public async Task View_RankingValid_TestAsync()
        {
            Player player = new() { Name = "new_player_name" };
            playerController.CreateNewPlayer(player);
            var viewResult = await gameController.View("ranking", 1, 0, GameServiceUtil.DEFAULT_NEW_QUESTION_OPTION) as OkObjectResult;
            var ranking = viewResult.Value;

            Assert.IsType<List<Player>>(ranking);
        }
    }
}