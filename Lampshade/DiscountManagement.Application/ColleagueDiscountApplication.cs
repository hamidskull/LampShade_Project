using _0_Framework.Application;
using DiscountManagement.Application.Contracts.ColleagueDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using System.Collections.Generic;

namespace DiscountManagement.Application
{
    public class ColleagueDiscountApplication : IColleagueDiscountApplication
    {
        private readonly IColleagueDiscountRepository _colleagueDiscountRepository;
        public ColleagueDiscountApplication(IColleagueDiscountRepository colleagueDiscountRepository)
        {
            _colleagueDiscountRepository = colleagueDiscountRepository;
        }

        public OperationResult Create(CreateColleagueDiscount command)
        {
            var operationResult = new OperationResult();
            if (_colleagueDiscountRepository.Exists(x => x.ProductId == command.ProductId &&
            x.DiscountRate == command.DiscountRate))
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

            var discount = new ColleagueDiscount(command.ProductId, command.DiscountRate);
            _colleagueDiscountRepository.Create(discount);
            _colleagueDiscountRepository.SaveChanges();

            return operationResult.Successed();

        }

        public OperationResult Edit(EditColleagueDiscount command)
        {
            var operationResult = new OperationResult();
            var discount = _colleagueDiscountRepository.Get(command.Id);
            if (discount == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);

            if (_colleagueDiscountRepository.Exists(x => x.ProductId == command.ProductId &&
             x.DiscountRate == command.DiscountRate && x.Id != command.Id))
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

            discount.Edit(command.ProductId, command.DiscountRate);
            _colleagueDiscountRepository.SaveChanges();

            return operationResult.Successed();

        }

        public List<ColleagueDiscountViewModel> GetAll()
        {
            return _colleagueDiscountRepository.GetAll();
        }

        public EditColleagueDiscount GetDetails(long id)
        {
            return _colleagueDiscountRepository.GetDetails(id);
        }

        public OperationResult Remove(long id)
        {
            var operationResult = new OperationResult();
            var discount = _colleagueDiscountRepository.Get(id);
            if (discount == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);

            discount.Remove();
            _colleagueDiscountRepository.SaveChanges();

            return operationResult.Successed();
        }

        public OperationResult Restore(long id)
        {
            var operationResult = new OperationResult();
            var discount = _colleagueDiscountRepository.Get(id);
            if (discount == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);

            discount.Restore();
            _colleagueDiscountRepository.SaveChanges();

            return operationResult.Successed();
        }

        public List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel searchModel)
        {
            return _colleagueDiscountRepository.Search(searchModel);
        }
    }
}
