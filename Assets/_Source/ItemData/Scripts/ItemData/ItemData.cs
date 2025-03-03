using System;
using UnityEngine;

namespace InventoryTestCase
{
    [CreateAssetMenu(fileName = "ItemData_", menuName = "Configs/Items/Item")]
    public class ItemData : ScriptableObject
    {
        [SerializeField, HideInInspector] private string _id;
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public float ItemWeight { get; private set; }

        [SerializeField] protected bool _isStackable;

        [SerializeField] protected int _maxStackSize;

        public int MaxStackSize => _maxStackSize;
        public bool IsStackable => _isStackable;
        public string ID => _id;
        [field: SerializeField] public Sprite Image { get; private set; }
        [field: TextArea]
        [field: SerializeField] public string ItemDescription { get; private set; }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (!IsStackable)
            {
                _maxStackSize = 1;
            }

            if (string.IsNullOrEmpty(_id))
            {
                _id = Guid.NewGuid().ToString();
            }
        }
#endif
    }
}
