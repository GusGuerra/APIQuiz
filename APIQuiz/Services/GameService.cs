using System;
using System.Collections.Generic;
using System.Linq;
using APIQuiz.Models;
using APIQuiz.Util;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace APIQuiz.Services
{
    /// <summary>
    /// Encapsulates the methods that interact with the Open Trivia DB API (external API)
    /// </summary>
    public class GameService
    {
        private static readonly GameService GameServiceInstance = new();
        public static GameService Singleton() => GameServiceInstance;
        private readonly HttpClient client;
        private List<Question> questions;
        private Dictionary<int, Question> activeQuestion;
        private string Token { get; set; }
        private int NextId {get; set;}
        private GameService()
        {
            client = new HttpClient();
            
            UriBuilder uriBuilder = new();
            uriBuilder.Scheme = GameServiceUtil.EXTERNAL_API_SCHEME;
            uriBuilder.Host = GameServiceUtil.EXTERNAL_API_HOST;
            client.BaseAddress = uriBuilder.Uri;

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            questions = new List<Question>();
            activeQuestion = new Dictionary<int, Question>();
            Token = null;
            NextId = 1;
        }

        /// <summary>
        /// Gets new questions from the external API, initializes and stores them.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private async Task GetNew(int amount = GameServiceUtil.DEFAULT_NEW_QUESTION_AMOUNT)
        {
            await CheckAndRetrieveToken();

            string uri = GameServiceUtil.EXTERNAL_API_PATH_QUESTION;
            uri = GameServiceUtil.AddQuery(uri, GameServiceUtil.EXTERNAL_API_KEY_QUESTION_AMOUNT, amount.ToString());
            uri = GameServiceUtil.AddQuery(uri, GameServiceUtil.EXTERNAL_API_KEY_TOKEN, Token);

            var responseStream = await client.GetStreamAsync(uri);
            Response externalResponse = await JsonSerializer.DeserializeAsync<Response>(responseStream);

            var newQuestions = externalResponse.Result.ToList();
            newQuestions.ForEach(q => q.Id = GetAndUpdateNextId());
            newQuestions.ForEach(q => q.SeenBy = new List<int>());

            questions.AddRange(newQuestions);
        }

        /// <summary>
        /// Uses one of the questions from the database that the player haven't seen.
        /// If there are none, gets new questions from the external API.
        /// </summary>
        /// <param name="activePlayerId"></param>
        /// <returns>A question never before seen by the active player</returns>
        public async Task<Question> ViewNew(int activePlayerId, bool isNewQuestion = GameServiceUtil.DEFAULT_NEW_QUESTION_OPTION)
        {
            if (!isNewQuestion)
            {
                return activeQuestion[activePlayerId];
            }

            if (!questions.Any() || questions.FirstOrDefault(q => !q.SeenBy.Contains(activePlayerId)) == null)
            {
                await GetNew();
            }

            Question question = questions.FirstOrDefault(q => !q.SeenBy.Contains(activePlayerId));
            question.SeenBy.Add(activePlayerId);

            if (!activeQuestion.ContainsKey(activePlayerId))
            {
                activeQuestion.Add(activePlayerId, question);
            }
            
            activeQuestion[activePlayerId] = question;

            return question;
        }

        /// <summary>
        /// Compares the answer given by the player with the correct answer
        /// </summary>
        /// <param name="playerAnswer"></param>
        /// <param name="player"></param>
        /// <returns>True if the player's answer is correct. Otherwise, false.</returns>
        public bool ComparePlayerAnswer(string playerAnswer, int playerId)
        {
            Question question = activeQuestion[playerId];
            bool verdict = (question.CorrectAnswer == playerAnswer);
            activeQuestion[playerId] = null;
            return verdict;
        }

        /// <summary>
        /// Checks if the player has an active question
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>True if there is an active question for the player. Otherwise, false.</returns>
        public bool HasActiveQuestion(int playerId)
        {
            if (!activeQuestion.ContainsKey(playerId))
                return false;

            return activeQuestion[playerId] != null;
        }

        /// <summary>
        /// Checks if a Token is needed and requests it from the external API.
        /// </summary>
        /// <returns></returns>
        private async Task CheckAndRetrieveToken()
        {
            if (Token != null)
                return;

            string uri = GameServiceUtil.EXTERNAL_API_PATH_TOKEN;
            uri = GameServiceUtil.AddQuery(
                uri,
                GameServiceUtil.EXTERNAL_API_KEY_COMMAND,
                GameServiceUtil.EXTERNAL_API_VALUE_TOKEN_REQUEST);

            var responseStream = await client.GetStreamAsync(uri);
            var response = await JsonSerializer.DeserializeAsync<Response>(responseStream);

            Token = response.Token;
        }

        private int GetAndUpdateNextId()
        {
            return NextId++;
        }
    }
}
