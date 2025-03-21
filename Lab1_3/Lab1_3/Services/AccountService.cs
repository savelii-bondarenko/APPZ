using Lab1_3.Models;

namespace Lab1_3.Services;

public class AccountService
{
    private readonly Dictionary<string, Account> _accounts = new();

    private readonly EventSystem _eventSystem;

    public AccountService(EventSystem eventSystem)
    {
        _eventSystem = eventSystem;

        // Створюємо тестові облікові записи
        CreateDefaultAccounts();
    }

    public Account? CurrentAccount { get; private set; }

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

        CurrentAccount = account;
        _eventSystem.NotifyInformation(this, $"Вхід в обліковий запис: {account}");
        return true;
    }

    public void Logout()
    {
        if (CurrentAccount != null)
        {
            _eventSystem.NotifyInformation(this, $"Вихід з облікового запису: {CurrentAccount}");
            CurrentAccount = null;
        }
    }

    private void CreateDefaultAccounts()
    {
        CreateAccount("user1");
        CreateAccount("premium_user", true);
    }
}