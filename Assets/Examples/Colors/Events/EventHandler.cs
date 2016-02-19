using UnityEngine;

using SDD.Events;

namespace Colors.Events {

  /// <summary>
  /// Event handler
  /// </summary>
  public abstract class EventHandler : MonoBehaviour, IEventHandler {

    /// <summary>
    /// @example
    ///   EventManager.Instance.AddListener<MoveResolvedEvent>(OnMoveResolved);
    /// </summary>
    public abstract void SubscribeEvents();

    /// <summary>
    /// @example
    ///   EventManager.Instance.RemoveListener<MoveResolvedEvent>(OnMoveResolved);
    /// </summary>
    public abstract void UnsubscribeEvents();

    protected virtual void OnEnable() {
      SubscribeEvents();
    }

    protected virtual void OnDisable() {
      UnsubscribeEvents();
    }

  }
}
