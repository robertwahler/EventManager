namespace Colors.Test {
  static class TestExtensions {

    public static object Call(this object obj, string methodName, params object[] args) {
      var method = obj.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
      if (method != null) {
        return method.Invoke(obj, args);
      }
      return null;
    }

  }
}
