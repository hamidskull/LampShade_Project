﻿using _0_Framework.Domain;
using AccountManagement.Domain.AccountAgg;
using System.Collections.Generic;

namespace AccountManagement.Domain.RoleAgg
{
    public class Role : EntityBase
    {
        public string Name { get; private set; }
        public List<Account> Accounts { get; private set; }
        public List<Permission> Permissions { get; private set; }

        //for many role
        public List<AccountRole> AccountRoles { get; private set; }


        protected Role() { }
        public Role(string name, List<Permission> permissions)
        {
            Name = name;
            Accounts = new List<Account>();
            Permissions = permissions;

            AccountRoles = new List<AccountRole>();
        }
        public void Edit(string name, List<Permission> permissions)
        {
            Name = name;
            Permissions = permissions;
        }
    }
}
