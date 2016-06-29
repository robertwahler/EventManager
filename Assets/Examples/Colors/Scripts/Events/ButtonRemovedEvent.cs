namespace Colors.Events {

  /// <summary>
  /// Raised event signals a button is removed
  /// </summary>
  public class ButtonRemovedEvent : BaseEvent {

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
