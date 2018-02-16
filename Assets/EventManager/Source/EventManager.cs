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

            LogListener(del);

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
            LogRaise(e);

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

        public Dictionary<string, string> listeners = new Dictionary<string, string>();
        public Dictionary<string, string> senders = new Dictionary<string, string>();

        private void LogRaise(Event e)
        {
            senders[GetCaller()] = e.GetType().ToString();
            Debug.Log(GetCaller() + " raised " + e.GetType());
        }

        private void LogListener<T>(EventDelegate<T> del) where T : Event
        {
            listeners[GetCaller()] = typeof(T).ToString();
            Debug.Log(GetCaller() + " added Listener for " + typeof(T) + ": " + del.Method.Name);
        }

    }
}
