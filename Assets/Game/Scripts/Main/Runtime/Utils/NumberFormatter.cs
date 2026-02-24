using System;

public static class NumberFormatter
{
    private const long Thousand = 1000;
    private const long Million = 1000000;
    private const long Billion = 1000000000;
    private const long Trillion = 1000000000000;

    public static string FormatNumber(long number)
    {
        if (number < Thousand)
        {
            return number.ToString();
        }
        else if (number < Million)
        {
            return (number / (double)Thousand).ToString("0.#") + "k";
        }
        else if (number < Billion)
        {
            return (number / (double)Million).ToString("0.#") + "m";
        }
        else if (number < Trillion)
        {
            return (number / (double)Billion).ToString("0.#") + "b";
        }
        else
        {
            return (number / (double)Trillion).ToString("0.#") + "t";
        }
    }

    public static string FormatNumber(double number)
    {
        if (number < Thousand)
        {
            return number.ToString("0.#");
        }
        else if (number < Million)
        {
            return (number / Thousand).ToString("0.#") + "k";
        }
        else if (number < Billion)
        {
            return (number / Million).ToString("0.#") + "m";
        }
        else if (number < Trillion)
        {
            return (number / Billion).ToString("0.#") + "b";
        }
        else
        {
            return (number / Trillion).ToString("0.#") + "t";
        }
    }
}