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
        [Trait("Controller", "CreateNewPlayer")]
        public void CreateNewPlayer_InvalidName_Test(string name)
        {
            Player player = new() { Name = name};

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
            Player player = new() { Name = name };

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var createdResult = createNewPlayerResult as CreatedAtActionResult;

            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        [Trait("Controller", "GetPlayerById")]
        public void GetPlayerById_Invalid_Test()
        {
            Player player = new() { Name = "new_player_name" };
            playerController.CreateNewPlayer(player);
            
            var getPlayerByIdResult = playerController.GetPlayerById(PlayerServiceUtil.MAX_PLAYER_NUMBER + 1).Result;
            var notFoundResult = getPlayerByIdResult as NotFoundResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 2)]
        [InlineData(7, 5)]
        [Trait("Controller", "GetPlayerById")]
        public void GetPlayerById_Valid_Test(int playerListSize, int id)
        {
            for (int i = 0; i < playerListSize; i++)
            {
                Player player = new() { Name = "new_player_name" };
                playerController.CreateNewPlayer(player);
            }

            var getPlayerByIdValue = playerController.GetPlayerById(id).Value;

            Assert.NotNull(getPlayerByIdValue);
            Assert.IsType<Player>(getPlayerByIdValue);
            Assert.Equal(id, getPlayerByIdValue.Id);
        }

        [Fact]
        [Trait("Controller", "UpdatePlayerById")]
        public void UpdatePlayerById_Valid_Test()
        {
            Player player = new() { Name = "old_player_name" };
            playerController.CreateNewPlayer(player);

            Player updatedPlayer = new() { Id = 999, Name = "new_player_name", Score = 999};
            var updatePlayerByIdResult = playerController.UpdatePlayerById(player.Id, updatedPlayer);
            var okResult = updatePlayerByIdResult as OkResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        [Trait("Controller", "UpdatePlayerById")]
        public void UpdatePlayerById_InvalidId_Test()
        {
            Player player = new() { Name = "old_player_name" };
            playerController.CreateNewPlayer(player);

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
            Player player = new() { Name = "old_player_name" };
            playerController.CreateNewPlayer(player);

            Player updatedPlayer = new() { Id = 999, Name = name, Score = 999 };
            var updatePlayerByIdResult = playerController.UpdatePlayerById(player.Id, updatedPlayer);
            var badRequestResult = updatePlayerByIdResult as BadRequestResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        [Trait("Controller", "DeletePlayerById")]
        public void DeletePlayerById_Valid_Test()
        {
            Player player = new() { Name = "new_player_name" };
            playerController.CreateNewPlayer(player);

            var deletePlayerByIdResult = playerController.DeletePlayerById(player.Id);
            var okResult = deletePlayerByIdResult as OkResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        [Trait("Controller", "DeletePlayerById")]
        public void DeletePlayerById_InvalidPlayer_Test()
        {
            Player player = new() { Name = "new_player_name" };
            playerController.CreateNewPlayer(player);

            var deletePlayerByIdResult = playerController.DeletePlayerById(PlayerServiceUtil.MAX_PLAYER_NUMBER + 1);
            var notFoundResult = deletePlayerByIdResult as NotFoundResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}