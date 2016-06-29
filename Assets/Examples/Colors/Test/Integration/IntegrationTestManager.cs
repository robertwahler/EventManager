using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

using SDD.Events;

namespace Examples.Integration.Test {

  /// <summary>
  /// Manages the Setup and Teardown before and after ALL integration tests.
  /// Add this to the scene after GameManager.
  /// </summary>
  public class IntegrationTestManager : MonoBehaviour {

    /// <summary>
    /// Set true to watch the full animations in the scene. Use this flag to
    /// disable animations in the test runner to speed up tests.
    /// </summary>
    public bool enableAnimations = false;

    /// <summary>
    /// Set this above zero to slow down tests for debugging
    /// </summary>
    public float debugDelay = 0.0f;

    /// <summary>
    /// This path is used to stub the Settings folder. Used for all tests.
    /// </summary>
    public string TestPath { get; set; }

    /// <summary>
    /// This path holds test fixtures
    /// </summary>
    public string FixturesPath { get; set; }

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    public static IntegrationTestManager Instance {
      get {
        if (instance == null) {
          Debug.Log(string.Format("IntegrationTestManager.Instance.get looking for object"));
          instance = (IntegrationTestManager) GameObject.FindObjectOfType(typeof(IntegrationTestManager));
          if (instance == null) {
            Debug.LogWarning(string.Format("IntegrationTestManager.Instance.get object not found, is it attached to the scene?"));
          }
        }

        return instance;
      }
    }
    private static IntegrationTestManager instance;

    /// <summary>
    /// Tracks when GameManager and its systems have been initialized
    /// </summary>
    private bool initialized = false;

    protected virtual void OnDisable() {
      Debug.Log("IntegrationTestManager.OnDisable()");

      AfterAll();
    }

    protected virtual void OnEnable() {
      Debug.Log("IntegrationTestManager.OnEnable()");

      BeforeAll();
    }

    /// <summary>
    /// This is run before all the tests in this namespace (SDD.Test).
    /// </summary>
    /// <remarks>
    /// Needs to run BEFORE the GameManager so that the stubs are ready
    /// </remarks>
    public void BeforeAll() {
      Debug.Log(string.Format("IntegrationTestManager.BeforeAll()"));

      var paths = new string[] {System.IO.Directory.GetCurrentDirectory(), "tmp", "test"};
      TestPath = paths.Aggregate(System.IO.Path.Combine);

      StubSettingsPath();
    }

    /// <summary>
    /// This is run after all the tests in this namespace (SDD.Test)
    /// </summary>
    public void AfterAll() {
      Debug.Log(string.Format("IntegrationTestManager.AfterAll()"));

      Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// This is run by IntegrationTestBase before each of the tests in this namespace (SDD.Test).
    /// </summary>
    public void BeforeEach() {
      Debug.Log(string.Format("IntegrationTestManager.BeforeEach()"));

      // clear out all JSON before each test
      ClearSettings();
    }

    /// <summary>
    /// This is run by IntegrationTestBase after each test in this namespace (SDD.Test)
    /// </summary>
    public void AfterEach() {
      Debug.Log(string.Format("IntegrationTestManager.AfterEach()"));

    }
    /// <summary>
    /// GameManager is ready. Initialize it its systems
    /// </summary>
    protected virtual void Initialize() {
      if (!initialized) {
        Debug.Log(string.Format("IntegrationTestManager.Initialize()"));

        //
        // This is the place to init singletons, disable logging, disable audio, add mocks, etc
        //

        initialized = true;
      }
    }

    /// <summary>
    /// Clear the settings for a fresh start
    /// </summary>
    public void ClearSettings() {
      Debug.Log(string.Format("IntegrationTestManager.ClearSettingsFolder()"));

      if (System.IO.Directory.Exists(TestPath)) {
        // clear out test folder
        System.IO.Directory.Delete(path: TestPath, recursive: true);
      }

      if (!System.IO.Directory.Exists(TestPath)) {
        System.IO.Directory.CreateDirectory(TestPath);
      }

      //Settings.Clear();
    }

    /// <summary>
    /// Stub out production settings locations so that we use an empty tmp
    /// folder for each test
    /// </summary>
    /// <remarks>
    /// Needs to run BEFORE the GameManager so that the stubs are ready
    /// </remarks>
    private void StubSettingsPath() {
      Debug.Log(string.Format("IntegrationTestManager.StubSettingsPath()"));

      // stub out production settings folder
      //SDD.Settings.DataPath = TestPath;
      ClearSettings();
    }

  }
}
