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
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;

            _ = await gameController.View(player, "question", 0, createdPlayer.Id, true);

            PlayerAnswer playerAnswer = new() { Answer = "True", Password = player.Password };
            var getAnswerResult = gameController.GetAnswer(createdPlayer.Id, playerAnswer);
            var okResult = getAnswerResult as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        [Trait("GameController", "GetAnswer")]
        public async Task GetAnswer_InvalidPassword_Test()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "correctPasswd" };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;

            _ = await gameController.View(player, "question", 0, createdPlayer.Id, true);

            PlayerAnswer playerAnswer = new() { Answer = "True", Password = "incorrectPassword" };
            var getAnswerResult = gameController.GetAnswer(createdPlayer.Id, playerAnswer);
            var forbiddenResult = getAnswerResult as StatusCodeResult;

            Assert.NotNull(forbiddenResult);
            Assert.Equal(403, forbiddenResult.StatusCode);
        }

        [Fact]
        [Trait("GameController", "View")]
        public async Task View_Question_Valid_Test_Async()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };
            
            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;

            var viewResult = await gameController.View(player, "question", 0, createdPlayer.Id, true) as OkObjectResult;
            var question = viewResult.Value;

            Assert.IsType<PlayerFriendlyQuestion>(question);
        }

        [Fact]
        [Trait("GameController", "View")]
        public async Task View_Question_InvalidPassword_Test_Async()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "correctPasswd" };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;

            UserCreatedPlayer requestingPlayer = new() { Password = "incorrectPasswd" };

            var viewResult = await gameController.View(requestingPlayer, "question", 0, createdPlayer.Id, true);
            var forbiddenResult = viewResult as StatusCodeResult;

            Assert.NotNull(forbiddenResult);
            Assert.Equal(403, forbiddenResult.StatusCode);
        }

        [Fact]
        [Trait("GameController", "View")]
        public async Task View_Ranking_Simple_Test_Async()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };
            _ = playerController.CreateNewPlayer(player);
            var viewResult = await gameController.View(new UserCreatedPlayer(), "ranking", 1, 0, GameServiceUtil.DEFAULT_NEW_QUESTION_OPTION) as OkObjectResult;
            var ranking = viewResult.Value;

            Assert.IsType<List<Player>>(ranking);
        }
    }
}