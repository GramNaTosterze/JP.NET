namespace UnityContainers;

public class Worker2
{
    public ICalculator m_calc { get; set; }
    
    public void Work(string a, string b)
    {
        Console.WriteLine($"{m_calc.Eval(a,b)}");
    }
}