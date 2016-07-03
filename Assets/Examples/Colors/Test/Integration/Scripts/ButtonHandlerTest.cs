using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

using SDD.Events;
using SDD.Assertions;
using Colors;
using Colors.Events;

namespace Examples.Integration.Test.Colors {

  // DynamicTest attribute must match supporting test scene name
  [IntegrationTest.DynamicTest("ColorsTest")]
  [IntegrationTest.Timeout(10)]
  //[IntegrationTest.ExpectExceptions(false, typeof (System.ArgumentException))]
  //[IntegrationTest.SucceedWithAssertions]
  //[IntegrationTest.Ignore]
  public class ButtonHandlerTest : IntegrationTestBase {

    /// <summary>
    /// Number of times the ButtonClickEvent fired with Handled == true
    /// </summary>
    protected int buttonClickEventHandledCount = 0;

    public override void SubscribeEvents() {
      Debug.Log(string.Format("ButtonHandlerTest.SubscribeEvents() name {0}", name));

      EventManager.Instance.AddListener<ButtonClickEvent>(OnButtonClick);
    }

    public override void UnsubscribeEvents() {
      Debug.Log(string.Format("ButtonHandlerTest.UnsubscribeEvents() name {0}", name));

      EventManager.Instance.RemoveListener<ButtonClickEvent>(OnButtonClick);
    }

    /// <summary>
    /// Any button in the scene was clicked
    /// </summary>
    protected void OnButtonClick(ButtonClickEvent e) {
      if (e.Handled) {
        Debug.Log(string.Format("ButtonHandlerTest.OnButtonClick({0}) name {1}", e, name));
        buttonClickEventHandledCount += 1;
      }
    }

    protected override IEnumerator Run() {
      Debug.Log("ButtonHandlerTest.Run()");

      Assert.True(buttonClickEventHandledCount == 0);

      // click all four red buttons by raising the click event
      for (int i = 0; i < 4; i++) {
        EventManager.Instance.Raise(new ButtonClickEvent(){ Kind=ButtonKind.Red });

        // four of each kind of button
        Assert.True(buttonClickEventHandledCount == (i + 1) * 4);

        // debug pause
        yield return new WaitForSeconds(DebugDelay);
      }

      // four of each kind of button
      Assert.True(buttonClickEventHandledCount == 4 * 4);

      // if we made it here then all is well because assertions will raise exceptions
      IntegrationTest.Pass();
    }

  }
}
