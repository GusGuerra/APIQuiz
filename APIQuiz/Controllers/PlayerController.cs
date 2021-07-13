using Microsoft.AspNetCore.Mvc;
using APIQuiz.Models;
using APIQuiz.Services;
using APIQuiz.Util;

namespace APIQuiz.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class PlayerController : ControllerBase
    {
        private PlayerService playerService;
        public PlayerController()
        {
            playerService = PlayerService.Singleton();
        }

        /// <summary>
        /// Creates a new player from request body and inserts it in the list
        /// </summary>
        /// <param name="player"></param>
        /// <returns> Created object in json format </returns>
        [HttpPost]
        public IActionResult CreateNewPlayer(UserCreatedPlayer userCreatedPlayer)
        {
            if (!PlayerServiceUtil.IsValidName(userCreatedPlayer.Name))
            {
                return BadRequest();
            }
            
            if (!PlayerServiceUtil.IsValidPassword(userCreatedPlayer.Password))
            {
                return BadRequest();
            }

            int playerId = playerService.Create(userCreatedPlayer);
            Player player = playerService.Get(playerId);

            return CreatedAtAction(nameof(CreateNewPlayer), new { id = playerId }, player);
        }

        /// <summary>
        /// Retrieves information about the player with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Player object with specified id </returns>
        [HttpGet("{id}")]
        public ActionResult<Player> GetPlayerById(int id)
        {
            return !playerService.Exists(id) ? NotFound() : playerService.Get(id);
        }

        /// <summary>
        /// Lists all active players
        /// </summary>
        /// <returns> List of active players in json format </returns>
        [HttpGet]
        public IActionResult GetAllPlayers([FromQuery] int page = PlayerServiceUtil.DEFAULT_PAGE_NUMBER)
        {
            return Ok(playerService.GetAll(page));
        }

        /// <summary>
        /// Verify invalid requests and updates the player with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedPlayer"></param>
        /// <returns> No content </returns>
        [HttpPut("{id}")]
        public IActionResult UpdatePlayerById(int id, UserCreatedPlayer updatedPlayer)
        {
            if (!playerService.Exists(id))
            {
                return NotFound();
            }

            // TODO: Check password
            
            if (!PlayerServiceUtil.IsValidName(updatedPlayer.Name))
            {
                return BadRequest();
            }

            playerService.Update(id, updatedPlayer);

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
            if (!playerService.Exists(id))
            {
                return NotFound();
            }

            playerService.Delete(id);

            return Ok();
        }
    }
}
