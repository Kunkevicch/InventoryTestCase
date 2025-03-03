using System;
using Zenject;

namespace InventoryTestCase
{
    public class ProgressObserver : IInitializable, IDisposable
    {
        private readonly InventoryModel _inventoryModel;
        private readonly IStorageService _storageService;

        public ProgressObserver(InventoryModel inventoryModel, IStorageService storageService)
        {
            _inventoryModel = inventoryModel;
            _storageService = storageService;
        }

        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public interface IProgressObserver
    {

    }
}
