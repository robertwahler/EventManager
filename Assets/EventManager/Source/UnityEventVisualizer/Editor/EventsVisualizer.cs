using System.Collections.Generic;

namespace EventVisualizer.Base
{

    public static class EventsFinder
    {
        public static List<EventCall> FindAllEvents()
        {
            return SDD.Events.EventManager.Instance.Events;
        }
    }
}