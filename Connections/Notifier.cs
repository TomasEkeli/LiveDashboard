using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;

namespace LiveDashboard.Web.Connections
{
    public class Notifier
    {
        readonly List<Group> groups = new List<Group>();

        public void AddGroup(Group toAdd)
        {
            if (groups.All(g => g.Name != toAdd.Name))
            {
                groups.Add(toAdd);
            }
        }

        public void Notify()
        {
            var context = GlobalHost.ConnectionManager.GetConnectionContext<RandomValue>();

            foreach (var group in groups)
            {
                var number = new Random().Next(group.Min, group.Max);
                context.Groups.Send(group.Name, number);
            }
        }
    }

    public class Group
    {
        public string Name { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}