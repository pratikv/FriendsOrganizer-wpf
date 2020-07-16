using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FriendsOrganizer.Model;
using FriendsOrganizer.UI.ViewModels;

namespace FriendsOrganizer.UI.Wrapper
{
    public class FriendWrapper : ModelWrapper<Friend>
    {
        public FriendWrapper(Friend model)
            :base(model)
        {

        }

        public int Id => Model.Id;

        public string FirstName
        {
            get => GetValue<string>();
            set
            {
                SetValue(value);
                //ValidateProperty(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int? FavouriteLanguageId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "Robot", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Robots are not valid friends";
                    }
                    break;
            }
        }
    }

    public class NotifyErrorInfoBase : ViewModelBase, INotifyDataErrorInfo
    {
        private IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public IEnumerable GetErrors(string propertyName)
        {
            return errors.ContainsKey(propertyName) ? errors[propertyName] : null;
        }

        public bool HasErrors => errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            base.OnPropertyChanged(nameof(HasErrors));
        }

        protected void AddError(string propertyName, string error)
        {
            if (!errors.ContainsKey(propertyName))
            {
                errors[propertyName] = new List<string>();
            }

            if (!errors[propertyName].Contains(error))
            {
                errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }

        }

    }

    public class FriendPhoneNumberWrapper : ModelWrapper<FriendPhoneNumber>
    {
        public FriendPhoneNumberWrapper(FriendPhoneNumber model) : base(model)
        {
        }

        public string Number
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}
