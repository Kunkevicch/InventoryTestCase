using UnityEngine;

namespace InventoryTestCase
{
    [CreateAssetMenu(fileName = "WeaponData_", menuName = "Configs/Items/Ammo")]
    public class AmmoData : ItemData
    {
        [field: SerializeField] public AmmoType Type { get; private set; }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
        }
#endif
    }
}
