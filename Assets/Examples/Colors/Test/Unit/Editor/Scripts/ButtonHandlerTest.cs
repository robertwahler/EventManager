using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Colors.Test {

  [TestFixture]
  [Category("UI")]
  public class ButtonHandlerTest : UnitTest {

    private GameObject TestContainer { get; set; }
    private ButtonHandler buttonHandler;

    [SetUp]
    public void Before() {
      TestContainer = new GameObject();
      buttonHandler = TestContainer.AddComponent<ButtonHandler>();
      buttonHandler.name = "buttonHandler";
      // Testing MonoBehaviours requires taking manual control. Call a
      // protected method for setup.
      buttonHandler.Call("OnEnable");
    }

    [TearDown]
    public void After() {
      // Testing MonoBehaviours requires taking manual control. Call a
      // protected method for cleanup.
      buttonHandler.Call("OnDisable");
      if (TestContainer != null) {
        GameObject.DestroyImmediate(TestContainer);
      }
      TestContainer = null;
      buttonHandler = null;
    }

    [Test]
    public void DefaultsToOff() {
      Assert.IsTrue(buttonHandler.On == false);
    }

    [Test]
    public void OnClickTogglesOn() {
      Assert.IsTrue(buttonHandler.On == false);
      buttonHandler.OnClick();
      Assert.IsTrue(buttonHandler.On == true);
      buttonHandler.OnClick();
      Assert.IsTrue(buttonHandler.On == false);
    }
  }
}

