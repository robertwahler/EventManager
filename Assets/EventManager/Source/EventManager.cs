using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SDD.Events
{

    /// <summary>
    /// Event Manager manages publishing raised events to subscribing/listening classes.
    ///
    /// @example subscribe
    ///     EventManager.Instance.AddListener<SomethingHappenedEvent>(OnSomethingHappened);
    ///
    /// @example unsubscribe
    ///     EventManager.Instance.RemoveListener<SomethingHappenedEvent>(OnSomethingHappened);
    ///
    /// @example publish an event
    ///     EventManager.Instance.Raise(new SomethingHappenedEvent());
    ///
    /// This class is a minor variation on <http://www.willrmiller.com/?p=87>
    /// </summary>
    public class EventManager
    {

        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventManager();
                }

                return instance;
            }
        }
        private static EventManager instance = null;

        public List<EventVisualizer.Base.EventCall> Events { get; private set; }
        public bool EnableDebugLog { get; set; }
        public bool EnableEventVisualization { get; set; }

        private EventManager()
        {
            Events = new List<EventVisualizer.Base.EventCall>();
            EnableEventVisualization = true;
            EnableDebugLog = false;
        }

        public delegate void EventDelegate<T>(T e) where T : Event;
        private delegate void EventDelegate(Event e);

        /// <summary>
        /// The actual delegate, there is one delegate per unique event. Each
        /// delegate has multiple invocation list items.
        /// </summary>
        private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();

        /// <summary>
        /// Lookups only, there is one delegate lookup per listener
        /// </summary>
        private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();

        /// <summary>
        /// Add the delegate.
        /// </summary>
        public void AddListener<T>(EventDelegate<T> del) where T : Event
        {
            if (delegateLookup.ContainsKey(del))
            {
                return;
            }

            if (EnableEventVisualization) LogAddListener(del);

            // Create a new non-generic delegate which calls our generic one.  This
            // is the delegate we actually invoke.
            EventDelegate internalDelegate = (e) => del((T)e);
            delegateLookup[del] = internalDelegate;

            EventDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel))
            {
                delegates[typeof(T)] = tempDel += internalDelegate;
            }
            else
            {
                delegates[typeof(T)] = internalDelegate;
            }
        }

        /// <summary>
        /// Remove the delegate. Can be called multiple times on same delegate.
        /// </summary>
        public void RemoveListener<T>(EventDelegate<T> del) where T : Event
        {
            if (EnableEventVisualization) LogRemoveListener(del);

            EventDelegate internalDelegate;
            if (delegateLookup.TryGetValue(del, out internalDelegate))
            {
                EventDelegate tempDel;
                if (delegates.TryGetValue(typeof(T), out tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                    {
                        delegates.Remove(typeof(T));
                    }
                    else
                    {
                        delegates[typeof(T)] = tempDel;
                    }
                }

                delegateLookup.Remove(del);
            }
        }

        /// <summary>
        /// The count of delegate lookups. The delegate lookups will increase by
        /// one for each unique AddListener. Useful for debugging and not much else.
        /// </summary>
        public int DelegateLookupCount { get { return delegateLookup.Count; } }

        /// <summary>
        /// Raise the event to all the listeners
        /// </summary>
        public void Raise(Event e)
        {
            if (EnableEventVisualization) LogRaise(e);

            EventDelegate del;
            if (delegates.TryGetValue(e.GetType(), out del))
            {
                del.Invoke(e);
            }
        }

        private string GetCaller()
        {
            var stacktrace = StackTraceUtility.ExtractStackTrace();
            if (stacktrace.Length != 0)
            {
                var stackEntries = stacktrace.Split('\n');
                foreach (var entry in stackEntries)
                {
                    if(entry.StartsWith(this.GetType().ToString()))
                    {
                        continue;
                    }

                    //First item that is not this class is our external caller
                    return Regex.Match(entry, "([^:]*):").Groups[1].Value;
                }
            }

            return "";
        }

        private void LogRaise(Event e)
        {
            string eventName = e.GetType().ToString();
            string sender = GetCaller();
            if(EnableDebugLog) Debug.Log(sender + " raised " + eventName);

            var newEvents = new List<EventVisualizer.Base.EventCall>();
            foreach (var eventCall in Events)
            {
                if(eventCall.EventName == eventName)
                {
                    newEvents.Add(new EventVisualizer.Base.EventCall(sender, eventCall.Receiver, eventCall.EventName, eventCall.Method));
                }
            }
            Events.AddRange(newEvents);
            Events.RemoveAll(item => ((item.Sender == null) &&
                                      (item.EventName == eventName)));
        }

        private void LogAddListener<T>(EventDelegate<T> del) where T : Event
        {
            string receiver = GetCaller();
            string eventName = typeof(T).ToString();
            string methodName = del.Method.Name;
            if (EnableDebugLog) Debug.Log(receiver + " added Listener for " + eventName + ": " + methodName);
            
            //add this as an EventCall, even though there is no sender so far (this is needed to show empty slots)
            EventVisualizer.Base.EventCall newCall = new EventVisualizer.Base.EventCall(null, receiver, eventName, methodName);
            Events.Add(newCall);
        }

        private void LogRemoveListener<T>(EventDelegate<T> del) where T : Event
        {
            string receiver = GetCaller();
            string eventName = typeof(T).ToString();
            string methodName = del.Method.Name;
            if (EnableDebugLog) Debug.Log(receiver + " removed Listener for " + eventName + ": " + methodName);

            Events.RemoveAll(item => (  (item.EventName.Equals(eventName)) &&
                                        (item.Receiver.Equals(receiver)) &&
                                        (item.Method.Equals(methodName))));
        }

    }
}
