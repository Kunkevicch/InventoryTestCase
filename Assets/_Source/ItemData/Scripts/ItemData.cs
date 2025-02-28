using UnityEngine;

namespace InventoryTestCase
{
    [CreateAssetMenu(fileName = "ItemData_", menuName = "Configs/Items/ItemData")]
    public class ItemData : ScriptableObject
    {
        public int ID => GetInstanceID();
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public float ItemWeight { get; private set; }

        [SerializeField] private bool _isStackable;

        [SerializeField] private int _maxStackSize;

        public int MaxStackSize => _maxStackSize;
        public bool IsStackable => _isStackable;
        [field: SerializeField] public Sprite Image { get; private set; }
        [field: TextArea]
        [field: SerializeField] public string ItemDescription { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (IsStackable)
            {
                _maxStackSize = 1;
            }
        }
#endif
    }
}
