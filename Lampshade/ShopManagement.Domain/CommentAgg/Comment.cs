﻿using _0_Framework.Domain;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Domain.CommentAgg
{
    public class Comment : EntityBase
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Message { get; private set; }
        public bool IsConfirmed { get; private set; }
        public bool IsCanceled { get; private set; }
        public long ProductId { get; private set; }
        public Product Product { get; private set; }

        protected Comment() { }

        public Comment(string name, string email, string message, long productId)
        {
            Name = name;
            Email = email;
            Message = message;
            ProductId = productId;
        }
        public void Confirm()
        {
            IsConfirmed = true;
            IsCanceled = false;
        }
        public void Cancel()
        {
            IsCanceled = true;
            IsConfirmed = false;
        }
    }
}
