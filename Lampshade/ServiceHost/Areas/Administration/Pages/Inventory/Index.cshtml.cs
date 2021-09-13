using InventoryManagement.Application.Contracts.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;

namespace ServiceHost.Areas.Administration.Pages.Inventory
{
    public class IndexModel : PageModel
    {
        public InventorySearchModel SearchModel { get; set; }
        public SelectList productList;
        public List<InventoryViewModel> Inventory;

        private readonly IProductApplication _productApplication;
        private readonly IInventoryApplication _inventoryApplication;
        public IndexModel(IProductApplication productApplication, IInventoryApplication inventoryApplication)
        {
            _productApplication = productApplication;
            _inventoryApplication = inventoryApplication;
        }

        public void OnGet(InventorySearchModel searchModel)
        {
            Inventory = _inventoryApplication.Search(searchModel);
            productList = new SelectList(_productApplication.GetProduct(), "Id", "Name");

        }

        public PartialViewResult OnGetCreate()
        {
            var command = new CreateInventory
            {
                Products = _productApplication.GetProduct()
            };
            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(CreateInventory command)
        {
            var result = _inventoryApplication.Create(command);
            return new JsonResult(result);
        }
        public PartialViewResult OnGetEdit(long id)
        {
            var discount = _inventoryApplication.GetDetails(id);
            discount.Products = _productApplication.GetProduct();

            return Partial("./Edit", discount);
        }
        public JsonResult OnPostEdit(EditInventory command)
        {
            var result = _inventoryApplication.Edit(command);
            return new JsonResult(result);
        }
        public PartialViewResult OnGetIncrease(long id)
        {
            var command = new IncreaseInventory
            {
                InventoryId = id
            };
            return Partial("Increase", command);
        }
        public JsonResult OnPostIncrease(IncreaseInventory command)
        {
            var result = _inventoryApplication.Increase(command);
            return new JsonResult(result);
        }
        public PartialViewResult OnGetReduce(long id)
        {
            var command = new ReduceInventory
            {
                InventoryId = id
            };
            return Partial("Reduce", command);
        }
        public JsonResult OnPostReduce(ReduceInventory command)
        {
            var result = _inventoryApplication.Reduce(command);
            return new JsonResult(result);
        }
        public PartialViewResult OnGetLog(long id)
        {
            var log = _inventoryApplication.GetOperationLog(id);

            return Partial("OperationLog", log);
        }
    }
}
