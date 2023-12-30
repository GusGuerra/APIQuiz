using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using APIQuiz.Models;
using APIQuiz.Services;
using APIQuiz.Util;
using System.Threading.Tasks;

namespace APIQuiz.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private PlayerService playerService;
        private GameService gameService;
        public GameController()
        {
            playerService = PlayerService.Singleton();
            gameService = GameService.Singleton();
        }

        /// <summary>
        /// Compares the player's answer with the atual answer and calls Player.IncreaseScore()
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="playerAnswer"></param>
        /// <returns> If the request is valid returns Ok(200) and a right/wrong guess message;
        /// Otherwise, the corresponding status code </returns>
        [HttpPost("{playerId}")]
        public IActionResult GuessAnswer(int playerId, PlayerAnswer playerAnswer)
        {
            if (!playerService.Exists(playerId))
            {
                return NotFound();
            }

            if (!playerService.CheckPassword(playerId, playerAnswer.Password))
            {
                return StatusCode(403);
            }

            if (!gameService.HasActiveQuestion(playerId))
            {
                return BadRequest();
            }

            var player = playerService.Get(playerId);
            if (gameService.ComparePlayerAnswer(playerAnswer.Answer, playerId))
            {
                player.IncreaseScore(PlayerServiceUtil.POINTS_PER_CORRECT_ANSWER);

                return Ok(new string(GameServiceUtil.CORRECT_ANSWER_MESSAGE));
            }

            player.IncreaseScore(PlayerServiceUtil.POINTS_PER_INCORRECT_ANSWER);
            return Ok(new string(GameServiceUtil.INCORRECT_ANSWER_MESSAGE));
        }

        [HttpGet]
        public async Task<IActionResult> View(
            UserCreatedPlayer player,
            [FromQuery] string view,
            [FromQuery] int page,
            [FromQuery] int id,
            [FromQuery] bool fetch = GameServiceUtil.DEFAULT_NEW_QUESTION_OPTION)
        {
            if (view == "question")
            {
                if (!playerService.Exists(id))
                {
                    return NotFound();
                }

                if (!playerService.CheckPassword(id, player.Password))
                {
                    return StatusCode(403);
                }

                if (!fetch && !gameService.HasActiveQuestion(id))
                {
                    return BadRequest();
                }

                PlayerFriendlyQuestion question = (await gameService.ViewNew(id, fetch)).GenerateUserFriendlyQuestion();
                return Ok(question);
            }
            else if (view == "ranking")
            {
                if (playerService.GetAll(PlayerServiceUtil.DEFAULT_PAGE_NUMBER).Count == 0)
                {
                    return NoContent();
                }

                return Ok(playerService.GetRanking(page));
            }

            return BadRequest();
        }
    }
}
