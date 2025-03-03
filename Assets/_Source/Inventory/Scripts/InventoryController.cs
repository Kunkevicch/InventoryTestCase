using System;
using UnityEngine;
using Zenject;

namespace InventoryTestCase
{
    public class InventoryController : IInitializable, IDisposable
    {
        private readonly InventoryUIView _inventoryView;
        private readonly InventoryModel _inventoryModel;
        private readonly IControlPanel _inventoryControlPanel;

        public InventoryController(
            InventoryUIView inventoryView
            , InventoryModel inventoryModel
            , IControlPanel inventoryControlPanel
            )
        {
            _inventoryView = inventoryView;
            _inventoryModel = inventoryModel;
            _inventoryControlPanel = inventoryControlPanel;
        }

        public void Initialize()
        {
            _inventoryModel.InventoryChanged += OnInventoryChanged;
            
            _inventoryView.InitializeInventoryView(_inventoryModel.Config);
            _inventoryModel.Initialize();
            

            _inventoryView.ItemsSwapped += OnItemSwapped;
            _inventoryView.ItemUnlockRequested += OnItemUnlockRequested;

            _inventoryControlPanel.ShootClicked += OnShootClicked;
            _inventoryControlPanel.AddAmmoClicked += OnAddAmmoClicked;
            _inventoryControlPanel.AddItemClicked += OnAddItemClicked;
            _inventoryControlPanel.DeleteItemClicked += OnDeleteItemClicked;
        }

        public void Dispose()
        {
            _inventoryModel.InventoryChanged -= OnInventoryChanged;

            _inventoryView.ItemsSwapped -= OnItemSwapped;
            _inventoryView.ItemUnlockRequested -= OnItemUnlockRequested;

            _inventoryControlPanel.ShootClicked -= OnShootClicked;
            _inventoryControlPanel.AddAmmoClicked -= OnAddAmmoClicked;
            _inventoryControlPanel.AddItemClicked -= OnAddItemClicked;
            _inventoryControlPanel.DeleteItemClicked -= OnDeleteItemClicked;
        }

        private void OnInventoryChanged()
        {
            _inventoryView.ResetAllItems();
            var currentInventoryState = _inventoryModel.GetCurrentInventoryState();
            foreach (var keyPair in currentInventoryState)
            {
                if (!keyPair.Value.IsEmpty)
                {
                    _inventoryView.UpdateData(keyPair.Key, keyPair.Value.item.Image, keyPair.Value.quantity);
                }
                if (!keyPair.Value.isLocked)
                {
                    _inventoryView.UnlockSlotByID(keyPair.Key);
                }
            }
        }

        private void OnItemSwapped(int firstItemID, int secondItemID)
        => _inventoryModel.SwapItems(firstItemID, secondItemID);

        private void OnItemUnlockRequested(int index)
        {
            //check that slot can be unlocked
            _inventoryModel.UnlockSlot(index);
        }

        private void OnShootClicked()
        => _inventoryModel.DeleteRandomItemOfType<AmmoData>();

        private void OnAddAmmoClicked()
        => _inventoryModel.AddRandomItem<AmmoData>();

        private void OnAddItemClicked()
        => _inventoryModel.AddRandomItems();

        private void OnDeleteItemClicked()
        => _inventoryModel.DeleteRandomItem();
    }
}
