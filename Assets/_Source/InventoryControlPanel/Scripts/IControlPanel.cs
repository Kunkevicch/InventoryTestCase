using System;

namespace InventoryTestCase
{
    public interface IControlPanel
    {
        public event Action ShootClicked;
        public event Action AddAmmoClicked;
        public event Action AddItemClicked;
        public event Action DeleteItemClicked;
    }
}
