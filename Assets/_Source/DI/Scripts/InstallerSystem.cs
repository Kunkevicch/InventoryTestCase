using InventoryTestCase;
using UnityEngine;
using Zenject;

public class InstallerSystem : MonoInstaller
{
    [SerializeField] private InventoryConfig _inventoryConfig;

    public override void InstallBindings()
    {
        BindSystem();
    }

    private void BindSystem()
    {
        Container.BindInterfacesAndSelfTo<InventoryModel>()
            .AsSingle()
            .WithArguments(_inventoryConfig)
            .NonLazy();

        Container.BindInterfacesAndSelfTo<JSONStorageService>()
            .AsSingle()
            .NonLazy();
    }
}