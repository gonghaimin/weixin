using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;

namespace Weixin.Tool.Messages.MsgHelpers
{
    /// <summary>
    /// 事件帮助类
    /// </summary>
    public class EventHelper
    {
        public static Event GetEventType(XDocument doc)
        {
            return EventHelper.GetEventType(doc.Root.Element("Event").Value);
        }

        public static Event GetEventType(string str)
        {
            return (Event)Enum.Parse(typeof(Event), str, true);
        }
    }
}
