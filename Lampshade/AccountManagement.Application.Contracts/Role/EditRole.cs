using System.Collections.Generic;

namespace AccountManagement.Application.Contracts.Role
{
    public class EditRole : CreateRole
    {
        public long Id { get; set; }
        public List<string> Permissions { get; set; }
    }
}
