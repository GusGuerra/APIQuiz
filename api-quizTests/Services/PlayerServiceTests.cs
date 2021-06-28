using Xunit;
using APIQuiz.Services;
using APIQuiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIQuiz.Services.Tests
{
    public class PlayerServiceTests
    {
        private PlayerService playerService;

        public PlayerServiceTests()
        {
            playerService = new();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [Trait("PlayerService", "GetAll")]
        public void GetAll_Simple_Test(int playerListSize)
        {
            for (int i = 0; i < playerListSize; i++)
            {
                Player player = new() { Name = "new_player_name" };
                playerService.Create(player);
            }

            Assert.Equal(playerListSize, playerService.GetAll().Count);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 2)]
        [InlineData(7, 5)]
        [Trait("PlayerService", "Get")]
        public void Get_ValidId_Test(int playerListSize, int id)
        {
            for (int i = 0; i < playerListSize; i++)
            {
                Player player = new() { Name = "new_player_name" };
                playerService.Create(player);
            }

            Player getResult = playerService.Get(id);
            Assert.NotNull(getResult);
            Assert.Equal(id, getResult.Id);
        }

        [Theory]
        [InlineData(2, 4)]
        [InlineData(5, 7)]
        [Trait("PlayerService", "Get")]
        public void Get_InvalidId_Test(int playerListSize, int id)
        {
            for (int i = 0; i < playerListSize; i++)
            {
                Player player = new() { Name = "new_player_name" };
                playerService.Create(player);
            }

            Player getResult = playerService.Get(id);
            Assert.Null(getResult);
        }

        [Fact]
        [Trait("PlayerService", "Create")]
        public void Create_Simple_Test()
        {
            Player player = new() { Name = "new_player_name" };
            playerService.Create(player);

            var players = playerService.GetAll();
            var getResult = playerService.Get(player.Id);
            Assert.NotNull(getResult);
            Assert.Single(players);
            Assert.Equal(player, players[0]);
            Assert.Equal(player, getResult);
        }

        [Fact]
        [Trait("PlayerService", "Create")]
        public void Create_InvalidId_Test()
        {
            Player player = new() { Id = 10, Name = "new_player_name" };
            playerService.Create(player);

            var players = playerService.GetAll();
            Assert.Single(players);
            Assert.Equal(1, players[0].Id);
        }

        [Fact]
        [Trait("PlayerService", "Create")]
        public void Create_InvalidScore_Test()
        {
            Player player = new() { Name = "new_player_name", Score = 100 };
            playerService.Create(player);

            var players = playerService.GetAll();
            Assert.Single(players);
            Assert.Equal(0, players[0].Score);
        }

        [Fact]
        [Trait("PlayerService", "Delete")]
        public void Delete_Simple_Test()
        {
            Player player = new() { Name = "new_player_name" };
            playerService.Create(player);

            var players = playerService.GetAll();
            Assert.Single(players);

            playerService.Delete(player.Id);

            Assert.Empty(players);
            Assert.Null(playerService.Get(player.Id));
        }

        [Fact()]
        [Trait("PlayerService", "Update")]
        public void Update_Simple_Test()
        {
            Player player = new() { Name = "old_player_name" };
            playerService.Create(player);

            Player updatedPlayer = new() { Id = 1, Name = "new_player_name" };
            playerService.Update(updatedPlayer);

            var players = playerService.GetAll();
            var getResult = playerService.Get(player.Id);
            Assert.Single(players);
            Assert.Equal(updatedPlayer.Name, getResult.Name);
            Assert.Equal(updatedPlayer.Name, players[0].Name);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 2)]
        [InlineData(7, 5)]
        [Trait("PlayerService", "Exists")]
        public void Exists_True_Test(int playerListSize, int id)
        {
            for (int i = 0; i < playerListSize; i++)
            {
                Player player = new() { Name = "new_player_name" };
                playerService.Create(player);
            }

            Assert.True(playerService.Exists(id));
        }

        [Theory]
        [InlineData(2, 4)]
        [InlineData(5, 7)]
        [Trait("PlayerService", "Exists")]
        public void Exists_False_Test(int playerListSize, int id)
        {
            for (int i = 0; i < playerListSize; i++)
            {
                Player player = new() { Name = "new_player_name" };
                playerService.Create(player);
            }

            Assert.False(playerService.Exists(id));
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
