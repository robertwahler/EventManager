using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

using Colors.Events;

namespace Colors {

  /// <summary>
  /// Button handler
  /// </summary>
  public class ButtonHandler : EventHandler {

    /// <summary>
    /// Kind of button. Assign in IDE.
    /// </summary>
    public ButtonKind kind = ButtonKind.Red;

    /// <summary>
    /// Button text. Assign in IDE.
    /// </summary>
    public Text text;

    /// <summary>
    /// Kind of button. Assign in IDE.
    /// </summary>
    public bool On { get { return GetOn(); } set { SetOn(value); }}
    protected bool on = false;

    /// <summary>
    /// Event reference cache.
    /// </summary>
    /// <remarks>
    /// Demo of reference caching. Caching an event reference is normally
    /// needed only when no allocations are wanted after initialization to
    /// minimize GC. Since this reference is only used once per object, it
    /// really isn't that useful.
    /// </remarks>
    protected ButtonRemoveEvent buttonRemoveEvent;

    protected override void OnEnable() {
      Debug.Log(string.Format("ButtonHandler.OnEnable() name {0}", name));
      base.OnEnable();

      // Fresh reference on each enable
      buttonRemoveEvent = new ButtonRemoveEvent();
      On = false;
    }

    public override void SubscribeEvents() {
      Debug.Log(string.Format("ButtonHandler.SubscribeEvents() name {0}", name));

      EventManager.Instance.AddListener<ButtonClickEvent>(OnButtonClick);
      EventManager.Instance.AddListener<ButtonRemoveEvent>(OnButtonRemove);
    }

    public override void UnsubscribeEvents() {
      Debug.Log(string.Format("ButtonHandler.UnsubscribeEvents() name {0}", name));

      EventManager.Instance.RemoveListener<ButtonClickEvent>(OnButtonClick);
      EventManager.Instance.RemoveListener<ButtonRemoveEvent>(OnButtonRemove);
    }

    /// <summary>
    /// Any button in the scene was clicked
    /// </summary>
    public void OnButtonClick(ButtonClickEvent e) {
      Debug.Log(string.Format("ButtonHandler.OnButtonClick({0}) name {1}", e, name));

      // all senders welcome if not handled
      if (!e.Handled) {
        if (e.ButtonHandler.kind == kind) {
          On = !On;
        }
      }
    }

    /// <summary>
    /// Any button in the scene was clicked
    /// </summary>
    public void OnButtonRemove(ButtonRemoveEvent e) {
      Debug.Log(string.Format("ButtonHandler.OnButtonRemove({0}) name {1}", e, name));

      // only this sender is handled here
      if ((!e.Handled) && (e.ButtonHandler == this)) {
        GameObject.Destroy(gameObject);
        // raise again as notice for logging, etc
        EventManager.Instance.Raise(new ButtonRemoveEvent(){ ButtonHandler=this, Handled=true, Kind=e.Kind, Name=e.Name });
      }
    }

    /// <summary>
    /// OnClick handler for the Button on this specific GameObject
    /// </summary>
    public void OnClick() {
      Debug.Log(string.Format("ButtonHandler.OnClick() name {0}", name));

      EventManager.Instance.Raise(new ButtonClickEvent(){ ButtonHandler=this });
    }

    /// <summary>
    /// Remove click handler for the button on this specific GameObject
    /// </summary>
    public void RemoveClick() {
      Debug.Log(string.Format("ButtonHandler.OnRemove() name {0} removing, EventManager.DelegateLookupCount {1}", name, EventManager.Instance.DelegateLookupCount));

      // set and raise, the actual event is handled in the listener
      buttonRemoveEvent.ButtonHandler= this;
      buttonRemoveEvent.Name = name;
      buttonRemoveEvent.Kind = kind;
      buttonRemoveEvent.Handled = false;
      EventManager.Instance.Raise(buttonRemoveEvent);
    }

    protected virtual bool GetOn() {
      return on;
    }

    protected virtual void SetOn(bool value) {
      on = value;
      string label = on ? "on" : "off";
      if (text != null) {
        text.text = label;
      }
    }

  }
}
