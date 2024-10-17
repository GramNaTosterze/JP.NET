namespace UnityContainers;

class CatCalc: ICalculator
{
    public String Eval(string a, string b)
    {
        return $"{a}{b}";
    } 
}