using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryTestCase
{
    public class InventoryUIView : MonoBehaviour
    {
        [SerializeField] private ItemUIView _itemPrefab;
        [SerializeField] private RectTransform _contentPanel;

        List<ItemUIView> _listOfItemsView = new();
        private int _currentSelectedItemID = -1;

        public void InitializeInventoryView(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                ItemUIView uiItem = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(_contentPanel);

                uiItem.ItemClicked += OnItemClicked;
                uiItem.ItemBeginDrag += OnItemBeginDragged;
                uiItem.ItemEndDrag += OnItemEndDragged;
                uiItem.ItemDropped += OnItemDropped;

                _listOfItemsView.Add(uiItem);
            }
        }

        public event Action<int> DescriptionRequested;
        public event Action<int> ItemActionRequested;
        public event Action<int> StartDragging;
        public event Action<int, int> ItemsSwapped;
        public event Action<int> ItemDeleteRequested;

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
        {
            //_deleteBtn.gameObject.SetActive(false);
            //_description.ResetDescription();
            DeselectAllItems();
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
            //_deleteBtn.gameObject.SetActive(false);
            DescriptionRequested?.Invoke(index);
        }

        private void OnItemBeginDragged(ItemUIView itemView)
        {
            int index = _listOfItemsView.IndexOf(itemView);
            if (index == -1)
                return;
            _currentSelectedItemID = index;
            //OnItemBeginDragged(itemView);
            StartDragging?.Invoke(index);
        }

        private void OnItemEndDragged(ItemUIView itemView) =>
            ResetDraggedItem();


        private void OnItemDropped(ItemUIView itemView)
        {
            int index = _listOfItemsView.IndexOf(itemView);

            if (index == -1)
            {
                return;
            }
            ItemsSwapped?.Invoke(_currentSelectedItemID, index);
        }

        private void DeselectAllItems()
        {
            for (int i = 0; i < _listOfItemsView.Count; i++)
            {
                _listOfItemsView[i].Deselect();
            }
        }
        private void ResetDraggedItem()
        {
            //_inventoryPointerFollow.Toggle(false);
            _currentSelectedItemID = -1;
        }
    }
}
