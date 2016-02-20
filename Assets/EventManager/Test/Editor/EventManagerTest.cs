using NUnit.Framework;

namespace SDD.Events.Test {

  public class TestOneEvent : SDD.Events.Event {
    public string Name { get; set; }
  }

  public class TestTwoEvent : SDD.Events.Event {
    public string Name { get; set; }
  }

  [TestFixture]
  [Category("EventManager")]
  public class EventManagerTest  {

    private int testOneEventCount = 0;
    private string testOneEventName = "";

    private int testTwoEventCount = 0;
    private string testTwoEventName = "";

    private void OnTestOne(TestOneEvent e) {
      testOneEventCount += 1;
      testOneEventName = e.Name;
    }

    private void OnTestTwo(TestTwoEvent e) {
      testTwoEventCount += 1;
      testTwoEventName = e.Name;
    }

    [SetUp]
    public void Before() {
      testOneEventCount = 0;
      testOneEventName = "";
      testTwoEventCount = 0;
      testTwoEventName = "";
      Assert.IsTrue(EventManager.Instance.DelegateLookupCount == 0);
    }

    [TearDown]
    public void After() {
      // ensure cleanup
      EventManager.Instance.RemoveListener<TestOneEvent>(OnTestOne);
      EventManager.Instance.RemoveListener<TestTwoEvent>(OnTestTwo);
      Assert.IsTrue(EventManager.Instance.DelegateLookupCount == 0);
    }

    [Test]
    public void InstancePropertyGetterCreatesEventManager() {
      Assert.IsTrue(EventManager.Instance != null);
      Assert.IsTrue(EventManager.Instance.GetType() == typeof(EventManager));
    }

    [Test]
    public void AddListenerIncrementsLookupCountOncePer() {
      EventManager.Instance.AddListener<TestOneEvent>(OnTestOne);
      Assert.IsTrue(EventManager.Instance.DelegateLookupCount == 1);

      EventManager.Instance.AddListener<TestOneEvent>(OnTestOne);
      Assert.IsTrue(EventManager.Instance.DelegateLookupCount == 1);
    }

    [Test]
    public void RemoveListenerDecrementsLookupCountAlways() {
      EventManager.Instance.AddListener<TestOneEvent>(OnTestOne);
      EventManager.Instance.AddListener<TestOneEvent>(OnTestOne);
      EventManager.Instance.AddListener<TestOneEvent>(OnTestOne);
      Assert.IsTrue(EventManager.Instance.DelegateLookupCount == 1);

      EventManager.Instance.RemoveListener<TestOneEvent>(OnTestOne);
      Assert.IsTrue(EventManager.Instance.DelegateLookupCount == 0);
    }

    [Test]
    public void RemoveListenerHandlesNoListeners() {
      Assert.IsTrue(testOneEventCount == 0);
      Assert.IsTrue(EventManager.Instance.DelegateLookupCount == 0);

      EventManager.Instance.RemoveListener<TestOneEvent>(OnTestOne);
      Assert.IsTrue(testOneEventCount == 0);
      Assert.IsTrue(EventManager.Instance.DelegateLookupCount == 0);
    }

    [Test]
    public void RaiseInvokes() {
      EventManager.Instance.AddListener<TestOneEvent>(OnTestOne);
      EventManager.Instance.Raise(new TestOneEvent() { Name="One A" });
      Assert.IsTrue(testOneEventCount == 1);
      Assert.IsTrue(testOneEventName == "One A");

      EventManager.Instance.Raise(new TestOneEvent() { Name="One B" });
      Assert.IsTrue(testOneEventCount == 2);
      Assert.IsTrue(testOneEventName == "One B");
    }

    [Test]
    public void RaiseInvokesCorrectDelegate() {
      EventManager.Instance.AddListener<TestOneEvent>(OnTestOne);
      EventManager.Instance.AddListener<TestTwoEvent>(OnTestTwo);
      EventManager.Instance.Raise(new TestTwoEvent() { Name="Two A" });
      Assert.IsTrue(testTwoEventCount == 1);
      Assert.IsTrue(testTwoEventName == "Two A");
      Assert.IsTrue(testOneEventCount == 0);
      Assert.IsTrue(testOneEventName == "");
    }

    [Test]
    public void RaiseHandlesNoListeners() {
      Assert.IsTrue(testOneEventCount == 0);
      Assert.IsTrue(EventManager.Instance.DelegateLookupCount == 0);

      EventManager.Instance.Raise(new TestOneEvent() { Name="One A" });
      Assert.IsTrue(testOneEventCount == 0);
      Assert.IsTrue(EventManager.Instance.DelegateLookupCount == 0);
    }
  }
}

