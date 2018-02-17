
namespace EventVisualizer.Base
{
    [System.Serializable]
    public class EventCall
    {
        public string Sender { get; set; }
        public string Receiver { get; private set; }
        public string EventName { get; private set; }
        public string Method { get; private set; }
        public string ReceiverComponentName { get; private set; }

        public string MethodFullPath
        {
            get
            {
                return ReceiverComponentName + Method;
            }
        }

        public EventCall(string sender, string receiver, string eventName, string methodName)
        {
            Sender = sender;
            Receiver = receiver;
            EventName = eventName;
            Method = methodName;
        }
    }
}