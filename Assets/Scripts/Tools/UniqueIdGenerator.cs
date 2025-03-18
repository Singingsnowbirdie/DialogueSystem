using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GM_Tools
{
    [ExecuteInEditMode] 
    public class UniqueIdGenerator : MonoBehaviour
    {
        [SerializeField] private string _uniqueId; 

        private static readonly System.Random _random = new System.Random();
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public string UniqueId => _uniqueId;

        public string GenerateId(int length = 8)
        {
            _uniqueId = new string(Enumerable.Repeat(Characters, length)
                .Select(s => s[_random.Next(s.Length)])
                .ToArray());
            return _uniqueId;
        }

        public void ResetId()
        {
            _uniqueId = string.Empty;
        }

        public void CopyIdToClipboard()
        {
            if (!string.IsNullOrEmpty(_uniqueId))
            {
                GUIUtility.systemCopyBuffer = _uniqueId;
                Debug.Log("ID copied to clipboard: " + _uniqueId);
            }
            else
            {
                Debug.Log("ID not generated. Create ID first..");
            }
        }
    }
}

