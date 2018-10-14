using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Command and Observer patterns, right? :)
public interface IEvent
{
}

public interface IEventListener
{
    void Receive(IEvent InEvent);
}

public class EventSystem
{
    public Dictionary<string, List<IEventListener>> Listeners {get; private set;}

    public EventSystem()
    {
        Listeners = new Dictionary<string, List<IEventListener>>();
    }

    public void Unsubscribe(string InName, IEventListener InListener)
    {
        //Unsubscribe here
    }

    public void Subscribe(string InName, IEventListener InListener) //ofc here we need a template with [where T : IEvent] and etc but, I have no enough time.
    {
        if (!Listeners.ContainsKey(InName))
        {
            Listeners.Add(InName, new List<IEventListener>());
        }

        Listeners[InName].Add(InListener);
    }

    public void Broadcast(string InName, IEvent InEvent)
    {
        if (!Listeners.ContainsKey(InName)) return;

        foreach(IEventListener Listener in Listeners[InName])
        {
            Listener.Receive(InEvent);
        }
    }
}
