using APIQuiz.Models;
using APIQuiz.Util;
using Microsoft.AspNetCore.Mvc;
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
        [Trait("PlayerController", "CreateNewPlayer")]
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
        [InlineData("you can't use spaces")]
        [Trait("PlayerController", "CreateNewPlayer")]
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
        [Trait("PlayerController", "CreateNewPlayer")]
        public void CreateNewPlayer_Valid_Test(string name)
        {
            UserCreatedPlayer player = new() { Name = name, Password = "randomPasswd" };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdResult = createNewPlayerResult as CreatedAtActionResult;

            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        [Trait("PlayerController", "GetPlayerById")]
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
        [Trait("PlayerController", "GetPlayerById")]
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
        [Trait("PlayerController", "UpdatePlayerById")]
        public void UpdatePlayerById_Valid_Test()
        {
            UserCreatedPlayer player = new() { Name = "old_player_name", Password = "randomPasswd" };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;
            
            UserCreatedPlayer updatedPlayer = new() { Name = "new_player_name", Password = "randomPasswd"};
            var updatePlayerByIdResult = playerController.UpdatePlayerById(createdPlayer.Id, updatedPlayer);
            var okResult = updatePlayerByIdResult as OkResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        [Trait("PlayerController", "UpdatePlayerById")]
        public void UpdatePlayerById_InvalidId_Test()
        {
            UserCreatedPlayer player = new() { Name = "old_player_name", Password = "randomPasswd" };
            _ = playerController.CreateNewPlayer(player);

            UserCreatedPlayer updatedPlayer = new() { Name = "new_player_name", Password = "randomPasswd" };
            var updatePlayerByIdResult = playerController.UpdatePlayerById(PlayerServiceUtil.MAX_PLAYER_NUMBER + 1, updatedPlayer);
            var notFoundResult = updatePlayerByIdResult as NotFoundResult;
            
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        [Trait("PlayerController", "UpdatePlayerById")]
        public void UpdatePlayerById_InvalidName_Test(string name)
        {
            UserCreatedPlayer player = new() { Name = "old_player_name", Password = "randomPasswd" };
            
            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;

            UserCreatedPlayer updatedPlayer = new() { Name = name, Password = "randomPasswd" };
            var updatePlayerByIdResult = playerController.UpdatePlayerById(createdPlayer.Id, updatedPlayer);
            var badRequestResult = updatePlayerByIdResult as BadRequestResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        [Trait("PlayerController", "UpdatePlayerById")]
        public void UpdatePlayerById_InvalidPassword_Test()
        {
            UserCreatedPlayer player = new() { Name = "old_player_name", Password = "correctPasswd" };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;

            UserCreatedPlayer updatedPlayer = new() { Name = "new_player_name", Password = "incorrectPasswd" };
            var updatePlayerByIdResult = playerController.UpdatePlayerById(createdPlayer.Id, updatedPlayer);
            var forbiddenResult = updatePlayerByIdResult as StatusCodeResult;

            Assert.NotNull(forbiddenResult);
            Assert.Equal(403, forbiddenResult.StatusCode);
        }

        [Fact]
        [Trait("PlayerController", "DeletePlayer")]
        public void DeletePlayer_Valid_Test()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;

            var deletePlayerResult = playerController.DeletePlayer(createdPlayer.Id, player);
            var okResult = deletePlayerResult as OkResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        [Trait("PlayerController", "DeletePlayer")]
        public void DeletePlayer_InvalidPlayer_Test()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd"};
            _ = playerController.CreateNewPlayer(player);

            var deletePlayerResult = playerController.DeletePlayer(PlayerServiceUtil.MAX_PLAYER_NUMBER + 1, player);
            var notFoundResult = deletePlayerResult as NotFoundResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        [Trait("PlayerController", "DeletePlayer")]
        public void DeletePlayer_InvalidPassword_Test()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "correctPasswd" };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdAtActionResult = createNewPlayerResult as CreatedAtActionResult;
            var createdPlayer = createdAtActionResult.Value as Player;

            UserCreatedPlayer wrongPasswordPlayer = new() { Name = player.Name, Password = "incorrectPasswd" };

            var deletePlayerResult = playerController.DeletePlayer(createdPlayer.Id, wrongPasswordPlayer);
            var forbiddenResult = deletePlayerResult as StatusCodeResult;

            Assert.NotNull(forbiddenResult);
            Assert.Equal(403, forbiddenResult.StatusCode);
        }
    }
}