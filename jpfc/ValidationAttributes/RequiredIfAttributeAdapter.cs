using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;

namespace jpfc.ValidationAttributes
{
    public class RequiredIfAttributeAdapter : AttributeAdapterBase<RequiredIfAttribute>
    {
        private readonly RequiredIfAttribute _attribute;

        public RequiredIfAttributeAdapter(RequiredIfAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
            _attribute = attribute;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-requiredif", GetErrorMessage(context));

            MergeAttribute(context.Attributes, "data-val-requiredif-prop", _attribute.Property);
            MergeAttribute(context.Attributes, "data-val-requiredif-hasvalue", _attribute.HasValue == null ? "" : _attribute.HasValue.ToString());
            MergeAttribute(context.Attributes, "data-val-requiredif-nothasvalue", _attribute.NotHasValue == null ? "" : _attribute.NotHasValue.ToString());
        }
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return GetErrorMessage(validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
