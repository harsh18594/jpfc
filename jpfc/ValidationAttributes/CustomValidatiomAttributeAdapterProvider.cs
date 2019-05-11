using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace jpfc.ValidationAttributes
{
    public class CustomValidatiomAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            switch (attribute)
            {
                case RequiredIfAttribute _:
                    return new RequiredIfAttributeAdapter(attribute as RequiredIfAttribute, stringLocalizer);
                default:
                    return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
            }
        }
    }
}
