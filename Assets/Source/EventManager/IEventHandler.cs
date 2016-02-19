namespace SDD.Events {

  /// <summary>
  ///  Interface for event handlers
  /// </summary>
  public interface IEventHandler {

    /// <summary>
    /// @example usage for body of method
    ///     EventManager.OnSetRule += OnSetRule;
    /// </summary>
    void SubscribeEvents();

    /// <summary>
    /// @example usage for body of method
    ///    EventManager.OnSetRule -= OnSetRule;
    /// </summary>
    void UnsubscribeEvents();

  }
}
