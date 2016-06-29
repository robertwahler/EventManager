using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Examples.Test {

  [SetUpFixture]
  public class TestSetup {

    private static TestSetup instance = null;

    /// <summary>
    /// This path is used to stub the Settings folder. Used for all tests.
    /// </summary>
    public string TestPath { get; set; }

    /// <summary>
    /// This path holds test fixtures
    /// </summary>
    public string FixturesPath { get; set; }

    public TestSetup() {
      instance = this;
      Initialize();
    }

    protected virtual void Initialize() {
      Debug.Log(string.Format("TestSetup.Initialize()"));

      var paths = new string[] {System.IO.Directory.GetCurrentDirectory(), "tmp", "test"};
      TestPath = paths.Aggregate(System.IO.Path.Combine);

      paths = new string[] {System.IO.Directory.GetCurrentDirectory(), "Assets", "Test", "Unit", "Editor", "Fixtures"};
      FixturesPath = paths.Aggregate(System.IO.Path.Combine);
    }

    /// <summary>
    /// This is run before all the tests in this namespace (SDD.Test)
    /// </summary>
    [SetUp]
    public void BeforeAll() {
      Debug.Log(string.Format("TestSetup.BeforeAll()"));

      StubSettingsPath();

      //
      // Start singletons here
      //
    }

    /// <summary>
    /// This is run after all the tests in this namespace (SDD.Test)
    /// </summary>
    [TearDown]
    public void AfterAll() {
      Debug.Log(string.Format("TestSetup.AfterAll()"));

      //
      // Destroy singletons here
      //

      // allow to garbage collect
      instance = null;

      Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    public static TestSetup Instance {
      get {
        if (instance == null) {
          instance = new TestSetup();
        }
        return instance;
      }
    }


    /// <summary>
    /// Clear the settings for a fresh start
    /// </summary>
    public void ClearSettings() {
      Debug.Log(string.Format("TestSetup.ClearSettingsFolder()"));

      if (System.IO.Directory.Exists(TestPath)) {
        // clear out test folder
        System.IO.Directory.Delete(path: TestPath, recursive: true);
      }

      if (!System.IO.Directory.Exists(TestPath)) {
        System.IO.Directory.CreateDirectory(TestPath);
      }

      // Clear custom serialization files
      //SDD.Settings.Clear();
    }

    private void StubSettingsPath() {
      Debug.Log(string.Format("TestSetup.StubSettingsPath()"));

      // stub out production serialization folder
      //SDD.Settings.DataPath = TestPath;
      ClearSettings();
    }

  }
}
