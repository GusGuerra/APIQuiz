using System.Collections.Generic;
using System.Linq;
using APIQuiz.Models;

namespace APIQuiz.Services
{
    public class PlayerService
    {
        private List<Player> Players { get; }
        private int NextId = 1;
        public PlayerService()
        {
            Players = new List<Player>();
        }

        /// <summary>
        /// Lists all active players
        /// </summary>
        /// <returns>
        /// List of all active players
        /// </returns>
        public List<Player> GetAll() => Players;

        /// <summary>
        /// Finds player with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Player object with matching id or null if not found
        /// </returns>
        public Player Get(int id) => Players.FirstOrDefault(p => p.Id == id);

        /// <summary>
        /// Adds a new player to the game
        /// </summary>
        /// <param name="player"></param>
        public void Create(Player player)
        {
            player.Id = GetAndUpdateNextId();
            player.Score = 0;

            Players.Add(player);
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
        public void Update(Player player)
        {
            var index = Players.FindIndex(p => p.Id == player.Id);
            Players[index] = player;
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

        /// <summary>
        /// Checks if the specified player has a valid name
        /// </summary>
        /// <param name="player"></param>
        /// <returns> True if the player has a valid name; Otherwise, false </returns>
        public static bool HasValidName(Player player)
        {
            return !string.IsNullOrEmpty(player.Name) && !string.IsNullOrWhiteSpace(player.Name);
        }
    }
}
