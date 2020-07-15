using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace FriendsOrganizer.UI.Wrapper
{
    public class ModelWrapper<T> : NotifyErrorInfoBase
    {
        public T Model { get; }

        public ModelWrapper(T model)
        {
            Model = model;
        }


        protected virtual TValue GetValue<TValue>([CallerMemberName]string propertyName = null)
        {
            return (TValue) typeof(T).GetProperty(propertyName).GetValue(Model);
        }
        protected virtual void SetValue<TValue>(TValue value,[CallerMemberName]string propertyName = null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, value);
            OnPropertyChanged(propertyName);
            ValidatePropertyInternal(propertyName, value);
        }

        private void ValidatePropertyInternal(string propertyName, object value)
        {
            ClearErrors(propertyName);

            //1. Validate data annotations
            ValidateDataAnnotations(propertyName, value);

            ValidateCustomErrors(propertyName);
        }

        private void ValidateDataAnnotations(string propertyName, object value)
        {
            var contxt = new ValidationContext(Model) {MemberName = propertyName};
            ICollection<ValidationResult> results = new List<ValidationResult>();
            Validator.TryValidateProperty(value, contxt, results);

            foreach (var result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }
        }

        private void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
    }
}