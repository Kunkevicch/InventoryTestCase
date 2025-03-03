using UnityEngine;

namespace InventoryTestCase
{
    [CreateAssetMenu(fileName = "EquipData_", menuName = "Configs/Items/Equip")]
    public class EquipData : ItemData
    {
        [field: SerializeField] public EquipType Type { get; private set; }
        [field: SerializeField] public int Armor { get; private set; }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Armor = Mathf.Abs(Armor);
        }
#endif
    }
}
