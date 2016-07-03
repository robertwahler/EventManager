namespace Colors.Events {

  /// <summary>
  /// Raised event signals a button is removed
  /// </summary>
  public class ButtonRemoveEvent : BaseEvent {

    /// <summary>
    /// Sender handler
    /// </summary>
    public ButtonHandler ButtonHandler { get; set; }

    public string Name { get; set; }

    public ButtonKind Kind { get; set; }

    /// <summary>
    /// Return a string
    /// </summary>
    public override string ToString(){
      return string.Format("{0}, Name {1}, Kind {2}", base.ToString(), Name, Kind);
    }

  }
}
