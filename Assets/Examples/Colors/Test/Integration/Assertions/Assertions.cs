using UnityEngine;
using UnityEngine.Assertions;

namespace SDD.Assertions {

  /// <summary>
  /// Define our own assertions because the NUnit framework is in the Editor namespace
  /// </summary>
  public static class Assert  {

    public static void Equals(int a, int b) {
      throw new System.InvalidOperationException("Assertion not supported. Use Assert.True() or Assert.False() instead");
    }

    public static void True(bool condition) {
      UnityEngine.Assertions.Assert.IsTrue(condition);
    }

    public static void True(bool condition, string message) {
      UnityEngine.Assertions.Assert.IsTrue(condition, message);
    }

    public static void False(bool condition) {
      UnityEngine.Assertions.Assert.IsFalse(condition);
    }

    public static void False(bool condition, string message) {
      UnityEngine.Assertions.Assert.IsFalse(condition, message);
    }
  }
}
