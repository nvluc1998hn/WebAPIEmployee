using Base.Common.Attributes;
using Base.Common.Extensions;
using Base.Common.LanguageKeys;
using FluentValidation;
using System.Reflection;
using Utilities.Helpers;

namespace Base.Common.Validation
{
    /// <summary>Hàm dùng chung valid các input mức cơ bản</summary>
    /// <typeparam name="T">Class truyền vào</typeparam>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 15/11/2023 created
    /// </Modified>
    public class BaseValidator<T> : AbstractValidator<T> where T : class
    {

        public BaseValidator()
        {

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {

                var stringAttribute = property.GetCustomAttribute<ValidStringAttribute>();

                if (stringAttribute != null)
                {
                    RuleFor(c => (string)property.GetValue(c)).NotEmpty().WithMessage(LanguageKeycs.ValidateCannotBeEmpty.CreateClientMessage(property.Name))
                       .Matches(StringHelper.REGEX_SPECIAL_CHARATER).WithMessage(LanguageKeycs.ValidateCannotContainSpecialCharacter.CreateClientMessage(property.Name));

                }
                var numberAttribute = property.GetCustomAttribute<ValidNumberAttribute>();

                if (numberAttribute != null)
                {
                    RuleFor(c => Convert.ToDouble(property.GetValue(c))).GreaterThanOrEqualTo(0).
                        WithMessage(LanguageKeycs.ValidateGreaterThanZero.CreateClientMessage(property.Name));

                }

                var maxNumberAtribute = property.GetCustomAttribute<ValidNumberMaxAttribute>();

                if (maxNumberAtribute != null)
                {
                    int maxValue = maxNumberAtribute.MaxValue;
                    RuleFor(c => Convert.ToDouble(property.GetValue(c))).LessThanOrEqualTo(maxValue)
                        .WithMessage(LanguageKeycs.ValidateMustLessThan.CreateClientMessage(property.Name, maxValue));

                }

                var phoneAttribute = property.GetCustomAttribute<ValidPhoneAttribute>();
                if (phoneAttribute != null)
                {
                    RuleFor(c => (string)property.GetValue(c))
                       .Matches(StringHelper.REGEX_PHONE_NUMBER).WithMessage(LanguageKeycs.ValidatePhoneNumber.CreateClientMessage(property.Name));
                }

                var emailAttribute = property.GetCustomAttribute<ValidEmailAttribute>();

                if (emailAttribute != null)
                {
                    RuleFor(c => (string)property.GetValue(c)).EmailAddress().When(c => !string.IsNullOrEmpty((string)property.GetValue(c)))
                    .WithMessage(LanguageKeycs.ValidateEmail.CreateClientMessage(property.Name));
                }

            }

        }



    }
}
