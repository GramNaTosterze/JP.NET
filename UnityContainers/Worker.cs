namespace UnityContainers;

public class Worker
{
    private ICalculator m_calc;
    
    public Worker(ICalculator calc)
    {
        m_calc = calc;
    }

    public void Work(string a, string b)
    {
        Console.WriteLine($"{m_calc.Eval(a,b)}");
    }
}