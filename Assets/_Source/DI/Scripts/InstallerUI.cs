using InventoryTestCase;
using UnityEngine;
using Zenject;

public class InstallerUI : MonoInstaller
{
    [SerializeField] private InventoryUIView _inventoryView;

    public override void InstallBindings()
    {
        BindUI();
    }

    private void BindUI()
    {
        Container.BindInterfacesTo<InventoryControlPanel>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();


        Container.BindInterfacesAndSelfTo<InventoryController>()
            .AsSingle()
            .WithArguments(_inventoryView)
            .NonLazy();
    }
}