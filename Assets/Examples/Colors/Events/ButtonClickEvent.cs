namespace Colors.Events {

  /// <summary>
  /// Raised event signals a button was clicked
  /// </summary>
  public class ButtonClickEvent : SDD.Events.Event {

    public ButtonHandler ButtonHandler { get; set; }

    /// <summary>
    /// Return a string
    /// </summary>
    public override string ToString(){
      return string.Format("{0}, ButtonHandler {1}", base.ToString(), ButtonHandler);
    }

  }
}
