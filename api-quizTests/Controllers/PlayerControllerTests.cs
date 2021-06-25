using Xunit;
using APIQuiz.Controllers;
using APIQuiz.Services;
using APIQuiz.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIQuiz.Controllers.Tests
{
    public class PlayerControllerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [Trait("Controller", "CreateNewPlayer")]
        public void CreateNewPlayer_InvalidName_Test(string name)
        {
            PlayerService.ClearPlayerServiceClass();
            PlayerController playerController = new() { };
            Player player = new() { Name = name};

            var createNewPlayerResult = playerController.CreateNewPlayer(player);
            var badRequestResult = createNewPlayerResult as BadRequestResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact()]
        [Trait("Controller", "CreateNewPlayer")]
        public void CreateNewPlayer_Valid_Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        [Trait("Controller", "GetPlayerById")]
        public void GetPlayerById_Invalid_Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        [Trait("Controller", "GetPlayerById")]
        public void GetPlayerById_Valid_Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        [Trait("Controller", "UpdatePlayerById")]
        public void UpdatePlayerById_Valid_Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        [Trait("Controller", "UpdatePlayerById")]
        public void UpdatePlayerById_InvalidId_Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        [Trait("Controller", "UpdatePlayerById")]
        public void UpdatePlayerById_InvalidName_Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        [Trait("Controller", "UpdatePlayerById")]
        public void UpdatePlayerById_InvalidScore_Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        [Trait("Controller", "UpdatePlayerById")]
        public void UpdatePlayerById_InvalidPlayer_Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        [Trait("Controller", "DeletePlayerById")]
        public void DeletePlayerById_Valid_Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        [Trait("Controller", "DeletePlayerById")]
        public void DeletePlayerById_InvalidPlayer_Test()
        {
            Assert.True(false, "This test needs an implementation");
        }
    }
}