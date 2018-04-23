using System.Collections.Generic;
using UnityEngine;

namespace EventVisualizer.Base
{
    [System.Serializable]
    public class NodeData
    {
        public string Entity { get; private set; }

        public string Name
        {
            get
            {
                return Entity != null ? Entity : "<Missing>";
            }
        }
        
        public List<EventCall> Outputs { get; private set; }
        public List<EventCall> Inputs { get; private set; }

        [SerializeField]
        private static Dictionary<string, NodeData> nodes = new Dictionary<string, NodeData>();

        public static ICollection<NodeData> Nodes
        {
            get
            {
                return nodes != null ? nodes.Values : null;
            }
        }
        


        public static void Clear() 
        {
            nodes.Clear();
        }
        
        public static void RegisterEvent(EventCall eventCall)
        {
            CreateNode(eventCall.Receiver);
            nodes[eventCall.Receiver].Inputs.Add(eventCall);

            if (eventCall.Sender != null)
            {
                CreateNode(eventCall.Sender);
                nodes[eventCall.Sender].Outputs.Add(eventCall);
            }
        }

        private static void CreateNode(string entity)
        {
            if(entity == null)
            {
                return;
            }

            if(!nodes.ContainsKey(entity))
            {
                nodes.Add(entity, new NodeData(entity));
            }

        }
        
        public NodeData(string entity)
        {
            Entity = entity;
            Outputs = new List<EventCall>();
            Inputs = new List<EventCall>();
        }
    }

}