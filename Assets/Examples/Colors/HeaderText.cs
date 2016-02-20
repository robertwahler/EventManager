using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

using Colors.Events;

namespace Colors {

  /// <summary>
  /// Header text handler
  /// </summary>
  public class HeaderText : EventHandler {

    /// <summary>
    /// Header text. Assign in IDE.
    /// </summary>
    public Text text;

    public override void SubscribeEvents() {
      Debug.Log(string.Format("HeaderText.SubscribeEvents() name {0}", name));

      EventManager.Instance.AddListener<ButtonClickEvent>(OnButtonClickEvent);
      EventManager.Instance.AddListener<ButtonRemovedEvent>(OnButtonRemovedEvent);
    }

    public override void UnsubscribeEvents() {
      Debug.Log(string.Format("HeaderText.UnsubscribeEvents() name {0}", name));

      EventManager.Instance.RemoveListener<ButtonClickEvent>(OnButtonClickEvent);
      EventManager.Instance.RemoveListener<ButtonRemovedEvent>(OnButtonRemovedEvent);
    }

    /// <summary>
    /// A button in the scene was clicked
    /// </summary>
    public void OnButtonClickEvent(ButtonClickEvent e) {
      Debug.Log(string.Format("HeaderText.OnClick({0})", e));

      string caption = string.Format("{0} '{1}' was clicked.", e.ButtonHandler.kind, e.ButtonHandler.name);
      text.text = caption;
    }

    /// <summary>
    /// A button in the scene was destroyed
    /// </summary>
    public void OnButtonRemovedEvent(ButtonRemovedEvent e) {
      Debug.Log(string.Format("HeaderText.OnButtonRemoved({0})", e));

      string caption = string.Format("'{0}' was removed. EventManager.DelegateLookupCount is {1}.", e.Name, EventManager.Instance.DelegateLookupCount);
      text.text = caption;
    }

  }
}
