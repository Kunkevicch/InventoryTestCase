using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventoryTestCase
{
    public class ItemUIView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private Image _border;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _quantityText;

        public event Action<ItemUIView> ItemClicked;
        public event Action<ItemUIView> ItemDropped;
        public event Action<ItemUIView> ItemBeginDrag;
        public event Action<ItemUIView> ItemEndDrag;

        private bool _isEmpty = true;

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

        public void Select() => _border.gameObject.SetActive(true);

        public void Deselect() => _border.gameObject.SetActive(false);

        public void SetData(Sprite sprite, int quantity)
        {
            _icon.gameObject.SetActive(true);
            _icon.sprite = sprite;
            _quantityText.text = quantity.ToString();
            _isEmpty = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log(1);
            if (_isEmpty)
                return;

            ItemBeginDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            ItemDropped?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ItemEndDrag?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ItemClicked?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        { }
    }
}
