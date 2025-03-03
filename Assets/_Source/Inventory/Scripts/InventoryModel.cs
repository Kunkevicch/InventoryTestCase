using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventoryTestCase
{
    public class InventoryModel : IDisposable
    {
        private List<ItemDataModel> _inventoryItems;
        private readonly IStorageService _storageService;
        private readonly InventoryConfig _config;

        private const string SAVE_DATA_KEY = "inventory";
        public InventoryModel(InventoryConfig config, IStorageService storageService)
        {
            _config = config;
            _storageService = storageService;
        }

        public event Action InventoryChanged;

        public int Size => _config.SlotsPrices.Count;
        public InventoryConfig Config => _config;

        public void Initialize()
        {
            _inventoryItems = new List<ItemDataModel>();
            for (int i = 0; i < Size; i++)
            {
                _inventoryItems.Add(
                    ItemDataModel.GetEmptyItem()
                    .SetLockStatus(_config.SlotsPrices[i] != 0)
                    );
            }
            _storageService.Load<InventorySaveData>(SAVE_DATA_KEY, OnLoad);
        }

        public void Dispose()
        => InventoryChanged -= OnInventoryChanged;


        private void OnLoad(InventorySaveData loadedData)
        {
            for (int i = 0; i < loadedData.InventoryItems.Count; i++)
            {
                ItemDataModel slotDataModel = _inventoryItems[i];
                slotDataModel = slotDataModel.ChangeQuantity(loadedData.InventoryItems[i].quantity);
                slotDataModel.item = GetItemByID(loadedData.InventoryItems[i].itemID);
                slotDataModel = slotDataModel.SetLockStatus(loadedData.InventoryItems[i].IsLocked);
                _inventoryItems[i] = slotDataModel;
            }

            InventoryChanged?.Invoke();

            InventoryChanged += OnInventoryChanged;
        }

        public int AddItem(ItemData item, int quantity)
        {
            if (IsInventoryFull())
            {
                Debug.LogError("Inventory is full!");
                return 0;
            }

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
            var newData = new InventorySaveData();

            _storageService.Save(SAVE_DATA_KEY, newData);
            InventoryChanged?.Invoke();
            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemData item, int quantity)
        {
            ItemDataModel newItemDataModel = new() { item = item, quantity = quantity };

            for (int i = 0; i < _inventoryItems.Count; i++)
            {

                if (_inventoryItems[i].IsEmpty && !_inventoryItems[i].isLocked)
                {
                    _inventoryItems[i] = newItemDataModel;
                    return quantity;
                }
            }
            return 0;
        }

        private bool IsInventoryFull()
        => !_inventoryItems.Where(item => !item.isLocked).Any(item => item.IsEmpty);

        private bool IsInventoryEmpty()
            => _inventoryItems.Where(item => !item.isLocked).All(item => item.IsEmpty);

        private int AddStackableItem(ItemData item, int quantity)
        {
            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty || _inventoryItems[i].isLocked)
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

        public void AddRandomItems()
        {
            AddRandomItem<AmmoData>();
            AddRandomItem<EquipData>();
            AddRandomItem<WeaponData>();
        }

        public void AddRandomItem<T>() where T : ItemData
        {
            var randomItemOfType = GetRandomAvailableItem<T>();
            AddItem(randomItemOfType, randomItemOfType.MaxStackSize);
        }

        private ItemData GetRandomAvailableItem<T>() where T : ItemData
        {
            var availableItemsOfType = _config.AvailableItems.FindAll(x => x is T);
            return availableItemsOfType[UnityEngine.Random.Range(0, availableItemsOfType.Count)];
        }

        private ItemData GetItemByID(string ID)
        => _config.AvailableItems.Find(x => x.ID == ID);


        public void DeleteRandomItemOfType<T>(int amount = 1) where T : ItemData
        {
            var randomItemsOfType = _inventoryItems.FindAll(x => x.item is T);
            if (randomItemsOfType.Count == 0)
            {
                Debug.LogError($"Cant find items of type:{typeof(T).Name}");
                return;
            }
            int index = _inventoryItems.IndexOf(randomItemsOfType[UnityEngine.Random.Range(0, randomItemsOfType.Count)]);

            DeleteItem(index, amount);
        }
        public void DeleteRandomItem()
        {
            if (IsInventoryEmpty())
            {
                Debug.LogError("Inventory is empty");
                return;
            }
            var randomItems = _inventoryItems.FindAll(x => !x.IsEmpty && !x.isLocked);
            int randomItemID = _inventoryItems.IndexOf(randomItems[UnityEngine.Random.Range(0, randomItems.Count)]);
            DeleteItem(randomItemID, _inventoryItems[randomItemID].quantity);
        }

        public void DeleteItem(int itemID, int amount)
        {
            if (IsInventoryEmpty())
            {
                Debug.LogError("Inventory is empty!!");
                return;
            }

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
        => DeleteItem(_inventoryItems.IndexOf(item), 1);


        public Dictionary<int, ItemDataModel> GetCurrentInventoryState()
        {
            Dictionary<int, ItemDataModel> returnValue =
                new();

            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty && _inventoryItems[i].isLocked)
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

        public void UnlockSlot(int index)
        {
            _inventoryItems[index] = _inventoryItems[index].SetLockStatus(false);
            InventoryChanged?.Invoke();
        }

        private void OnInventoryChanged()
        {
            InventorySaveData saveData = new();

            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                InventorySaveDataDetails saveDataDetails = new()
                {
                    quantity = _inventoryItems[i].quantity,
                    itemID = _inventoryItems[i].IsEmpty ? "" : _inventoryItems[i].item.ID,
                    IsLocked = _inventoryItems[i].isLocked,
                    IsEmpty = _inventoryItems[i].IsEmpty
                };
                saveData.InventoryItems.Add(i, saveDataDetails);
            }

            _storageService.Save(SAVE_DATA_KEY, saveData);
        }
    }

    [Serializable]
    public class InventorySaveData
    {
        [JsonProperty(PropertyName = "Items")]
        public Dictionary<int, InventorySaveDataDetails> InventoryItems;

        public InventorySaveData()
        {
            InventoryItems = new();
        }
    }

    public struct InventorySaveDataDetails
    {
        public int quantity;
        public string itemID;
        public bool IsLocked;
        public bool IsEmpty;
    }
}
