using UnityEditor;
using UnityEngine;

namespace InventoryTestCase
{
    [CustomEditor(typeof(ItemData))]
    public class ItemDataEditor : Editor
    {
        private SerializedProperty _isStackableProp;
        private SerializedProperty _maxStackSizeProp;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            _isStackableProp = serializedObject.FindProperty("_isStackable");
            _maxStackSizeProp = serializedObject.FindProperty("_maxStackSize");

            DrawPropertiesExcluding(serializedObject, "m_Script", "_maxStackSize");

            if (_isStackableProp.boolValue)
            {
                EditorGUILayout.PropertyField(_maxStackSizeProp);
                _maxStackSizeProp.intValue = Mathf.Max(1, _maxStackSizeProp.intValue);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}