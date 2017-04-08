using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Saferide.Helpers;
using Saferide.Models;
using Xamarin.Forms;

namespace Saferide.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _password;
        private string _confirmPassword;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand RegisterClickedCommand { get; set; }

        public RegisterViewModel()
        {
            RegisterClickedCommand = new Command(Register);
        }

        private async void Register()
        {
            bool isMailValid;
            try
            {
                Regex rx = new Regex(
            @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
                isMailValid = rx.IsMatch(_email);
            }
            catch (FormatException)
            {
                isMailValid = false;
            }
            bool arePasswordTheSame = _password == _confirmPassword;
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{6,}");
            var isPasswordValid = hasNumber.IsMatch(_password) && hasUpperChar.IsMatch(_password) && hasMinimum8Chars.IsMatch(_password);
            if (_firstName != null &&
                _lastName != null &&
                _email != null &&
                _confirmPassword != null &&
                _password != null)
            {
                if (isMailValid)
                {
                    if (isPasswordValid)
                    {
                        if (arePasswordTheSame)
                        {
                            var user = new NewUser()
                            {
                                Firstame = _firstName,
                                Lastname = _lastName,
                                Email = _email,
                                Password = _password,
                                ConfirmPassword = _confirmPassword
                            };
                            var result = await App.LoginManager.Register(user);
                            switch (result)
                            {
                                case "Success":
                                    XFToast.ShortMessage("Bravo tu es maintenant enregistré!");
                                    break;
                            }
                        }
                        else
                        {
                            XFToast.LongMessage("Les deux mots de passes ne sont pas identiques");
                        }
                    }
                    else
                    {
                        XFToast.LongMessage(
                            "Le mot de passe doit faire au moins 8 caractères avec une lettre majuscule, miniscule et un chiffre");
                    }
                }
                else
                {
                    XFToast.LongMessage("Entrez une email valide");
                }
            }
            else
            {
                XFToast.LongMessage("Tous les champs doivent être complétés");
            }
        }
    }
}