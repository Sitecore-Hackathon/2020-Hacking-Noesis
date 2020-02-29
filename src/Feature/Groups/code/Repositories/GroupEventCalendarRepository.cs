using Sitecore.Data.Items;
using Sitecore.XA.Feature.Events.Models;
using System.Collections.Generic;

namespace Feature.Groups.Repositories
{
    public class GroupEventCalendarRepository : Sitecore.XA.Feature.Events.Repositories.EventCalendarRepository, IGroupEventCalendarRepository
    {
        protected override CalendarSettings GetCalendarSettings()
        {
            CalendarSettings calendarSettings = base.GetCalendarSettings();
            //overriding the way EventItems is populated, check GetCalendarSettings
            if (calendarSettings.Mode == CalendarMode.Json)
            {
                Item item;
                if (this.Rendering.DataSourceItem == null)
                {
                    item = this.Context.Item;
                }
                else
                {
                    item = this.Rendering.DataSourceItem;
                }
                this.EventItems = (IEnumerable<Item>)item.Axes.SelectItems("//*[@@templateid = '" + Sitecore.XA.Feature.Events.Templates.CalendarEvent.ID + "']");
            }

            return calendarSettings;
        }
    }

}
