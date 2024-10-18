using System.ComponentModel;
using Unity;
using UnityContainers;
using Container = UnityContainers.Container;

namespace UnityContainersTests;

public enum ContainerConfigurationType{
    Imperative,
    Declarative,
}

[TestClass]
public class UnitTest
{

    private IUnityContainer ConfigureContainer(ContainerConfigurationType configurationType)
    {
        switch (configurationType)
        {
            case ContainerConfigurationType.Imperative:
                return Container.Configure();
            case ContainerConfigurationType.Declarative:
                return Container.FromFile();
        }
        throw new InvalidEnumArgumentException();
    }
    
    [TestMethod]
    [DataRow(ContainerConfigurationType.Imperative)]
    [DataRow(ContainerConfigurationType.Declarative)]
    //[DataRow(false)]
    public void WorkerShouldReturnExpectedString(ContainerConfigurationType configurationType)
    {
        const string expected = "ab";
        IUnityContainer cont = ConfigureContainer(configurationType);
        
        using var sw = new StringWriter();
        Console.SetOut(sw);
        var w1 = cont.Resolve<Worker>();
        w1.Work("a", "b");

        var result = sw.ToString().Trim();
        Assert.AreEqual(expected, result);
        
    }
    
    [TestMethod]
    [DataRow(ContainerConfigurationType.Imperative)]
    [DataRow(ContainerConfigurationType.Declarative)]
    public void Worker2ShouldReturnExpectedString(ContainerConfigurationType configurationType)
    {
        const string expected = "5";
        IUnityContainer cont = ConfigureContainer(configurationType);

        using var sw = new StringWriter();
        Console.SetOut(sw);
        var w2 = cont.Resolve<Worker2>();
        w2.Work("1", "4");

        var result = sw.ToString().Trim();
        Assert.AreEqual(expected, result);
    }
    
    [TestMethod]
    [DataRow(ContainerConfigurationType.Imperative)]
    [DataRow(ContainerConfigurationType.Declarative)]
    public void Worker3ShouldReturnExpectedString(ContainerConfigurationType configurationType)
    {
        const string expected = "ab1";
        IUnityContainer cont = ConfigureContainer(configurationType);

        using var sw = new StringWriter();
        Console.SetOut(sw);
        var w3 = cont.Resolve<Worker3>("state");
        w3.Work("a", "b");

        var result = sw.ToString().Trim();
        Assert.AreEqual(expected, result);
    }
    
    [TestMethod]
    [DataRow(ContainerConfigurationType.Imperative)]
    [DataRow(ContainerConfigurationType.Declarative)]
    //[DataRow(false)]
    public void StatefulWorkerShouldReturnExpectedString(ContainerConfigurationType configurationType)
    {
        const string expected = "ab1";
        IUnityContainer cont = ConfigureContainer(configurationType);

        using var sw = new StringWriter();
        Console.SetOut(sw);
        var w1 = cont.Resolve<Worker>("state");
        w1.Work("a", "b");

        var result = sw.ToString().Trim();
        Assert.AreEqual(expected, result);
    }
    
    [TestMethod]
    [DataRow(ContainerConfigurationType.Imperative)]
    [DataRow(ContainerConfigurationType.Declarative)]
    public void StatefulWorker2ShouldReturnExpectedString(ContainerConfigurationType configurationType)
    {
        const string expected = "121";
        IUnityContainer cont = ConfigureContainer(configurationType);

        using var sw = new StringWriter();
        Console.SetOut(sw);
        var w2 = cont.Resolve<Worker2>("state");
        w2.Work("1", "2");

        var result = sw.ToString().Trim();
        Assert.AreEqual(expected, result);
    }
    
    [TestMethod]
    [DataRow(ContainerConfigurationType.Imperative)]
    [DataRow(ContainerConfigurationType.Declarative)]
    public void StatefulWorker3ShouldReturnExpectedString(ContainerConfigurationType configurationType)
    {
        const string expected = "ab1";
        IUnityContainer cont = ConfigureContainer(configurationType);

        using var sw = new StringWriter();
        Console.SetOut(sw);
        var w3 = cont.Resolve<Worker3>("state");
        w3.Work("a", "b");

        var result = sw.ToString().Trim();
        Assert.AreEqual(expected, result);
    }
    
    
    [TestMethod]
    [DataRow(ContainerConfigurationType.Imperative)]
    [DataRow(ContainerConfigurationType.Declarative)]
    public void StateCalcShouldReturnTheSameObject(ContainerConfigurationType configurationType)
    {
        const string expected = "ab1\n122\nab3";
        IUnityContainer cont = ConfigureContainer(configurationType);
        
        using var sw = new StringWriter();
        Console.SetOut(sw);
            
        var w1 = cont.Resolve<Worker >("state");
        w1.Work("a", "b");
        var w2 = cont.Resolve<Worker3>("state");
        w2.Work("1", "2");
        var w3 = cont.Resolve<Worker3>("state");
        w3.Work("a", "b");
            
        var result = sw.ToString().Trim();
        Assert.AreEqual(expected, result);
    }
}