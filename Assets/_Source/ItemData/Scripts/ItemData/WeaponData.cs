using UnityEngine;

namespace InventoryTestCase
{
    [CreateAssetMenu(fileName = "WeaponData_", menuName = "Configs/Items/Weapon")]
    public class WeaponData : ItemData
    {
        [field: SerializeField] public AmmoType AmmoType { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Damage = Mathf.Abs(Damage);
        }
#endif
    }
}
