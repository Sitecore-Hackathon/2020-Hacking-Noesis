using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.Security.Accounts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Globalization;
using Sitecore.Data;
using System;
using Sitecore.Configuration;

namespace Feature.Accounts.SubmitActions
{
    public class RegisterSubmit : SubmitActionBase<string>
    {
        public Database Master { get; }
        public Database Web { get; }
        public RegisterSubmit(ISubmitActionData submitActionData) : base(submitActionData)
        {
            Master = Factory.GetDatabase("master");
            Web = Factory.GetDatabase("web");
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value" /> to an instance of the specified target type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="target">The target object.</param>
        /// <returns>
        /// true if <paramref name="value" /> was converted successfully; otherwise, false.
        /// </returns>
        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));

            if (formSubmitContext.Canceled || formSubmitContext.HasErrors)
                return false;

            string username = string.Empty;
            string password = string.Empty;
            string email = string.Empty;

            foreach (IViewModel field in formSubmitContext.Fields)
            {
                Assert.ArgumentNotNull((object)field, "postedField");

                IValueField valueField = field as IValueField;

                if (valueField != null)
                {
                    PropertyInfo fieldTitle = field.GetType().GetProperty("Title");
                    PropertyInfo fieldValue = field.GetType().GetProperty("Value");


                    if ((object)fieldTitle == null)
                    {
                        return false;
                    }
                    else
                    {

                        if (fieldTitle.GetValue((object)field).ToString() == "Username")
                        {
                            username = fieldValue.GetValue((object)field).ToString();
                        }

                        if (fieldTitle.GetValue((object)field).ToString() == "Password")
                        {
                            password = fieldValue.GetValue((object)field).ToString();
                        }

                        if (fieldTitle.GetValue((object)field).ToString() == "Email")
                        {
                            email = fieldValue.GetValue((object)field).ToString();
                        }

                    }
                }
            }

            if (!User.Exists(@"mydomain\" + username))
            {
                System.Web.Security.Membership.CreateUser(@"mydomain\" + username, password, email);
                using (new Sitecore.SecurityModel.SecurityDisabler()) { 
                    CreateItem(username,email);
                }
                return true;
            }
            else {
                formSubmitContext.Errors.Add(new FormActionError
                {
                    ErrorMessage = "OhOh! You have some problems!!"
                });
                return false;
            }
        }

        private static void CreateItem(string username, string email) {

            string parentItemQuery = Context.Site.RootPath + "/*[@@templatename='Home']/*[@@templatename='Users']";
            Item parentItem = Context.Database.SelectSingleItem(parentItemQuery);

            var templateId = new TemplateID(Templates.User.ID);

            string itemName = ProposeValidItemName(username);

            var item = parentItem?.Children?.FirstOrDefault(c => c.Name.Equals(itemName, StringComparison.InvariantCultureIgnoreCase));

            if (item == null)
            {
                item = parentItem.Add(itemName, templateId);
            }

            UpdateData(item, username, email);
        }

        public static string ProposeValidItemName(string text, bool useSpaces = true)
        {
            text = RemoveDiacritics(text);

            text = ItemUtil.ProposeValidItemName(text);

            if (!useSpaces)
            {
                text = text.Replace(" ", "-");
            }

            return text;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static void UpdateData(Item item, string username, string email)
        {

            try
            {
                item.Editing.BeginEdit();

                item[Sitecore.FieldIDs.DisplayName] = username;
                item[Templates.User.Fields.Username] = username;
                item[Templates.User.Fields.Email] = email;

                item.Editing.EndEdit();
            }
            catch (Exception ex)
            {
                item.Editing.CancelEdit();
                throw ex;
            }
        }
    }
}