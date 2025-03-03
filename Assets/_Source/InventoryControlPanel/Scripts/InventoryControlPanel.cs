using System;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryTestCase
{
    public class InventoryControlPanel : MonoBehaviour, IControlPanel
    {
        [SerializeField] private Button _shootBtn;
        [SerializeField] private Button _addAmmoBtn;
        [SerializeField] private Button _addItemBtn;
        [SerializeField] private Button _deleteItemBtn;

        public event Action ShootClicked;
        public event Action AddAmmoClicked;
        public event Action AddItemClicked;
        public event Action DeleteItemClicked;

        private void OnEnable()
        {
            _shootBtn.onClick.AddListener(() => { ShootClicked?.Invoke(); });
            _addAmmoBtn.onClick.AddListener(() => { AddAmmoClicked?.Invoke(); });
            _addItemBtn.onClick.AddListener(() => { AddItemClicked?.Invoke(); });
            _deleteItemBtn.onClick.AddListener(() => { DeleteItemClicked?.Invoke(); });
        }

        private void OnDisable()
        {
            _shootBtn.onClick.RemoveListener(() => { ShootClicked?.Invoke(); });
            _addAmmoBtn.onClick.RemoveListener(() => { AddAmmoClicked?.Invoke(); });
            _addItemBtn.onClick.RemoveListener(() => { AddItemClicked?.Invoke(); });
            _deleteItemBtn.onClick.RemoveListener(() => { DeleteItemClicked?.Invoke(); });
        }
    }
}
