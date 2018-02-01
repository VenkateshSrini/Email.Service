using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMTP.Service.Model.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IsListOfEmailAddress : DataTypeAttribute
    {
        #region privates
        private readonly EmailAddressAttribute _emailAddressAttribute = new EmailAddressAttribute();
        #endregion

        #region ctor
        public IsListOfEmailAddress() : base(DataType.EmailAddress) { }
        #endregion

        #region Overrides
        /// <summary>
        /// Checks if the value is valid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var emailAddressList = value as List<string>;
            if (emailAddressList != null) return false;

            //lets test for mulitple email addresses
            
             return emailAddressList.All(t => _emailAddressAttribute.IsValid(t));
        }
        #endregion

    }
}
