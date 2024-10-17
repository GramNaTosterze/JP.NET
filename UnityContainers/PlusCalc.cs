namespace UnityContainers;

class PlusCalc: ICalculator
{
    public String Eval(string a, string b)
    {
        var numA = double.Parse(a);
        var numB = double.Parse(b);
        
        return $"{numA + numB}";
    } 
}