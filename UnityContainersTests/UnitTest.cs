using Unity;
using UnityContainers;
namespace UnityContainersTests;

[TestClass]
public class UnitTest
{
    private IUnityContainer _cont;
    [TestInitialize()]
    public void ConfigureUnityContainer()
    {
        _cont = Container.Configure();
    }
    
    [TestMethod]
    [DataRow(true)]
    //[DataRow(false)]
    public void WorkerShouldReturnExpectedString(bool isTrue)
    {
        const string expected = "ab";
        if (isTrue)
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);
            var w1 = _cont.Resolve<Worker>();
            w1.Work("a", "b");

            var result = sw.ToString().Trim();
            Assert.AreEqual(expected, result);
        }
        else
        {
            //...
        }
    }
    
    [TestMethod]
    [DataRow(true)]
    public void Worker2ShouldReturnExpectedString(bool isTrue)
    {
        const string expected = "5";
        if (isTrue)
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);
            var w2 = _cont.Resolve<Worker2>();
            w2.Work("1", "4");

            var result = sw.ToString().Trim();
            Assert.AreEqual(expected, result);
        }
        else
        {
            //...
        }
    }
    
    [TestMethod]
    [DataRow(true)]
    public void Worker3ShouldReturnExpectedString(bool isTrue)
    {
        const string expected = "ab1";
        if (isTrue)
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);
            var w3 = _cont.Resolve<Worker3>("state");
            w3.Work("a", "b");

            var result = sw.ToString().Trim();
            Assert.AreEqual(expected, result);
        }
        else
        {
            //...
        }
    }
    
    [TestMethod]
    [DataRow(true)]
    //[DataRow(false)]
    public void StatefulWorkerShouldReturnExpectedString(bool isTrue)
    {
        const string expected = "ab1";
        if (isTrue)
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);
            var w1 = _cont.Resolve<Worker>("state");
            w1.Work("a", "b");

            var result = sw.ToString().Trim();
            Assert.AreEqual(expected, result);
        }
        else
        {
            //...
        }
    }
    
    [TestMethod]
    [DataRow(true)]
    public void StatefulWorker2ShouldReturnExpectedString(bool isTrue)
    {
        const string expected = "121";
        if (isTrue)
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);
            var w2 = _cont.Resolve<Worker2>("state");
            w2.Work("1", "2");

            var result = sw.ToString().Trim();
            Assert.AreEqual(expected, result);
        }
        else
        {
            //...
        }
    }
    
    [TestMethod]
    [DataRow(true)]
    public void StatefulWorker3ShouldReturnExpectedString(bool isTrue)
    {
        const string expected = "ab1";
        if (isTrue)
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);
            var w3 = _cont.Resolve<Worker3>("state");
            w3.Work("a", "b");

            var result = sw.ToString().Trim();
            Assert.AreEqual(expected, result);
        }
        else
        {
            //...
        }
    }
    
    
    [TestMethod]
    [DataRow(true)]
    public void StateCalcShouldReturnTheSameObject(bool isTrue)
    {
        const string expected = "ab1\n122\nab3";
        if (isTrue)
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);
            
            var w1 = _cont.Resolve<Worker >("state");
            w1.Work("a", "b");
            var w2 = _cont.Resolve<Worker3>("state");
            w2.Work("1", "2");
            var w3 = _cont.Resolve<Worker3>("state");
            w3.Work("a", "b");
            
            var result = sw.ToString().Trim();
            Assert.AreEqual(expected, result);
        }
        else
        {
            //...
        }
    }
}