using UnityEngine;
//using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

using SDD.Events;
using Colors.Events;

namespace Examples.Integration.Test {

  /// <summary>
  /// All integration tests are based on IntegrationTestBase
  /// </summary>
  public class IntegrationTestBase : EventHandler {

    /// <summary>
    /// Convenience getter for test delay
    /// </summary>
    public float DebugDelay { get { return IntegrationTestManager.Instance.debugDelay; }}

    protected override void OnDisable() {
      Debug.Log("IntegrationTestBase.OnDisable()");
      base.OnDisable();

      After();
      if (IntegrationTestManager.Instance != null) {
        IntegrationTestManager.Instance.AfterEach();
      }
    }

    /// <summary>
    /// Before runs before EACH test
    /// </summary>
    protected virtual void Before() {
      //Debug.Log("IntegrationTestBase.Before()");

      // override me
    }

    /// <summary>
    /// After runs after EACH test
    /// </summary>
    protected virtual void After() {
      //Debug.Log("IntegrationTestBase.After()");

      // override me
    }

    public override void SubscribeEvents() {
      // override me
    }

    public override void UnsubscribeEvents(){
      // override me
    }

    protected virtual IEnumerator Start() {
      // wait one frame for the GameManager
      yield return null;

      //
      // Init singletons here
      //

      IntegrationTestManager.Instance.BeforeEach();
      Before();

      // wait one frame for the Before() in case MonoBehaviours need to spin up
      yield return null;

      // NOTE: If Run needs to wait more than one frame to start, it should manage that on its own
      yield return StartCoroutine(Run());
    }

    /// <summary>
    /// Run contains the core logic of each test
    /// </summary>
    protected virtual IEnumerator Run() {
      // override me
      yield break;
    }

    /// <summary>
    /// Wait forever
    /// </summary>
    public static IEnumerator WaitForever() {
      while (true) {
        yield return null;
      }
    }
  }
}
