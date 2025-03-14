using DialogueSystem.DialogueEditor;
using Player;
using UnityEngine;

namespace GM_Tools
{
    public class PlayerTool : MonoBehaviour
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

        public void SetReputationAmount(int amount, EFaction faction)
        {
            PlayerRepository.PlayerData playerData = _playerRepository.LoadPlayerData();

            switch (faction)
            {
                case EFaction.OfficialAuthorities:
                    playerData.Reputation_OfficialAuthorities = amount;
                    break;
                case EFaction.Civilian:
                    playerData.Reputation_Civilian = amount;
                    break;
                case EFaction.Bandits:
                    playerData.Reputation_Bandits = amount;
                    break;
                default:
                    break;
            }

            _playerRepository.SavePlayerData(playerData);
        }

        /// <summary>
        /// Use this for debug tool
        /// </summary>
        public void SetPlayerData(string name, EGender gender, ERace race, 
            int reputation_OA, int reputation_C, int reputation_B)
        {
            PlayerRepository.PlayerData playerData = _playerRepository.LoadPlayerData();
            playerData.Race = race;
            playerData.Gender = gender;
            playerData.PlayerName = name;
            playerData.Reputation_OfficialAuthorities = reputation_OA;
            playerData.Reputation_Civilian = reputation_C;
            playerData.Reputation_Bandits = reputation_B;

            _playerRepository.SavePlayerData(playerData);

        }
    }
}

