namespace SDD.Events {

  /// <summary>
  ///  Interface for event handlers
  /// </summary>
  public interface IEventHandler {

    /// <summary>
    /// Subscribe to events
    ///
    /// @example
    ///   Events.AddListener<MoveResolvedEvent>(OnMoveResolved);
    ///     or
    ///   EventManager.OnSetRule += OnSetRule;
    /// </summary>
    void SubscribeEvents();

    /// <summary>
    /// Unsubscribe from events
    ///
    /// @example
    ///   Events.RemoveListener<MoveResolvedEvent>(OnMoveResolved);
    ///     or
    ///   EventManager.OnSetRule -= OnSetRule;
    /// </summary>
    void UnsubscribeEvents();

  }
}
