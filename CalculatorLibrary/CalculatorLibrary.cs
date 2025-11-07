using Newtonsoft.Json;
using System.Diagnostics;



namespace CalculatorLibrary
{

    public class Calculator
    {
        JsonWriter writer;
        int sleepingTime = 400;
        public Calculator()
        {
            StreamWriter logfile = File.CreateText("calculator.json");
            logfile.AutoFlush = true;
            writer = new JsonTextWriter(logfile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }
        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" which we use if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");

            // Use a switch statement to do the math. TEST PR
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    break;
                case "s":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    break;
                case "m":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                        writer.WriteValue("Divide");
                    }
                    break;
                case "e":
                    result = Math.Pow(num1, num2);
                    writer.WriteValue("Exponent");
                    break;
                case "t":
                    result = (num1 * (Math.Pow(10, num2)));
                    writer.WriteValue("10x");
                    break;
                // Return text for an incorrect option entry.
                default:
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();

            return result;
        }

        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }

        public double? ViewHistory(List<Calculation> calculations)
        {
            bool quit = false;
            string? userInput = "";
            List<Calculation> historyOptions = new List<Calculation>();
            while (!quit)
            {
                Console.WriteLine();
                int lastEntry = Math.Max(0, calculations.Count - 5);
                for (int i = calculations.Count - 1; i >= lastEntry; i--)
                {
                    Console.WriteLine(calculations[i].CalculationResult());
                    historyOptions.Add(calculations[i]);
                }

                Console.WriteLine("\nTo use a previous result press the entry number, then press Enter");
                Console.WriteLine("\nTo delete this history press 'd' then press Enter");
                Console.Write("\nor press any other key then Enter to return to the calculator: ");

                userInput = Console.ReadLine();
                int historyOption;
                if (int.TryParse(userInput, out historyOption))
                {
                    foreach (Calculation item in historyOptions)
                    {
                        if (item.calculationId == historyOption)
                        {
                            return item.result;
                        }
                    }
                }

                if (userInput == "d")
                {
                    calculations.Clear();
                }

                quit = true;
                return null;

            }

            return null;
        }

    }

    public class Calculation
    {
        public double num1
        {
            get; set;
        }
        public double num2
        {
            get; set;
        }
        public double result
        {
            get; set;
        }
        public string? operation
        {
            get; set;
        }
        public int calculationId
        {
            get; set;
        }
        public string CalculationResult()
        {
            return $"#{calculationId}: {Math.Round(num1, 2)} {operation} {Math.Round(num2, 2)} = {Math.Round(result, 2)}";
        }
        public Calculation(double num1, double num2, double result, string? operation, int counter)
        {
            string convertOperation = operation switch
            {
                "a" => "+",
                "s" => "-",
                "m" => "*",
                "d" => "/",
                "e" => "^",
                "t" => "x10",
                _ => "unknown"
            };

            this.num1 = num1;
            this.num2 = num2;
            this.result = result;
            this.operation = convertOperation;
            this.calculationId = counter;

        }
    }
}
