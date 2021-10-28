﻿using _0_Framework.Domain;
using System.Collections.Generic;

namespace ShopManagement.Domain.OrderAgg
{
    public class Order : EntityBase
    {
        public long AccountId { get; private set; }
        public int PaymentMethod { get; private set; }
        public double TotalAmount { get; private set; }
        public double DiscountAmount { get; private set; }
        public double PayAmount { get; private set; }
        public bool IsPaid { get; private set; }
        public bool IsCanceled { get; private set; }
        public string IssueTrackingNo { get; private set; }
        public long RefId { get; private set; }
        public List<OrderItem> Items { get; private set; }

        public Order(long accountId, int paymentMethod, double totalAmount, double discountAmount,
            double payAmount)//, string issueTrackingNo, List<OrderItem> items)
        {
            AccountId = accountId;
            PaymentMethod = paymentMethod;
            TotalAmount = totalAmount;
            DiscountAmount = discountAmount;
            PayAmount = payAmount;
            //IssueTrackingNo = issueTrackingNo;
            //Items = items;
            Items = new List<OrderItem>();
            IsPaid = false;
            IsCanceled = false;
            RefId = 0;
        }
        public void AddItem(OrderItem item)
        {
            Items.Add(item);
        }
        public void PaymentSucceded(long refId)
        {
            if (refId != 0)
            {
                IsPaid = true;
                RefId = refId;
            }
        }
        public void Cancel()
        {
            IsCanceled = true;
        }
        public void SetIssueTrackingNo(string number)
        {
            IssueTrackingNo = number;
        }
    }
}
