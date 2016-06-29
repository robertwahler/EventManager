using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Examples.Integration.Test.Colors {

  // DynamicTest attribute must match supporting test scene name
  [IntegrationTest.DynamicTest("ColorsTest")]
  [IntegrationTest.Timeout(10)]
  //[IntegrationTest.ExpectExceptions(false, typeof (System.ArgumentException))]
  //[IntegrationTest.SucceedWithAssertions]
  //[IntegrationTest.Ignore]
  public class ButtonHandlerTest : IntegrationTestBase {

    protected override IEnumerator Run() {
      Debug.Log("ButtonHandlerTest.Run()");

      // debug
      yield return new WaitForSeconds(DebugDelay);

      // TODO: add a real test
      IntegrationTest.Pass();
    }

  }
}
