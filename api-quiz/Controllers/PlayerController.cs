using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using APIQuiz.Models;
using APIQuiz.Services;

namespace APIQuiz.Controllers
{
    [ApiController]
    [Route("apiQuiz/[controller]")]
    public class PlayerController : ControllerBase
    {
        public PlayerController()
        {
        }

        /// <summary>
        /// Creates a new player from request body and inserts it in the list
        /// </summary>
        /// <param name="player"></param>
        /// <returns> Created object in json format </returns>
        [HttpPost]
        public IActionResult CreateNewPlayer(Player player)
        {
            if (!PlayerService.HasValidName(player))
                return BadRequest();

            PlayerService.Create(player);
            return CreatedAtAction(nameof(CreateNewPlayer), new { id = player.Id }, player);
        }

        /// <summary>
        /// Retrieves information about the player with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Player object with specified id </returns>
        [HttpGet("{id}")]
        public ActionResult<Player> GetPlayerById(int id)
        {
            if (!PlayerService.Exists(id))
                return NotFound();

            return PlayerService.Get(id);
        }

        /// <summary>
        /// Lists all active players
        /// </summary>
        /// <returns> List of active players in json format </returns>
        [HttpGet]
        public ActionResult<List<Player>> GetAllPlayers() => PlayerService.GetAll();

        /// <summary>
        /// Verify invalid requests and updates the player with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedPlayer"></param>
        /// <returns> No content </returns>
        [HttpPut("{id}")]
        public IActionResult UpdatePlayerById(int id, Player updatedPlayer)
        {
            if (id != updatedPlayer.Id || !PlayerService.HasValidName(updatedPlayer))
                return BadRequest();

            if (!PlayerService.Exists(id))
                return NotFound();
            
            var existingPlayer = PlayerService.Get(id);
            if (existingPlayer.Score != updatedPlayer.Score)
                return BadRequest();
            
            PlayerService.Update(updatedPlayer);

            return Ok();
        }

        /// <summary>
        /// Verifies invalid deletion and removes the player with specified id from the list
        /// </summary>
        /// <param name="id"></param>
        /// <returns> No content </returns>
        [HttpDelete("{id}")]
        public IActionResult DeletePlayerById(int id)
        {
            if (!PlayerService.Exists(id))
                return NotFound();

            PlayerService.Delete(id);

            return Ok();
        }
    }
}
