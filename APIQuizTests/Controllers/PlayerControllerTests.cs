using APIQuiz.Models;
using APIQuiz.Util;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace APIQuiz.Controllers.Tests
{
    public class PlayerControllerTests
    {
        private PlayerController playerController;
        public PlayerControllerTests()
        {
            playerController = new();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [Trait("Controller", "CreateNewPlayer")]
        public void CreateNewPlayer_InvalidName_Test(string name)
        {
            UserCreatedPlayer player = new() { Name = name, Password = "randomPasswd"};

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var badRequestResult = createNewPlayerResult as BadRequestResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("ab")]
        [InlineData("password_123")]
        [InlineData("dontUseTheWordPassword")]
        [InlineData("dontUseThreeConsecutiveNumbers456")]
        [Trait("Controller", "CreateNewPlayer")]
        public void CreateNewPlayer_InvalidPassword_Test(string password)
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = password };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var badRequestResult = createNewPlayerResult as BadRequestResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Theory]
        [InlineData("normal_name")]
        [InlineData("name with $ymb0ls!")]
        [InlineData("name with spaces")]
        [Trait("Controller", "CreateNewPlayer")]
        public void CreateNewPlayer_Valid_Test(string name)
        {
            UserCreatedPlayer player = new() { Name = name, Password = "randomPasswd" };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdResult = createNewPlayerResult as CreatedAtActionResult;

            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        [Trait("Controller", "GetPlayerById")]
        public void GetPlayerById_Invalid_Test()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };
            playerController.CreateNewPlayer(player);
            
            var getPlayerByIdResult = playerController.GetPlayerById(PlayerServiceUtil.MAX_PLAYER_NUMBER + 1).Result;
            var notFoundResult = getPlayerByIdResult as NotFoundResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        [Trait("Controller", "GetPlayerById")]
        public void GetPlayerById_Valid_Test()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };
            
            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;
            var getPlayerByIdValue = playerController.GetPlayerById(createdPlayer.Id).Value;

            Assert.NotNull(getPlayerByIdValue);
            Assert.IsType<Player>(getPlayerByIdValue);
            Assert.Equal(createdPlayer.Id, getPlayerByIdValue.Id);
        }

        [Fact]
        [Trait("Controller", "UpdatePlayerById")]
        public void UpdatePlayerById_Valid_Test()
        {
            UserCreatedPlayer player = new() { Name = "old_player_name", Password = "randomPasswd" };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;
            
            Player updatedPlayer = new() { Id = 999, Name = "new_player_name", Score = 999};
            var updatePlayerByIdResult = playerController.UpdatePlayerById(createdPlayer.Id, updatedPlayer);
            var okResult = updatePlayerByIdResult as OkResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        [Trait("Controller", "UpdatePlayerById")]
        public void UpdatePlayerById_InvalidId_Test()
        {
            UserCreatedPlayer player = new() { Name = "old_player_name", Password = "randomPasswd" };
            _ = playerController.CreateNewPlayer(player);

            Player updatedPlayer = new() { Id = 1, Name = "new_player_name" };
            var updatePlayerByIdResult = playerController.UpdatePlayerById(PlayerServiceUtil.MAX_PLAYER_NUMBER + 1, updatedPlayer);
            var notFoundResult = updatePlayerByIdResult as NotFoundResult;
            
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        [Trait("Controller", "UpdatePlayerById")]
        public void UpdatePlayerById_InvalidName_Test(string name)
        {
            UserCreatedPlayer player = new() { Name = "old_player_name", Password = "randomPasswd" };
            
            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;
            
            Player updatedPlayer = new() { Id = 999, Name = name, Score = 999 };
            var updatePlayerByIdResult = playerController.UpdatePlayerById(createdPlayer.Id, updatedPlayer);
            var badRequestResult = updatePlayerByIdResult as BadRequestResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        [Trait("Controller", "DeletePlayerById")]
        public void DeletePlayerById_Valid_Test()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;

            var deletePlayerByIdResult = playerController.DeletePlayerById(createdPlayer.Id);
            var okResult = deletePlayerByIdResult as OkResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        [Trait("Controller", "DeletePlayerById")]
        public void DeletePlayerById_InvalidPlayer_Test()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd"};
            playerController.CreateNewPlayer(player);

            var deletePlayerByIdResult = playerController.DeletePlayerById(PlayerServiceUtil.MAX_PLAYER_NUMBER + 1);
            var notFoundResult = deletePlayerByIdResult as NotFoundResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}