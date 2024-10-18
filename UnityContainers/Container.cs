using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace UnityContainers;

public class Container
{
    static public IUnityContainer Configure()
    {
        IUnityContainer cont = new UnityContainer();
        cont.RegisterType<ICalculator, CatCalc>("catcalc");
        cont.RegisterType<ICalculator, PlusCalc>("pluscalc");
        // StateCalc jako singelton
        cont.RegisterType<ICalculator, StateCalc>(
            "statecalc",
            new ContainerControlledLifetimeManager(),
            new InjectionConstructor(1)
            );

        
        // obiekty Worker korzystały z CatCalc
        cont.RegisterType<Worker>(
            new InjectionConstructor(
                new ResolvedParameter<ICalculator>("catcalc"))
            );
        // obiekty Worker2 korzystały z PlusCalc
        cont.RegisterType<Worker2>(
            new InjectionProperty("m_calc",
                new ResolvedParameter<ICalculator>("pluscalc"))
        );
        // obiekty Worker3 korzystały z CatCalc
        cont.RegisterType<Worker3>(
            new InjectionMethod("SetCalc",
                new ResolvedParameter<ICalculator>("catcalc"))
        );
        
        
        // Workery z nazwą "state" korzystały ze StateCalc
        cont.RegisterType<Worker>("state",
            new InjectionConstructor(
                     new ResolvedParameter<ICalculator>("statecalc"))
            );
        cont.RegisterType<Worker2>("state",
            new InjectionProperty("m_calc",
                new ResolvedParameter<ICalculator>("statecalc"))
        );
        cont.RegisterType<Worker3>("state",
            new InjectionMethod("SetCalc",
                new ResolvedParameter<ICalculator>("statecalc"))
        );
        
        return cont;
    }

    static public IUnityContainer FromFile()
    {
        var configFile = new ExeConfigurationFileMap { ExeConfigFilename = "/home/chrissy/dev/JP.NET/L3/UnityContainers/App.config" };
        Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFile,
            ConfigurationUserLevel.None);
        UnityConfigurationSection section =
            (UnityConfigurationSection)config.GetSection("unity");
        IUnityContainer cont = new UnityContainer();
        section.Configure(cont, "");

        return cont;
    }
}