using System;

namespace InventoryTestCase
{
    [Serializable]
    public struct ItemDataModel
    {
        public int quantity;
        public ItemData item;
        public bool IsEmpty => item == null;

        public ItemDataModel ChangeQuantity(int newQuantity)
            => new() { item = item, quantity = newQuantity };

        public static ItemDataModel GetEmptyItem() => new() { item = null, quantity = 0 };
    }
}
