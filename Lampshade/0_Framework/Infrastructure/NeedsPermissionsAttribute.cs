using System;

namespace _0_Framework.Infrastructure
{
    public class NeedsPermissionsAttribute:Attribute
    {
        public int Permission { get; set; }

        public NeedsPermissionsAttribute(int permission)
        {
            Permission = permission;
        }
    }
}
