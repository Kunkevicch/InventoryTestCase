using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryTestCase
{
    public class InventoryUIView : MonoBehaviour
    {
        [SerializeField] private ItemUIView _itemPrefab;
        [SerializeField] private RectTransform _contentPanel;

        private readonly List<ItemUIView> _listOfItemsView = new();
        private int _currentSelectedItemID = -1;

        public void InitializeInventoryView(InventoryConfig inventoryConfig)
        {
            for (int i = 0; i < inventoryConfig.SlotsPrices.Count; i++)
            {
                ItemUIView uiItem = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(_contentPanel);

                uiItem.ItemClicked += OnItemClicked;
                uiItem.ItemBeginDrag += OnItemBeginDragged;
                uiItem.ItemEndDrag += OnItemEndDragged;
                uiItem.ItemDropped += OnItemDropped;
                uiItem.SlotsUnlocked += OnItemUnlocked;

                if (inventoryConfig.SlotsPrices[i] == 0)
                {
                    uiItem.Unlock();
                }

                _listOfItemsView.Add(uiItem);
            }
        }

        public event Action<int, int> ItemsSwapped;
        public event Action<int> ItemUnlockRequested;

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (_listOfItemsView.Count > itemIndex)
            {
                _listOfItemsView[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        public void ResetAllItems()
        {
            foreach (var item in _listOfItemsView)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        public void ResetSelection()
        => DeselectAllItems();

        public void UnlockSlotByID(int id)
        {
            if (_listOfItemsView.Count > id)
            {
                _listOfItemsView[id].Unlock();
            }
        }

        private void OnItemClicked(ItemUIView itemView)
        {
            int index = _listOfItemsView.IndexOf(itemView);

            if (index == -1)
                return;

            if (_currentSelectedItemID != -1)
            {
                _listOfItemsView[_currentSelectedItemID].Deselect();
            }
            _currentSelectedItemID = index;

            _listOfItemsView[_currentSelectedItemID].Select();
        }

        private void OnItemBeginDragged(ItemUIView itemView)
        {
            int index = _listOfItemsView.IndexOf(itemView);
            if (index == -1)
                return;
            _currentSelectedItemID = index;
        }

        private void OnItemEndDragged(ItemUIView itemView)
        => ResetDraggedItem();


        private void OnItemDropped(ItemUIView itemView)
        {
            int index = _listOfItemsView.IndexOf(itemView);

            if (index == -1)
            {
                return;
            }

            ItemsSwapped?.Invoke(_currentSelectedItemID, index);
        }

        private void OnItemUnlocked(ItemUIView itemView)
        => ItemUnlockRequested?.Invoke(_listOfItemsView.IndexOf(itemView));


        private void DeselectAllItems()
        {
            for (int i = 0; i < _listOfItemsView.Count; i++)
            {
                _listOfItemsView[i].Deselect();
            }
        }
        private void ResetDraggedItem()
        {
            _currentSelectedItemID = -1;
        }
    }
}
