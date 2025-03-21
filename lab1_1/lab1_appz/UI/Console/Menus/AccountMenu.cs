using lab1_appz.Services;

namespace lab1_appz.UI.Console.Menus
{
    public class AccountMenu
    {
        private readonly IConsoleView _view;
        private readonly ValidationService _validationService;
        private readonly AccountService _accountService;
        private readonly EventSystem _eventSystem;

        public AccountMenu(IConsoleView view, ValidationService validationService, AccountService accountService, EventSystem eventSystem)
        {
            _view = view;
            _validationService = validationService;
            _accountService = accountService;
            _eventSystem = eventSystem;
        }

        public void Show()
        {
            bool exit = false;
            while (!exit)
            {
                _view.Clear();
                _view.DisplayHeader("Керування обліковими записами");
                string currentAccount = _accountService.CurrentAccount != null 
                    ? $"Поточний обліковий запис: {_accountService.CurrentAccount}" 
                    : "Ви не увійшли в обліковий запис";
                _view.DisplayInfo(currentAccount);
                _view.DisplayLine();

                var options = new List<string>
                {
                    "Показати всі облікові записи",
                    "Створити новий обліковий запис",
                    "Увійти в обліковий запис",
                    "Вийти з облікового запису",
                    "Повернутися до головного меню"
                };

                _view.DisplayMenu(options);

                int choice = _validationService.ParseInt(
                    _view.GetInput("Виберіть опцію: "), 
                    1, 
                    options.Count, 
                    defaultValue: options.Count);

                switch (choice)
                {
                    case 1:
                        ShowAllAccounts();
                        break;
                    case 2:
                        CreateAccount();
                        break;
                    case 3:
                        Login();
                        break;
                    case 4:
                        Logout();
                        break;
                    case 5:
                        exit = true;
                        break;
                }
            }
        }

        public void ShowAllAccounts()
        {
            _view.Clear();
            _view.DisplayHeader("Всі облікові записи");
            var accounts = _accountService.GetAllAccounts();
            if (accounts.Count == 0)
            {
                _view.DisplayInfo("У системі немає жодного облікового запису.");
                _view.WaitForKeyPress();
                return;
            }
            for (int i = 0; i < accounts.Count; i++)
            {
                string current = _accountService.CurrentAccount == accounts[i] ? " (поточний)" : "";
                _view.DisplayInfo($"{i + 1}. {accounts[i]}{current}");
            }

            _view.WaitForKeyPress();
        }

        public void CreateAccount()
        {
            _view.Clear();
            _view.DisplayHeader("Створення нового облікового запису");

            string username = _view.GetInput("Введіть логін: ");
            if (!_validationService.IsValidName(username))
            {
                _view.DisplayError("Логін повинен містити від 3 до 20 символів і складатися з літер, цифр та символів _ або -");
                _view.WaitForKeyPress();
                return;
            }

            bool isPremium = _validationService.ParseBool(_view.GetInput("Бажаєте створити преміум-акаунт? (так/ні): "));

            var account = _accountService.CreateAccount(username, isPremium);
            if (account != null)
            {
                _view.DisplaySuccess($"Обліковий запис успішно створено: {account}");
                
                // Запитати користувача, чи хоче він увійти в новостворений акаунт
                bool login = _validationService.ParseBool(_view.GetInput("Бажаєте увійти в новостворений акаунт? (так/ні): "));
                if (login)
                {
                    _accountService.Login(username);
                }
            }
            else
            {
                _view.DisplayError($"Не вдалося створити обліковий запис з логіном '{username}'");
            }
            
            _view.WaitForKeyPress();
        }


        public void Login()
        {
            _view.Clear();
            _view.DisplayHeader("Вхід в обліковий запис");

            if (_accountService.CurrentAccount != null)
            {
                _view.DisplayInfo($"Ви вже увійшли в обліковий запис: {_accountService.CurrentAccount}");
                bool logout = _validationService.ParseBool(_view.GetInput("Бажаєте вийти з поточного облікового запису? (так/ні): "));
                if (logout)
                {
                    _accountService.Logout();
                }
                else
                {
                    _view.WaitForKeyPress();
                    return;
                }
            }

            var accounts = _accountService.GetAllAccounts();
            if (accounts.Count == 0)
            {
                _view.DisplayInfo("У системі немає жодного облікового запису. Спочатку створіть обліковий запис.");
                _view.WaitForKeyPress();
                return;
            }

            _view.DisplayInfo("Доступні облікові записи:");
            for (int i = 0; i < accounts.Count; i++)
            {
                _view.DisplayInfo($"{i + 1}. {accounts[i]}");
            }

            int accountIndex = _validationService.ParseInt(
                _view.GetInput("Виберіть обліковий запис (номер) або 0 для скасування: "), 
                0, 
                accounts.Count, 
                defaultValue: 0);

            if (accountIndex == 0)
            {
                return;
            }

            var selectedAccount = accounts[accountIndex - 1];
            if (_accountService.Login(selectedAccount.Username))
            {
                _view.DisplaySuccess($"Ви увійшли в обліковий запис: {selectedAccount}");
            }
            else
            {
                _view.DisplayError($"Не вдалося увійти в обліковий запис: {selectedAccount}");
            }
            
            _view.WaitForKeyPress();
        }

        public void Logout()
        {
            _view.Clear();
            _view.DisplayHeader("Вихід з облікового запису");

            if (_accountService.CurrentAccount == null)
            {
                _view.DisplayInfo("Ви не увійшли в обліковий запис");
                _view.WaitForKeyPress();
                return;
            }

            _view.DisplayInfo($"Ви зараз увійшли як: {_accountService.CurrentAccount}");
            bool confirm = _validationService.ParseBool(_view.GetInput("Підтвердіть вихід (так/ні): "));
            
            if (confirm)
            {
                _accountService.Logout();
                _view.DisplaySuccess("Ви вийшли з облікового запису");
            }
            
            _view.WaitForKeyPress();
        }
    }
} 