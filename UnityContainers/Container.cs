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
        cont.RegisterType<ICalculator, StateCalc>(
            "statecalc",
            new ContainerControlledLifetimeManager(),
            new InjectionConstructor(1)
            );

        
        // workers
        cont.RegisterType<Worker>(
            new InjectionConstructor(
                new ResolvedParameter<ICalculator>("catcalc"))
            );
        
        cont.RegisterType<Worker2>(
            new InjectionProperty("m_calc",
                new ResolvedParameter<ICalculator>("pluscalc"))
        );
        
        cont.RegisterType<Worker3>(
            new InjectionMethod("SetCalc",
                new ResolvedParameter<ICalculator>("catcalc"))
        );
        
        
        // state workers
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
}