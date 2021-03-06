using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;
using System;
using System.Collections.Generic;

namespace ShopManagement.Application
{
    public class ProductCategoryApplication : IProductCategoryApplication
    {
        private readonly IProductCategoryRepository _prodcutCategoryRepository;
        private readonly IFileUploader _fileUploader;

        public ProductCategoryApplication(IProductCategoryRepository prodcutCategoryRepository, IFileUploader fileUploader)
        {
            _prodcutCategoryRepository = prodcutCategoryRepository;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateProductCategory command)
        {
            var operation = new OperationResult();
            if (_prodcutCategoryRepository.Exists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = GenerateSlug.Slugify(command.Slug);
            //var slug = command.Slug.Slugify();
            var picturePath = _fileUploader.Upload(command.Picture, slug);

            var productCategory = new ProductCategory(command.Name, command.Description, picturePath,
                command.PictureAlt, command.PictureTitle, slug, command.MetaDescription, command.Keywords);

            _prodcutCategoryRepository.Create(productCategory);
            _prodcutCategoryRepository.SaveChanges();

            return operation.Successed();
        }

        public OperationResult Edit(EditProductCategory command)
        {
            var operation = new OperationResult();
            var productCategory = _prodcutCategoryRepository.Get(command.Id);
            if (productCategory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_prodcutCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = GenerateSlug.Slugify(command.Slug);
            //var path = command.Slug;
            var picturePath = _fileUploader.Upload(command.Picture, slug);

            productCategory.Edit(command.Name, command.Description, picturePath, command.PictureAlt,
                command.PictureTitle, slug, command.MetaDescription, command.Keywords);
            _prodcutCategoryRepository.SaveChanges();

            return operation.Successed();
        }

        public EditProductCategory GetDetails(long id)
        {
            return _prodcutCategoryRepository.GetDetails(id);
        }

        public List<ProductCategoryViewModel> GetProductCategories()
        {
            return _prodcutCategoryRepository.GetProductCategories();
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            return _prodcutCategoryRepository.Search(searchModel);
        }
    }
}
