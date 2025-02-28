using InventoryTestCase;
using UnityEngine;
using Zenject;

public class InstallerUI : MonoInstaller
{
    [SerializeField] private InventoryUIView _inventoryView;
    [SerializeField] private int _inventorySize;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InventoryModel>()
            .AsSingle()
            .WithArguments(_inventorySize)
            .NonLazy();

        Container.BindInterfacesAndSelfTo<InventoryController>()
            .AsSingle()
            .WithArguments(_inventoryView, _inventorySize)
            .NonLazy();
    }
}