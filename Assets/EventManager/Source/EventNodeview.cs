using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EventNodeview : EditorWindow
{
    Dictionary<string, Rect> rectangles = new Dictionary<string, Rect>();

    [MenuItem("Window/Event Nodeview")]
    static void ShowEditor()
    {}

    void OnGUI()
    {
        //Add all listeners and senders to our list of rectangles we wanna show
        //First put the senders (left side)
        int index = 0;
        int posYIndex = 0;
        foreach (var sender in SDD.Events.EventManager.Instance.senders.Keys)
        {
            if (!rectangles.ContainsKey(sender))
            {
                index++;
                posYIndex++;
                var posY = (100 * posYIndex) + 10;
                rectangles.Add(sender, new Rect(200, posY, 100, 100));
            }
        }
        //then put down the receivers (right side)
        posYIndex = 0;
        foreach (var listener in SDD.Events.EventManager.Instance.listeners.Keys)
        {
            if (!rectangles.ContainsKey(listener))
            {
                index++;
                posYIndex++;
                var posY = (100 * posYIndex) + 10;
                rectangles.Add(listener, new Rect(400, posY, 100, 100));
            }
        }

        //Connect each Sender and Listener that have the same Event
        foreach (var listener in SDD.Events.EventManager.Instance.listeners)
        {
            foreach (var sender in SDD.Events.EventManager.Instance.senders)
            {
                if(listener.Value.Equals(sender.Value))
                {
                    DrawArrowConnection(rectangles[sender.Key], rectangles[listener.Key]);
                }
            }
        }

        //This updates the rectangles and their connections. This is needed to allow drag+drop
        BeginWindows();
        index = 0;
        List<string> rectKeys = new List<string>(rectangles.Keys);
        foreach (var rectKey in rectKeys)
        {
            rectangles[rectKey] = GUI.Window(index++, rectangles[rectKey], DrawNodeWindow, rectKey);
        }
        EndWindows();
    }

    void DrawNodeWindow(int id)
    {
        GUI.DragWindow();
    }

    void DrawArrowConnection(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
        for (int i = 0; i < 3; i++) // Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);

        //add small arrow to the end
        Handles.color = Color.black;
        Handles.DrawLine(endPos, new Vector3(endPos.x - 10, endPos.y + 5));
        Handles.DrawLine(endPos, new Vector3(endPos.x - 10, endPos.y - 5));
    }
}