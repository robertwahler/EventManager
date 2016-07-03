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

      EventManager.Instance.AddListener<ButtonClickEvent>(OnButtonClickEvent);
    }

    public override void UnsubscribeEvents() {
      Debug.Log(string.Format("ButtonHandler.UnsubscribeEvents() name {0}", name));

      EventManager.Instance.RemoveListener<ButtonClickEvent>(OnButtonClickEvent);
    }

    /// <summary>
    /// Any button in the scene was clicked
    /// </summary>
    public void OnButtonClickEvent(ButtonClickEvent e) {
      Debug.Log(string.Format("ButtonHandler.OnClick({0}) name {1}", e, name));

      if (!e.Handled) {
        if (e.ButtonHandler.kind == kind) {
          On = !On;
        }
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
    /// OnRemove handler for the button on this specific GameObject
    /// </summary>
    public void OnRemove() {
      Debug.Log(string.Format("ButtonHandler.OnRemove() name {0} removing, EventManager.DelegateLookupCount {1}", name, EventManager.Instance.DelegateLookupCount));

      buttonRemoveEvent.Name = name;
      buttonRemoveEvent.Kind = kind;
      buttonRemoveEvent.Handled = true;
      EventManager.Instance.Raise(buttonRemoveEvent);

      GameObject.DestroyImmediate(gameObject);
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
