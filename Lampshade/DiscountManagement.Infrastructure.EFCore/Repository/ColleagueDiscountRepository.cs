using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Application.Contracts.ColleagueDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using ShopManagement.Infrastructure.EFCore;
using System.Collections.Generic;
using System.Linq;

namespace DiscountManagement.Infrastructure.EFCore.Repository
{
    public class ColleagueDiscountRepository : RepositoryBase<long, ColleagueDiscount>, IColleagueDiscountRepository
    {
        private readonly DiscountContext _context;
        private readonly ShopContext _shopContext;
        public ColleagueDiscountRepository(DiscountContext context, ShopContext shopContext) : base(context)
        {
            _context = context;
            _shopContext = shopContext;
        }

        public List<ColleagueDiscountViewModel> GetAll()
        {
            return _context.ColleagueDiscounts.Select(x => new ColleagueDiscountViewModel
            {
                CreationDate = x.CreationDate.ToFarsi(),
                DiscountRate = x.DiscountRate,
                ProductId = x.ProductId
            }).ToList();
        }

        public EditColleagueDiscount GetDetails(long id)
        {
            return _context.ColleagueDiscounts.Select(x => new EditColleagueDiscount
            {
                DiscountRate = x.DiscountRate,
                Id = x.Id,
                ProductId = x.ProductId
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel searchModel)
        {
            var products = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();

            var query = _context.ColleagueDiscounts.Select(x => new ColleagueDiscountViewModel
            {
                ProductId = x.ProductId,
                CreationDate = x.CreationDate.ToFarsi(),
                DiscountRate = x.DiscountRate,
                Id = x.Id,
                IsRemoved = x.IsRemoved
            });

            if (searchModel.ProductId > 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            var discounts = query.OrderByDescending(x => x.ProductId).ToList();

            discounts.ForEach(dis => dis.Product = products.FirstOrDefault(x => x.Id == dis.ProductId)?.Name);

            return discounts;
        }
    }
}
