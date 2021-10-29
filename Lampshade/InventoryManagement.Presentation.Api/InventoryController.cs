using _01_LampshadeQuery.Contracts.Inventory;
using InventoryManagement.Application.Contracts.Inventory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace InventoryManagement.Presentation.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryQuery _inventoryQuery;

        public InventoryController(IInventoryQuery inventoryQuery)
        {
            _inventoryQuery = inventoryQuery;
        }

        [HttpPost]
        public StockStatus CheckStock(IsInStock command)
        {
            return _inventoryQuery.CheckStock(command);
        }
    }
}
