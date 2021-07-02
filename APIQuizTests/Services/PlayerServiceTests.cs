using Xunit;
using APIQuiz.Util;
using APIQuiz.Models;
using System.Collections.Generic;

namespace APIQuiz.Services.Tests
{
    public class PlayerServiceTests
    {
        private PlayerService playerService;

        public PlayerServiceTests()
        {
            playerService = PlayerService.Singleton();
        }

        [Theory]
        [InlineData(PlayerServiceUtil.MAX_PLAYERS_PER_PAGE - 1)]
        [InlineData(PlayerServiceUtil.MAX_PLAYERS_PER_PAGE)]
        [InlineData(PlayerServiceUtil.MAX_PLAYERS_PER_PAGE * 2)]
        [InlineData(PlayerServiceUtil.MAX_PLAYERS_PER_PAGE * 3 - 1)]
        [InlineData(PlayerServiceUtil.MAX_PLAYERS_PER_PAGE * 3)]
        [InlineData(PlayerServiceUtil.MAX_PLAYERS_PER_PAGE * 3 + 1)]
        [Trait("PlayerService", "GetAll")]
        public void GetAll_Simple_Test(int playerListSize)
        {
            for (int i = 0; i < playerListSize; i++)
            {
                Player player = new() { Name = "new_player_name" };
                playerService.Create(player);
            }

            int minimumNumberOfPages = PlayerServiceUtil.HighestPageNumberCalculation(playerListSize);

            for (int i = 1; i <= minimumNumberOfPages; i++)
            {
                var getAllResult = playerService.GetAll(i);
                Assert.True(getAllResult.Count <= PlayerServiceUtil.MAX_PLAYERS_PER_PAGE);
            }
        }

        [Fact]
        [Trait("PlayerService", "Get")]
        public void Get_ValidId_Test()
        {
            Player player = new() { Name = "new_player_name" };
            playerService.Create(player);

            Player getResult = playerService.Get(player.Id);
            Assert.NotNull(getResult);
            Assert.Equal(player.Id, getResult.Id);
        }

        [Fact]
        [Trait("PlayerService", "Get")]
        public void Get_InvalidId_Test()
        {
            Player player = new() { Name = "new_player_name" };
            playerService.Create(player);

            Player getResult = playerService.Get(PlayerServiceUtil.MAX_PLAYER_NUMBER);
            Assert.Null(getResult);
        }

        [Fact]
        [Trait("PlayerService", "Create")]
        public void Create_Simple_Test()
        {
            Player player = new() { Name = "new_player_name" };
            playerService.Create(player);

            var getResult = playerService.Get(player.Id);
            Assert.NotNull(getResult);
            Assert.Equal(player, getResult);
        }

        [Fact]
        [Trait("PlayerService", "Create")]
        public void Create_InvalidId_Test()
        {
            Player player = new() { Id = PlayerServiceUtil.MAX_PLAYER_NUMBER, Name = "new_player_name" };
            playerService.Create(player);

            Assert.NotNull(player);
            Assert.NotEqual(PlayerServiceUtil.MAX_PLAYER_NUMBER, player.Id);
        }

        [Fact]
        [Trait("PlayerService", "Create")]
        public void Create_InvalidScore_Test()
        {
            Player player = new() { Name = "new_player_name", Score = 100 };
            playerService.Create(player);

            var getResult = playerService.Get(player.Id);
            
            Assert.Equal(0, getResult.Score);
        }

        [Fact]
        [Trait("PlayerService", "Delete")]
        public void Delete_Simple_Test()
        {
            Player player = new() { Name = "new_player_name" };
            playerService.Create(player);

            Assert.True(playerService.Exists(player.Id));

            playerService.Delete(player.Id);

            Assert.Null(playerService.Get(player.Id));
            Assert.False(playerService.Exists(player.Id));
        }

        [Fact]
        [Trait("PlayerService", "Update")]
        public void Update_Simple_Test()
        {
            Player player = new() { Name = "old_player_name" };
            playerService.Create(player);

            Player updatedPlayer = new() { Id = 10, Name = "new_player_name", Score = 999 };
            playerService.Update(player.Id, updatedPlayer);

            var getResult = playerService.Get(player.Id);
            Assert.Equal(updatedPlayer.Name, getResult.Name);
            Assert.Equal(0, getResult.Score);
            Assert.Equal(player.Id, getResult.Id);
        }

        [Fact]
        [Trait("PlayerService", "Exists")]
        public void Exists_True_Test()
        {
            Player player = new() { Name = "new_player_name" };
            playerService.Create(player);

            Assert.True(playerService.Exists(player.Id));
        }

        [Theory]
        [InlineData(24)]
        [InlineData(57)]
        [Trait("PlayerService", "Exists")]
        public void Exists_False_Test(int playerListSize)
        {
            for (int i = 0; i < playerListSize; i++)
            {
                Player player = new() { Name = "new_player_name" };
                playerService.Create(player);
            }

            Assert.False(playerService.Exists(PlayerServiceUtil.MAX_PLAYER_NUMBER));
        }

        [Theory]
        [InlineData(".")]
        [InlineData("a")]
        [InlineData("name with spaces")]
        [Trait("PlayerService", "HasValidName")]
        public void HasValidName_True_Test(string name)
        {
            Player player = new() { Name = name };
            Assert.True(PlayerService.HasValidName(player));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [Trait("PlayerService", "HasValidName")]
        public void HasValidName_False_Test(string name)
        {
            Player player = new() { Name = name };
            Assert.False(PlayerService.HasValidName(player));
        }
    }
}
