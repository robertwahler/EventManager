using SDD.Events;

namespace Colors.Events {

  /// <summary>
  /// Base event
  /// </summary>
  public class BaseEvent : Event {

    /// <summary>
    /// Events with Handled==false are requests for action.  If Handled==true
    /// then the event is an annoucement that an action occurred.
    /// </summary>
    public bool Handled { get; set; }

    /// <summary>
    /// Return a string
    /// </summary>
    public override string ToString(){
      return string.Format("{0}, Handled {1}", base.ToString(), Handled);
    }

  }
}
