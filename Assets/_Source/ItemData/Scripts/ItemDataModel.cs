using System;

namespace InventoryTestCase
{
    [Serializable]
    public struct ItemDataModel
    {
        public int quantity;
        public bool isLocked;
        public ItemData item;
        public int slotPrice;
        public bool IsEmpty => item == null;
        public ItemDataModel ChangeQuantity(int newQuantity)
            => new() { item = item, quantity = newQuantity, isLocked = isLocked };
        public ItemDataModel SetLockStatus(bool status)
            => new() { item = item, quantity = quantity, isLocked = status };

        public static ItemDataModel GetEmptyItem() => new() { item = null, quantity = 0 };
    }
}
