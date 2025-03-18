using DialogueSystem.DialogueEditor;
using Player;
using UnityEngine;

namespace GM_Tools
{
    public class PlayerTool : MonoBehaviour
    {
        private PlayerRepository _playerRepository;

        public PlayerRepository PlayerRepository
        {
            get
            {
                _playerRepository ??= new PlayerRepository();
                return _playerRepository;
            }
        }

        public void ResetPlayerData()
        {
            PlayerRepository.ResetData();
        }

        public void SetPlayerData(string name, EGender gender, ERace race,
            int reputation_OA, int reputation_C, int reputation_B)
        {
            PlayerData playerData = PlayerRepository.LoadPlayerData();
            playerData.Race = race;
            playerData.Gender = gender;
            playerData.PlayerName = name;
            playerData.Reputation_OfficialAuthorities = reputation_OA;
            playerData.Reputation_Civilian = reputation_C;
            playerData.Reputation_Bandits = reputation_B;

            PlayerRepository.SavePlayerData(playerData);
        }
    }
}

