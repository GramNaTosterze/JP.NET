namespace UnityContainers;

public class Worker3
{
    private ICalculator m_calc;

    public void SetCalc(ICalculator calc)
    {
        m_calc = calc;
    }
    public void Work(string a, string b)
    {
        Console.WriteLine($"{m_calc.Eval(a,b)}");
    }
}