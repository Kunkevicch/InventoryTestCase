using System;
using Zenject;

namespace InventoryTestCase
{
    public class InventoryController : IInitializable, IDisposable
    {
        private readonly InventoryUIView _inventoryView;
        private readonly InventoryModel _inventoryModel;
        private readonly int _inventorySize;

        public InventoryController(InventoryUIView inventoryView, InventoryModel inventoryModel, int inventorySize)
        {
            _inventoryView = inventoryView;
            _inventoryModel = inventoryModel;
            _inventorySize = inventorySize;
        }

        public void Initialize()
        {
            _inventoryModel.InventoryChanged += OnInventoryChanged;

            _inventoryModel.Initialize();
            _inventoryView.InitializeInventoryView(_inventorySize);

            _inventoryView.DescriptionRequested += OnDescriptionRequested;
            _inventoryView.ItemsSwapped += OnItemSwapped;
            _inventoryView.StartDragging += OnStartDragging;
            _inventoryView.ItemActionRequested += ItemActionRequested;
            _inventoryView.ItemDeleteRequested += OnItemDeleteRequested;
        }

        public void Dispose()
        {
            _inventoryModel.InventoryChanged -= OnInventoryChanged;

            _inventoryView.DescriptionRequested -= OnDescriptionRequested;
            _inventoryView.ItemsSwapped -= OnItemSwapped;
            _inventoryView.StartDragging -= OnStartDragging;
            _inventoryView.ItemActionRequested -= ItemActionRequested;
            _inventoryView.ItemDeleteRequested -= OnItemDeleteRequested;
        }

        private void OnInventoryChanged()
        {
            _inventoryView.ResetAllItems();
            var currentInventoryState = _inventoryModel.GetCurrentInventoryState();

            foreach (var keyPair in currentInventoryState)
            {
                _inventoryView.UpdateData(keyPair.Key, keyPair.Value.item.Image, keyPair.Value.quantity);
            }
        }

        private void OnDescriptionRequested(int itemID)
        {
            ItemDataModel itemDataModel = _inventoryModel.GetItemByID(itemID);
            if (itemDataModel.IsEmpty)
            {
                _inventoryView.ResetSelection();
                return;
            }

            ItemData item = itemDataModel.item;
            //_inventoryView.UpdateDescription(itemID, item.Image, item.Name, item.Description);
        }

        private void ItemActionRequested(int itemID)
        {
            throw new NotImplementedException();
        }

        private void OnStartDragging(int itemID)
        {
            ItemDataModel itemDataModel = _inventoryModel.GetItemByID(itemID);
            if (itemDataModel.IsEmpty)
                return;

            //_inventoryView.CreateDraggedItem(itemDataModel.item.Image, itemDataModel.quantity);
        }

        private void OnItemSwapped(int firstItemID, int secondItemID) => _inventoryModel.SwapItems(firstItemID, secondItemID);

        private void OnItemDeleteRequested(int itemID) => _inventoryModel.DeleteItem(itemID, 1);
    }
}
