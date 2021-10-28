using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using ShopManagement.Infrastructure.EFCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Infrastructure.EFCore.Repository
{
    public class InventoryRepository : RepositoryBase<long, Inventory>, IInventoryRepository
    {
        private readonly AccountContext _accountContext;
        private readonly InventoryContext _context;
        private readonly ShopContext _shopContext;
        public InventoryRepository(InventoryContext context, ShopContext shopContext, AccountContext accountContext) : base(context)
        {
            _context = context;
            _shopContext = shopContext;
            _accountContext = accountContext;
        }

        public Inventory GetBy(long productId)
        {
            return _context.Inventory.FirstOrDefault(x => x.ProductId == productId);
        }

        public EditInventory GetDetails(long id)
        {
            return _context.Inventory.Select(x => new EditInventory
            {
                Id = x.Id,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
        {
            var accounts = _accountContext.Accounts.Select(x => new { x.Id, x.Fullname }).ToList();
            var operations = _context.Inventory.FirstOrDefault(x => x.Id == inventoryId)
                .Operations.Select(x => new InventoryOperationViewModel
                {
                    Count = x.Count,
                    CurrentCount = x.CurrentCount,
                    Id = x.Id,
                    Description = x.Description,
                    Operation = x.Operation,
                    OperationDate = x.OperationDate.ToFarsi(),
                    OperatorId = x.OperatorId,
                    OrderId = x.OrderId
                }).ToList();
            foreach (var operation in operations)
            {
                operation.Operator = accounts.FirstOrDefault(x => x.Id == operation.OperatorId)?.Fullname;
            }

            return operations;
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            var products = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();

            var query = _context.Inventory.Select(x => new InventoryViewModel
            {
                Id = x.Id,
                InStock = x.InStock,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice,
                CreationDate = x.CreationDate.ToFarsi(),
                CurrentCount = x.CalculateCurrentCount()
            });

            if (searchModel.ProductId > 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            if (searchModel.InStock)
                query = query.Where(x => !x.InStock);

            var inventory = query.OrderByDescending(x => x.Id).ToList();

            inventory.ForEach(x => x.Product = products.FirstOrDefault(p => p.Id == x.ProductId)?.Name);

            return inventory;
        }
    }
}
