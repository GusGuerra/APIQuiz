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

        [HttpGet("{id}")]
        public IActionResult GetAnswer(int id, PlayerAnswer playerAnswer)
        {
            if (!playerService.Exists(id))
            {
                return NotFound();
            }

            if (!gameService.HasActiveQuestion(id))
            {
                return BadRequest();
            }

            var player = playerService.Get(id);
            if (gameService.ComparePlayerAnswer(playerAnswer.Answer, id))
            {
                player.IncreaseScore(PlayerServiceUtil.POINTS_PER_CORRECT_ANSWER);

                return Ok(new string(@"Correct! :)"));
            }

            player.IncreaseScore(PlayerServiceUtil.POINTS_PER_INCORRECT_ANSWER);
            return Ok(new string(@"Incorrect! :("));
        }

        [HttpGet]
        public async Task<IActionResult> View(
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
