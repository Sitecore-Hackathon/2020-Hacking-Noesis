using Feature.Groups.Repositories;
using Sitecore.XA.Feature.Events.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace Feature.Groups.Controllers
{
    public class GroupEventCalendarController : StandardController
    {
        public IGroupEventCalendarRepository GroupEventCalendarRepository { get; set; }
        public GroupEventCalendarController()
        {
            GroupEventCalendarRepository = new GroupEventCalendarRepository();
        }
        
        protected override object GetModel()
        {
            return (object)this.GroupEventCalendarRepository.GetModel();
        }
    }
}
