using Xunit;
using APIQuiz.Util;
using APIQuiz.Models;

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
                UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };
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
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };
            int playerId = playerService.Create(player);

            Player getResult = playerService.Get(playerId);
            Assert.NotNull(getResult);
            Assert.Equal(playerId, getResult.Id);
        }

        [Fact]
        [Trait("PlayerService", "Get")]
        public void Get_InvalidId_Test()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };
            _ = playerService.Create(player);

            Player getResult = playerService.Get(PlayerServiceUtil.MAX_PLAYER_NUMBER);
            Assert.Null(getResult);
        }

        [Fact]
        [Trait("PlayerService", "Create")]
        public void Create_Simple_Test()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };
            int playerId = playerService.Create(player);

            var getResult = playerService.Get(playerId);
            Assert.NotNull(getResult);
            Assert.Equal(player.Name, getResult.Name);
        }

        [Fact]
        [Trait("PlayerService", "Delete")]
        public void Delete_Simple_Test()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };
            int playerId = playerService.Create(player);

            Assert.True(playerService.Exists(playerId));

            playerService.Delete(playerId);

            Assert.Null(playerService.Get(playerId));
            Assert.False(playerService.Exists(playerId));
        }

        [Fact]
        [Trait("PlayerService", "Update")]
        public void Update_Simple_Test()
        {
            UserCreatedPlayer player = new() { Name = "old_player_name", Password = "randomPasswd"};
            int playerId = playerService.Create(player);

            UserCreatedPlayer updatedPlayer = new() { Name = "new_player_name", Password = "randomPasswd" };
            playerService.Update(playerId, updatedPlayer);

            var getResult = playerService.Get(playerId);

            Assert.Equal(updatedPlayer.Name, getResult.Name);
            Assert.Equal(playerId, getResult.Id);
        }

        [Fact]
        [Trait("PlayerService", "Exists")]
        public void Exists_True_Test()
        {
            UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };
            int playerId = playerService.Create(player);

            Assert.True(playerService.Exists(playerId));
        }

        [Theory]
        [InlineData(24)]
        [InlineData(57)]
        [Trait("PlayerService", "Exists")]
        public void Exists_False_Test(int playerListSize)
        {
            for (int i = 0; i < playerListSize; i++)
            {
                UserCreatedPlayer player = new() { Name = "new_player_name", Password = "randomPasswd" };
                playerService.Create(player);
            }

            Assert.False(playerService.Exists(PlayerServiceUtil.MAX_PLAYER_NUMBER));
        }
    }
}
