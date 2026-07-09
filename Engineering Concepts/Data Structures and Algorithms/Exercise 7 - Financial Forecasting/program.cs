using System;

class Program
{
    static double CalculateFutureValue(double currentValue, double growthRate, int years)
    {
        if (years == 0)
            return currentValue;

        return CalculateFutureValue(currentValue * (1 + growthRate), growthRate, years - 1);
    }

    static void Main()
    {
        double currentValue = 10000;
        double growthRate = 0.10;
        int years = 5;

        double futureValue = CalculateFutureValue(currentValue, growthRate, years);

        Console.WriteLine("Current Value: " + currentValue);
        Console.WriteLine("Growth Rate: " + (growthRate * 100) + "%");
        Console.WriteLine("Years: " + years);
        Console.WriteLine("Future Value: " + futureValue);

        Console.WriteLine("\nTime Complexity");
        Console.WriteLine("Recursive Forecasting : O(n)");
    }
}