using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventoryTestCase
{
    public class ItemUIView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private Image _selectionBorder;
        [SerializeField] private Image _lockedBorder;

        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _quantityText;

        public event Action<ItemUIView> ItemClicked;
        public event Action<ItemUIView> ItemDropped;
        public event Action<ItemUIView> ItemBeginDrag;
        public event Action<ItemUIView> ItemEndDrag;
        public event Action<ItemUIView> SlotsUnlocked;

        private bool _isEmpty = true;
        private bool _isLocked = true;

        private void Awake()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            _icon.gameObject.SetActive(false);
            _isEmpty = true;
        }

        public void Select() => _selectionBorder.enabled = true;

        public void Deselect() => _selectionBorder.enabled = false;

        public void SetData(Sprite sprite, int quantity)
        {
            _icon.gameObject.SetActive(true);
            _icon.sprite = sprite;
            _quantityText.gameObject.SetActive(quantity > 1);
            _quantityText.text = quantity.ToString();
            _isEmpty = false;
        }

        public void UnlockRequest()
        => SlotsUnlocked?.Invoke(this);


        public void Unlock()
        {
            _isLocked = false;
            _lockedBorder.gameObject.SetActive(false);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isEmpty || _isLocked)
                return;

            ItemBeginDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (_isLocked)
                return;

            ItemDropped?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isLocked)
                return;

            ItemEndDrag?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isLocked)
                return;
            ItemClicked?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        { }
    }
}
