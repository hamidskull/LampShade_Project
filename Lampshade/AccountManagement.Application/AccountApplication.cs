using _0_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using System.Collections.Generic;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileUploader _fileUploader;
        private readonly IAuthHelper _authHelper;
        private readonly IRoleRepository _roleRepository;

        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher,
            IFileUploader fileUploader, IAuthHelper authHelper, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _fileUploader = fileUploader;
            _authHelper = authHelper;
            _roleRepository = roleRepository;
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.Get(command.Id);

            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (command.Password != command.RePassword)
                return operation.Failed(ApplicationMessages.PasswordNotMatch);

            var password = _passwordHasher.Hash(command.Password);
            account.ChangePassword(password);
            _accountRepository.SaveChanges();

            return operation.Successed();
        }

        public OperationResult Register(RegisterAccount command)
        {
            var operation = new OperationResult();
            if (_accountRepository.Exists(x => x.Username == command.Username || x.Mobile == command.Mobile))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var password = _passwordHasher.Hash(command.Password);
            var path = "ProfilePhotos";
            var picturePath = _fileUploader.Upload(command.ProfilePhoto, path);

            //var account = new Account(command.Fullname, command.Username, password, command.Mobile,
            //    command.RoleId, picturePath);

            var account = new Account(command.Fullname, command.Username, password, command.Mobile,
               2, picturePath);

            _accountRepository.Create(account);
            _accountRepository.SaveChanges();

            return operation.Successed();
        }

        public OperationResult Edit(EditAccount command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.Get(command.Id);

            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_accountRepository.Exists(x => (x.Username == command.Username ||
            x.Mobile == command.Mobile) && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var path = "ProfilePhotos";
            var picturePath = _fileUploader.Upload(command.ProfilePhoto, path);

            //for many role
            var accountRoles = new List<AccountRole>();
            command.RoleId.ForEach(x => accountRoles.Add(new AccountRole(command.Id, x)));


            //account.Edit(command.Fullname, command.Username, command.Mobile,
            //   command.RoleId, picturePath,accountRoles);

            account.Edit(command.Fullname, command.Username, command.Mobile,
             2, picturePath, accountRoles);



            _accountRepository.SaveChanges();

            return operation.Successed();
        }

        public EditAccount GetDetails(long id)
        {
            return _accountRepository.GetDetails(id);
        }

        public OperationResult Login(Login command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetBy(command.Username);
            if (account == null)
                return operation.Failed(ApplicationMessages.WrongUserPass);

            var result = _passwordHasher.Check(account.Password, command.Password);

            if (!result.Verified)
                return operation.Failed(ApplicationMessages.WrongUserPass);

            //var permissions = _roleRepository.Get(account.RoleId).Permissions.Select(x => x.Code).ToList();

            //for one role
            //var permissions = _roleRepository.GetRolePermissions(account.RoleId);

            //for many roles
            var permissions = _roleRepository.GetAccountRolePermissions(account.Id);

            //var authViewModel = new AuthViewModel(account.Id, account.RoleId, account.Fullname, account.Username, account.Mobile, permissions);
            var authViewModel = new AuthViewModel(account.Id, account.AccountRoles[0].RoleId, account.Fullname, account.Username, account.Mobile, permissions);
            _authHelper.Signin(authViewModel);

            return operation.Successed();
        }

        public void Logout()
        {
            _authHelper.SignOut();
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            return _accountRepository.Search(searchModel);
        }
    }
}
