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
    public class QuestionService
    {
        private readonly HttpClient client;
        private List<Question> questions;
        private string Token { get; set; }
        private int NextId {get; set;}
        public QuestionService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(QuestionServiceUtil.BaseURL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            questions = new List<Question>();
            Token = null;
            NextId = 1;
        }

        /// <summary>
        /// Gets new questions from the external API, initializes and stores them.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private async Task GetNew(int amount = QuestionServiceUtil.DEFAULT_NEW_QUESTION_AMOUNT)
        {
            await CheckAndRetrieveToken();

            string uri = QuestionServiceUtil.QuestionPath;
            uri += "?" + QuestionServiceUtil.QuestionAmountParam + "=" + amount.ToString();
            uri += "&" + QuestionServiceUtil.TokenParam + "=" + Token;

            var responseStream = client.GetStreamAsync(uri);
            Response externalResponse = await JsonSerializer.DeserializeAsync<Response>(await responseStream);
            
            var newQuestions = externalResponse.Result.ToList();
            newQuestions.ForEach(q => q.Id = GetAndUpdateNextId());
            newQuestions.ForEach(q => q.SeenBy = new List<int>());

            questions.AddRange(newQuestions);
        }

        /// <summary>
        /// Uses one of the questions from the database that the player haven't seen.
        /// If there are none, gets new questions from the external API and calls itself again.
        /// </summary>
        /// <param name="activePlayerId"></param>
        /// <returns>A question never before seen by the active player</returns>
        public async Task<Question> ViewNew(int activePlayerId)
        {
            if (questions.Any() && questions.FirstOrDefault(q => !q.SeenBy.Contains(activePlayerId)) != null)
            {
                Question question = questions.FirstOrDefault(q => !q.SeenBy.Contains(activePlayerId));
                question.SeenBy.Add(activePlayerId);
                return question;
            }

            await GetNew(QuestionServiceUtil.DEFAULT_NEW_QUESTION_AMOUNT);

            return await ViewNew(activePlayerId);
        }

        /// <summary>
        /// Checks if a Token is needed and requests it from the external API.
        /// </summary>
        /// <returns></returns>
        private async Task CheckAndRetrieveToken()
        {
            if (Token != null)
                return;

            string uri = QuestionServiceUtil.TokenPath;
            uri += "?" + QuestionServiceUtil.TokenCommandParam + "=request";

            var responseStream = client.GetStreamAsync(uri);
            var response = await JsonSerializer.DeserializeAsync<Response>(await responseStream);
            Token = response.Token;
        }
        
        private int GetAndUpdateNextId()
        {
            return NextId++;
        }
    }
}
