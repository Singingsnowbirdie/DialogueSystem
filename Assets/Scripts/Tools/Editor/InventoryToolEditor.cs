using UnityEditor;
using UnityEngine;

namespace GM_Tools
{
    [CustomEditor(typeof(InventoryTool))]
    public class InventoryToolEditor : Editor
    {
        private string _itemId = "";
        private int _quantity = 1;

        public override void OnInspectorGUI()
        {
            InventoryTool inventoryTool = (InventoryTool)target;

            DrawDefaultInspector();

            // ADD OR REMOVE ITEM

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Add or Remove Item", EditorStyles.boldLabel);

            _itemId = EditorGUILayout.TextField("Item ID", _itemId);

            if (string.IsNullOrEmpty(_itemId))
            {
                EditorGUILayout.HelpBox("Item ID cannot be empty.", MessageType.Error);
            }

            _quantity = EditorGUILayout.IntField("Quantity", _quantity);

            if (_quantity < 1)
            {
                _quantity = 1;
            }

            if (GUILayout.Button("Update item quantity"))
            {
                inventoryTool.SetItemQuantity(_itemId, _quantity);
            }

            if (GUILayout.Button("Remove Item"))
            {
                inventoryTool.RemoveItem(_itemId);
            }

            // RESET DATA

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Reset Data", EditorStyles.boldLabel);

            if (GUILayout.Button("Reset Inventory Data"))
            {
                inventoryTool.ResetData();
            }

            EditorGUILayout.Space();

        }
    }
}

