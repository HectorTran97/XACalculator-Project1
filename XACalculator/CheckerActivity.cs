using System;

namespace XACalculator
{
    public static class CheckerActivity
    {
        // This method is checking the valid input entered 
        public static bool IsInputValid(string[] numbers, int index, string value)
        {
            if (!int.TryParse(value, out var _))
            {
                switch (value)
                {
                    case ".":
                        if (string.IsNullOrEmpty(numbers[index]))
                        {
                            numbers[index] = "0";
                        }
                        else if (numbers[index].Contains(".") == true)
                        {
                            return false;
                        }

                        break;

                    case "Pi":
                        //if (string.IsNullOrEmpty(numbers[index]))
                        //{
                        //    return true;
                        //}

                        //if (numbers[index].Contains("PI", StringComparison.OrdinalIgnoreCase) || numbers[index].Contains("E", StringComparison.OrdinalIgnoreCase) || numbers[index].Contains("!") || numbers[index].Contains("%"))
                        //{
                        //    return false;
                        //}
                        //if (numbers[index].Contains(numbers[index]))
                        //{
                        //    return false;
                        //}
                        IsValidValue(numbers, index);

                        break;

                    case "E":
                        //if (string.IsNullOrEmpty(numbers[index]))
                        //{
                        //    return true;
                        //}

                        //if (numbers[index].Contains("PI", StringComparison.OrdinalIgnoreCase) || numbers[index].Contains("E", StringComparison.OrdinalIgnoreCase) || numbers[index].Contains("!") || numbers[index].Contains("%"))
                        //{
                        //    return false;
                        //}
                        //if (numbers[index].Contains(numbers[index]))
                        //{
                        //    return false;
                        //}
                        IsValidValue(numbers, index);

                        break;
                }
            }

            return true;
        }

        // This method is parse value for number
        public static double? ParseValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            switch (value)
            {
                case "Pi":
                    return (double)Math.PI;
                case "E":
                    return (double)Math.E;
                default:
                    return (double?)double.Parse(value);
            }
        }

        // This method is checking the value for Pi and E number
        public static bool IsValidValue(string[] numbers, int index)
        {
            if (string.IsNullOrEmpty(numbers[index]))
            {
                return true;
            }

            if (numbers[index].Contains("PI", StringComparison.OrdinalIgnoreCase) || numbers[index].Contains("E", StringComparison.OrdinalIgnoreCase) || numbers[index].Contains("!") || numbers[index].Contains("%"))
            {
                return false;
            }

            if (numbers[index].Contains(numbers[index]))
            {
                return false;
            }

            return false;
        }

        // This method is to calculate the recursion equation ("!")
        public static double? CalculateRecursion(double? number)
        {
            double? recurNum = 1;
            for (int i = 1; i <= number; i++)
            {
                recurNum *= i;
            }

            return recurNum;
        }
    }
}