using lab1_appz.Models;

namespace lab1_appz.Services
{
    public class AccountService
    {
        private readonly Dictionary<string, Account> _accounts = new();

        private Account? _currentAccount;

        private readonly EventSystem _eventSystem;

        public AccountService(EventSystem eventSystem)
        {
            _eventSystem = eventSystem;
            
            // Створюємо тестові облікові записи
            CreateDefaultAccounts();
        }
        public Account? CurrentAccount => _currentAccount;
        public Account? CreateAccount(string username, bool isPremium = false)
        {
            // Перевіряємо, чи такий логін вже існує
            if (_accounts.ContainsKey(username))
            {
                _eventSystem.NotifyError(this, $"Обліковий запис з логіном '{username}' вже існує");
                return null;
            }

            var account = new Account(username, isPremium);
            _accounts.Add(username, account);
            
            _eventSystem.NotifyInformation(this, $"Створено новий обліковий запис: {account}");
            return account;
        }

        public List<Account> GetAllAccounts()
        {
            return _accounts.Values.ToList();
        }

        public Account? GetAccountByUsername(string username)
        {
            return _accounts.TryGetValue(username, out var account) ? account : null;
        }

        public bool Login(string username)
        {
            var account = GetAccountByUsername(username);
            if (account == null)
            {
                _eventSystem.NotifyError(this, $"Обліковий запис з логіном '{username}' не знайдено");
                return false;
            }

            _currentAccount = account;
            _eventSystem.NotifyInformation(this, $"Вхід в обліковий запис: {account}");
            return true;
        }

        public void Logout()
        {
            if (_currentAccount != null)
            {
                _eventSystem.NotifyInformation(this, $"Вихід з облікового запису: {_currentAccount}");
                _currentAccount = null;
            }
        }

        private void CreateDefaultAccounts()
        {
            CreateAccount("user1", false);
            CreateAccount("premium_user", true);
        }
    }
} 