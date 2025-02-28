using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventoryTestCase
{
    public class InventoryModel
    {
        private List<ItemDataModel> _inventoryItems;
        private readonly int _size;

        public InventoryModel(int size)
        {
            _size = size;
        }

        public event Action InventoryChanged;

        public int Size => _size;

        public void Initialize()
        {
            _inventoryItems = new List<ItemDataModel>();
            for (int i = 0; i < _size; i++)
            {
                _inventoryItems.Add(ItemDataModel.GetEmptyItem());
            }
        }

        public int AddItem(ItemData item, int quantity)
        {
            if (!item.IsStackable)
            {
                for (int i = 0; i < _inventoryItems.Count; i++)
                {
                    while (quantity > 0 && !IsInventoryFull())
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1);
                    }
                    InventoryChanged?.Invoke();
                    return quantity;

                }
            }
            quantity = AddStackableItem(item, quantity);
            InventoryChanged?.Invoke();
            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemData item, int quantity)
        {
            ItemDataModel newItemDataModel = new() { item = item, quantity = quantity };

            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                {
                    _inventoryItems[i] = newItemDataModel;
                    return quantity;
                }
            }
            return 0;
        }

        private bool IsInventoryFull()
        => !_inventoryItems.Where(item => item.IsEmpty).Any();

        private int AddStackableItem(ItemData item, int quantity)
        {
            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                    continue;

                if (_inventoryItems[i].item.ID == item.ID)
                {
                    int amountPossibleToTake
                        = _inventoryItems[i].item.MaxStackSize - _inventoryItems[i].quantity;

                    if (quantity > amountPossibleToTake)
                    {
                        _inventoryItems[i] = _inventoryItems[i].ChangeQuantity(_inventoryItems[i].item.MaxStackSize);
                        quantity -= amountPossibleToTake;

                    }
                    else
                    {
                        _inventoryItems[i] = _inventoryItems[i].ChangeQuantity(_inventoryItems[i].quantity + quantity);
                        InventoryChanged?.Invoke();
                        return 0;
                    }
                }
            }

            while (quantity > 0 && !IsInventoryFull())
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);
            }
            return quantity;
        }

        public void AddItem(ItemDataModel item) => AddItem(item.item, item.quantity);

        public void DeleteItem(int itemID, int amount)
        {
            if (_inventoryItems.Count > itemID)
            {
                if (_inventoryItems[itemID].IsEmpty)
                    return;

                int reminder = _inventoryItems[itemID].quantity - amount;

                if (reminder <= 0)
                {
                    _inventoryItems[itemID] = ItemDataModel.GetEmptyItem();
                }
                else
                {
                    _inventoryItems[itemID] = _inventoryItems[itemID].ChangeQuantity(reminder);
                }
                InventoryChanged?.Invoke();
            }
        }

        public void DeleteItem(ItemDataModel item)
        {
            var findedItem = _inventoryItems.IndexOf(item);
            DeleteItem(findedItem, 1);
        }

        public Dictionary<int, ItemDataModel> GetCurrentInventoryState()
        {
            Dictionary<int, ItemDataModel> returnValue =
                new();

            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                {
                    continue;
                }
                returnValue[i] = _inventoryItems[i];
            }
            return returnValue;
        }

        public ItemDataModel GetItemByID(int itemID) => _inventoryItems[itemID];

        public void SwapItems(int firstItemID, int secondItemID)
        {
            (_inventoryItems[secondItemID], _inventoryItems[firstItemID]) = (_inventoryItems[firstItemID], _inventoryItems[secondItemID]);
            InventoryChanged?.Invoke();
        }

        public int TryConsumeAmmoFromMultipleStacks(ItemData ammoData, int requiredAmount)
        {
            int remainingAmount = requiredAmount;
            int consumedAmmo = 0;

            foreach (var item in _inventoryItems)
            {
                if (item.IsEmpty)
                    continue;
                if (item.item.ID == ammoData.ID && item.quantity > 0)
                {
                    if (item.quantity >= remainingAmount)
                    {
                        DeleteItem(_inventoryItems.IndexOf(item), remainingAmount);
                        consumedAmmo += remainingAmount;
                        remainingAmount = 0;
                        break;
                    }
                    else
                    {
                        consumedAmmo += item.quantity;
                        remainingAmount -= item.quantity;
                        DeleteItem(_inventoryItems.IndexOf(item), item.quantity);
                    }
                }

                if (remainingAmount <= 0)
                {
                    break;
                }
            }

            if (consumedAmmo > 0)
            {
                InventoryChanged?.Invoke();
            }

            return consumedAmmo;
        }
    }
}
