namespace UnityContainers;

class StateCalc(int i = 0) : ICalculator
{
    private int _i = i;

    public String Eval(string a, string b)
    {
        return $"{a}{b}{_i++}";
    } 
}