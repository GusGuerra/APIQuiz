using System;
using System.Collections.Generic;
using System.Linq;
using APIQuiz.Models;
using APIQuiz.Util;

namespace APIQuiz.Services
{
    public class PlayerService
    {
        private static readonly PlayerService PlayerServiceInstance = new();
        public static PlayerService Singleton() => PlayerServiceInstance;
        private List<Player> Players { get; }
        private Dictionary<int, string> Passwords { get; }
        private int NextId = 1;
        private PlayerService()
        {
            Players = new List<Player>();
            Passwords = new Dictionary<int, string>();
        }

        /// <summary>
        /// Lists the specified page from the list of all active players
        /// </summary>
        /// <returns>
        /// List of all active players
        /// </returns>
        public List<Player> GetAll(int page)
        {
            page = PlayerServiceUtil.PageNumberAdjustment(page, Players.Count);

            int firstIndexInPage = (page - 1) * PlayerServiceUtil.MAX_PLAYERS_PER_PAGE;
            int playerAmount = PlayerServiceUtil.PlayerAmountCalculation(firstIndexInPage, Players.Count);

            return Players.GetRange(firstIndexInPage, playerAmount);
        }

        /// <summary>
        /// Finds player with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Player object with matching id or null if not found
        /// </returns>
        public Player Get(int id) => Players.FirstOrDefault(p => p.Id == id);

        /// <summary>
        /// Adds a new player to the active player list
        /// </summary>
        /// <param name="userCreatedPlayer"></param>
        /// <returns> The id of the created player </returns>
        public int Create(UserCreatedPlayer userCreatedPlayer)
        {
            Player player = userCreatedPlayer.GeneratePlayer();

            player.Id = GetAndUpdateNextId();
            player.Score = 0;

            Passwords.Add(player.Id, userCreatedPlayer.Password);
            Players.Add(player);

            return player.Id;
        }

        /// <summary>
        /// Removes a player from the list
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var player = Get(id);
            Players.Remove(player);
        }

        /// <summary>
        /// Updates a player's name 
        /// </summary>
        /// <param name="player"></param>
        public void Update(int id, UserCreatedPlayer updatedPlayer)
        {
            var index = Players.FindIndex(p => p.Id == id);

            Player player = updatedPlayer.GeneratePlayer();

            Players[index].CopyDataFrom(player);
        }

        /// <summary>
        /// Builds and returns the specified page of the player ranking
        /// </summary>
        /// <param name="page"></param>
        /// <returns>A list of players ordered by highest score</returns>
        public List<Player> GetRanking(int page)
        {
            List<Player> rankingPage = new(Players);
            rankingPage.Sort((p1, p2) => p1.Score > p2.Score ? -1 : p1.Score == p2.Score ? 0 : 1);

            page = PlayerServiceUtil.PageNumberAdjustment(page, Players.Count);

            int firstIndexInPage = (page - 1) * PlayerServiceUtil.MAX_PLAYERS_PER_PAGE;
            int playerAmount = PlayerServiceUtil.PlayerAmountCalculation(firstIndexInPage, Players.Count);

            return rankingPage.GetRange(firstIndexInPage, playerAmount);
        }

        /// <summary>
        /// Compares the user input password with the actual player password
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="userInput"></param>
        /// <returns> True if the passwords match; Otherwise, false </returns>
        public bool CheckPassword(int playerId, string userInput)
        {
            return Passwords[playerId] == userInput;
        }

        /// <summary>
        /// Gets the next id and updates its value
        /// </summary>
        /// <returns>Next id to be used by a player</returns>
        public int GetAndUpdateNextId()
        {
            return NextId++;
        }

        /// <summary>
        /// Gets the next id
        /// </summary>
        /// <returns>Next id to be used by a player</returns>
        public int GetNextId()
        {
            return NextId;
        }

        /// <summary>
        /// Check if a player exists in the active player list
        /// </summary>
        /// <param name="id"></param>
        /// <returns> True if a player with the specified id is found on the list; Otherwise, false. </returns>
        public bool Exists(int id)
        {
            var player = Get(id);
            return player != null;
        }
    }
}
