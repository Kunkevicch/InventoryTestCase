#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace InventoryTestCase
{
    [CustomEditor(typeof(InventoryConfig))]
    public class InventoryConfigEditor : Editor
    {
        private int _addCount = 1;
        private int _slotPrice = 100;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_slotsPrices"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_availableItems"), true);

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Batch Add Slots", EditorStyles.boldLabel);

            _addCount = EditorGUILayout.IntField("Number of Slots", Mathf.Max(1, _addCount));
            _slotPrice = EditorGUILayout.IntField("Slot Price", Mathf.Max(0, _slotPrice));

            if (GUILayout.Button($"Add {_addCount} Slot(s) with Price {_slotPrice}"))
            {
                Undo.RecordObject(target, "Add inventory slots");
                (target as InventoryConfig).AddSlots(_addCount, _slotPrice);
                EditorUtility.SetDirty(target);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif