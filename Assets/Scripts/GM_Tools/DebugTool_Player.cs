using DialogueSystem.DialogueEditor;
using Player;
using UnityEngine;

namespace GM_Tools
{
    public class DebugTool_Player : MonoBehaviour
    {
        private PlayerRepository _playerRepository;

        private void OnValidate()
        {
            _playerRepository ??= new PlayerRepository();
        }

        public void ResetPlayerData()
        {
            _playerRepository.ResetData();
        }

        public void SetReputationAmount(int amount)
        {
            PlayerRepository.PlayerData playerData = _playerRepository.LoadPlayerData();
            playerData.Reputation = amount;
            _playerRepository.SavePlayerData(playerData);
        }

        /// <summary>
        /// Use this for debug tool
        /// </summary>
        public void SetPlayerData(string name, EGender gender, ERace race, int reputationAmount)
        {
            PlayerRepository.PlayerData playerData = _playerRepository.LoadPlayerData();
            playerData.Race = race;
            playerData.Gender = gender;
            playerData.PlayerName = name;
            playerData.Reputation = reputationAmount;

            _playerRepository.SavePlayerData(playerData);

        }
    }
}

