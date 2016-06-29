using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Colors.Test {

  /// <summary>
  /// Base class for unit tests
  /// </summary>
  public class UnitTest {

    /// <summary>
    /// This path is used to stub the DataPath (parent of Settings) folder
    /// </summary>
    public string TestPath { get { return GetTestPath(); }}

    /// <summary>
    /// This path holds test fixtures
    /// </summary>
    public string FixturesPath { get { return GetFixturesPath(); }}

    public UnitTest() {
      Debug.Log(string.Format("UnitTest()"));
      Initialize();
    }

    [SetUp]
    public void BeforeEach() {
      Debug.Log(string.Format("UnitTest.BeforeEach()"));

      ClearSettings();
    }

    protected virtual void ClearSettings() {
      Debug.Log(string.Format("UnitTest.ClearSettings()"));

      TestSetup.Instance.ClearSettings();
    }

    protected virtual void Initialize() {
    }

    /// <summary>
    /// Read fixture file and return text
    /// </summary>
    protected virtual string ReadFixture(string fixture) {
      var sourcePaths = new string[] {TestSetup.Instance.FixturesPath, fixture};
      string sourcePath = sourcePaths.Aggregate(System.IO.Path.Combine);
      string content = System.IO.File.ReadAllText(sourcePath);

      // normalize EOL CRLF to Unix style EOL
      return Regex.Replace(content, @"\r\n", "\n", RegexOptions.Multiline);
    }

    private string GetTestPath() {
      return TestSetup.Instance.TestPath;
    }

    private string GetFixturesPath() {
      return TestSetup.Instance.FixturesPath;
    }

  }
}
