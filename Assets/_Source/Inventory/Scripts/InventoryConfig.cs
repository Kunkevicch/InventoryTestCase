using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventoryTestCase
{
    [CreateAssetMenu(fileName = "InventoryConfig", menuName = "Configs/Inventory")]
    public class InventoryConfig : ScriptableObject
    {
        [SerializeField] private List<int> _slotsPrices;
        [SerializeField] private List<ItemData> _availableItems;

        public List<int> SlotsPrices => _slotsPrices;
        public List<ItemData> AvailableItems => _availableItems;

        public void AddSlots(int count, int price)
        {
            var newPrices = new int[count];
            for (int i = 0; i < count; i++)
                newPrices[i] = price;
            _slotsPrices.AddRange(newPrices);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _availableItems.Distinct();
        }
#endif
    }
}
