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
    public class CreateMeeting : SubmitActionBase<string>
    {

        public CreateMeeting(ISubmitActionData submitActionData) : base(submitActionData)
        {
           
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

            string name = string.Empty;
            string description = string.Empty;
            string location = string.Empty;

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
                        if (fieldTitle.GetValue((object)field).ToString() == "Name")
                        {
                            name = fieldValue.GetValue((object)field).ToString();
                        }

                        if (fieldTitle.GetValue((object)field).ToString() == "Description")
                        {
                            description = fieldValue.GetValue((object)field).ToString();
                        }

                        if (fieldTitle.GetValue((object)field).ToString() == "Location")
                        {
                            location = fieldValue.GetValue((object)field).ToString();
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(location))
            {
                using (new Sitecore.SecurityModel.SecurityDisabler()) { 
                    CreateItem(name, description, location);
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

        private static void CreateItem(string name, string description, string location) {

            string parentItemQuery = Context.Site.RootPath + "/*[@@templatename='Home']/*[@@templatename='Groups']";
            Item parentItem = Context.Database.SelectSingleItem(parentItemQuery);

            //  var parentItem = Sitecore.Context.Database.GetItem(Templates.Users.ID);
            var templateId = new BranchId(new ID("{4CBA4FE4-E13E-4777-8A46-3310F77FD331}"));

            string itemName = ProposeValidItemName(name);

            var item = parentItem?.Children?.FirstOrDefault(c => c.Name.Equals(itemName, StringComparison.InvariantCultureIgnoreCase));

            if (item == null)
            {
                item = parentItem.Add(itemName, templateId);
            }

            UpdateData(item, name, description, location);
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

        private static void UpdateData(Item item, string name, string description, string location)
        {

            try
            {
                item.Editing.BeginEdit();
                
                var userName = User.Current.LocalName;
                var userId = GetUser(userName);

                item["Title"] = name;
                item["Content"] = description;

                Sitecore.Data.Fields.ReferenceField referenceField = item.Fields["Organizer"];
                referenceField.Value = userId.ToString();

                Sitecore.Data.Fields.MultilistField multiselectField = item.Fields["Members"];
                multiselectField.Add(userId.ToString());

                item.Editing.EndEdit();
            }
            catch (Exception ex)
            {
                item.Editing.CancelEdit();
                throw ex;
            }
        }

        private static ID GetUser(string username)
        {
            string parentItemQuery = Context.Site.RootPath + "/*[@@templatename='Home']/*[@@templatename='Users']";
            Item parentItem = Context.Database.SelectSingleItem(parentItemQuery);

            var item = parentItem?.Children?.FirstOrDefault(c => c.Name.Equals(username, StringComparison.InvariantCultureIgnoreCase));

            if (item == null) return new ID();
            {
                return item.ID;
            }
        }
    }
}