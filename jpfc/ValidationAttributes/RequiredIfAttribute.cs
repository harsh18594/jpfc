using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.ValidationAttributes
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        public string Property { get; set; }
        public object HasValue { get; set; }
        public object NotHasValue { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get the value of property from the model
            var modelValue = validationContext.ObjectInstance.GetType().GetProperty(Property).GetValue(validationContext.ObjectInstance);

            // The value of the model property needs to match the value set for HasValue
            //  In that case, validation will fail if the value parameter is null
            // Check if HasValue contains a comma and split the values so each can be compared
            bool valid = true;
            if (NotHasValue != null)
            {
                var strNotHasValue = NotHasValue.ToString();
                if (strNotHasValue.Contains(","))
                {
                    bool matchFound = false;
                    foreach (var val in strNotHasValue.Split(","))
                    {
                        if (modelValue != null)
                        {
                            if (IsNumber(val))
                            {
                                modelValue = modelValue.ToString();
                            }

                            if (modelValue.Equals(val))
                            {
                                matchFound = true;
                            }
                        }
                    }

                    if (!matchFound && value == null)
                    {
                        valid = false;
                    }
                }
                else
                {
                    if (modelValue != null && !modelValue.Equals(NotHasValue) && value == null)
                    {
                        valid = false;
                    }

                    if (string.Equals(modelValue?.ToString(), NotHasValue?.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        valid = true;
                    }
                }
            }
            else
            {
                if (HasValue != null)
                {
                    var strHasValue = HasValue.ToString();
                    if (strHasValue.Contains(","))
                    {
                        bool matchFound = false;
                        foreach (var val in strHasValue.Split(","))
                        {
                            if (modelValue != null)
                            {
                                if (IsNumber(val))
                                {
                                    modelValue = modelValue.ToString();
                                }

                                if (modelValue.Equals(val))
                                {
                                    matchFound = true;
                                }
                            }
                        }

                        if (matchFound && value == null)
                        {
                            valid = false;
                        }
                    }
                    else
                    {
                        if (modelValue != null && modelValue.Equals(HasValue) && value == null)
                        {
                            valid = false;
                        }
                    }
                }
                else
                {
                    if (modelValue != null && modelValue.Equals(HasValue) && value == null)
                    {
                        valid = false;
                    }
                }
            }

            return valid ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }

        private static bool IsNumber(object value)
        {
            return value is sbyte
                   || value is byte
                   || value is short
                   || value is ushort
                   || value is int
                   || value is uint
                   || value is long
                   || value is ulong
                   || value is float
                   || value is double
                   || value is decimal;
        }
    }
}
