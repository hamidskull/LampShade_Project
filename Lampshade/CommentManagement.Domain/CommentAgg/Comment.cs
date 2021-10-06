using _0_Framework.Domain;
using System.Collections.Generic;

namespace CommentManagement.Domain.CommentAgg
{
    public class Comment : EntityBase
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Website { get; private set; }
        public string Message { get; private set; }
        public bool IsConfirmed { get; private set; }
        public bool IsCanceled { get; private set; }
        public long OwnerRecordId { get; private set; }
        public long ParentId { get; private set; }
        public int Type { get; private set; }
        public Comment Parent { get; private set; }
        public List<Comment> Childern { get; private set; }

        protected Comment() { }

        public Comment(string name, string email, string website, string message, long ownerRecordId,
            long parentId, int type)
        {
            Name = name;
            Email = email;
            Website = website;
            Message = message;
            OwnerRecordId = ownerRecordId;
            ParentId = parentId;
            Type = type;
            Childern = new List<Comment>();
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
