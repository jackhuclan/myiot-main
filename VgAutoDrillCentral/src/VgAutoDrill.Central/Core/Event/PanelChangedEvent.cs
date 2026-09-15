using Mediator.Net.Contracts;
using VgAutoDrill.Central.Core.Domain;

namespace VgAutoDrill.Central.Core.Event;

public class PanelChangedEvent : IEvent
{
    public PanelChangedEvent(Location location)
    {
        ChangedLocation = location;
    }

    public Location ChangedLocation { get; set; }
}
