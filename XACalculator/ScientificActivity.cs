using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Content;

namespace XACalculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class ScientificActivity : AppCompatActivity
    {
        // Variables
        private TextView calculatorText;
        private TextView operatorText;
        private Button standardButton;

        private readonly string[] numbers = new string[2];
        private string @operator;
        private string @function;

        private bool _justPressEqual = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_scientific);

            calculatorText = FindViewById<TextView>(Resource.Id.calculator_text_view); // Define the calculator textView
            operatorText = FindViewById<TextView>(Resource.Id.operator_text_view); // Define the operator textview
            standardButton = FindViewById<Button>(Resource.Id.standard_button); // Define the standard button

            // This button will be responsible for changing between activities
            // Expecially here, it will change from the scientific calculator to the standard calculator
            standardButton.Click += (object sender, EventArgs e) =>
            {
                Intent nextActivity = new Intent(this, typeof(StandardActivity));
                nextActivity.PutExtra("number", calculatorText.Text);
                StartActivity(nextActivity);
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        // Using Java.Export supported by xamarin to get all those buttons in axml file and access to them
        // Divide each button for each different task
        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button)v;

            // Check if user press qual or not, then put the mark point
            if ("=" == button.Text)
            {
                Calculate();
                this._justPressEqual = true;
            }
            else
            {
                if ("0123456789.".Contains(button.Text))
                {
                    AddDigitOrDecimalPoint(button.Text);
                }
                else if ("÷×+-".Contains(button.Text))
                {
                    AddOperator(button.Text);
                }
                else if ("SinCosTanInLog%!".Contains(button.Text))
                {
                    AddTrigoFunction(button.Text);
                }
                else if ("PiE".Contains(button.Text))
                {
                    AddPiE(button.Text);
                }
                else
                {
                    Clear();
                }

                this._justPressEqual = false;
            }

        }

        // This method is used for adding numbers or decimal point to the text view
        private void AddDigitOrDecimalPoint(string value)
        {
            // Check if there is operator or not and then adding value to number 1 or 2
            int index = @operator == null ? 0 : 1;

            // Jump into the CheckerActivity class to check the input
            if (CheckerActivity.IsInputValid(numbers, index, value))
            {
                // Check if user press equal or not
                // If user pressed equal and entered new number, 
                // it will automaticall replace the old number to the new one
                if (this._justPressEqual)
                {
                    numbers[index] = value;
                }
                else
                {
                    numbers[index] += value;
                }                
            }

            UpdateCalculatorText();
        }

        // This method is used for adding and calculating the basic operators 
        private void AddOperator(string value)
        {
            // Check if the preceding number is null or empty
            // If the previous number is null, it will not allow to add the operators
            @operator = numbers[0] == null ? null : value;

            if (numbers[1] != null && @operator != null)
            {
                Calculate();

                UpdateCalculatorText();
            }
            else
            {
                UpdateCalculatorText();
            }
        }

        // This method is used for adding and calculating the trigonometry equations 
        private void AddTrigoFunction(string value)
        {
            // Check if the preceding number is null or empty
            // If the previous number is null, it will not allow to add the operators
            @function = numbers[0] == null ? null : value;

            // Check if the function is assigned or not
            if (@function != null)
            {
                Calculate();
            }
        }

        // This method is used for adding and calculating the Pi and E number
        private void AddPiE(string value)
        {
            // Check if there is operator or not and then adding value to number 1 or 2
            int index = @operator == null ? 0 : 1;

            // Jump into the CheckerActivity class to check the input
            if (CheckerActivity.IsInputValid(numbers, index, value))
            {
                numbers[index] = value;
                Calculate();
            }

            UpdateCalculatorText();
        }

        // This method will calculate all operations 
        private void Calculate(string newOperatorFunction = null)
        {
            double? result = null;

            double? first = CheckerActivity.ParseValue(numbers[0]);
            double? second = CheckerActivity.ParseValue(numbers[1]);

            // Check if the text view already contained operation
            // It will calculate first and then jump into the condition for function
            if (first != null && @operator != null && second != null)
            {
                switch (@operator)
                {
                    case "÷":
                        result = first / second;
                        break;
                    case "×":
                        result = first * second;
                        break;
                    case "+":
                        result = first + second;
                        break;
                    case "-":
                        result = first - second;
                        break;
                }
            }
            if (result != null)
            {
                switch (@function)
                {
                    case "Sin":
                        result = Math.Sin((double)result);
                        break;
                    case "Cos":
                        result = Math.Cos((double)result);
                        break;
                    case "Tan":
                        result = Math.Tan((double)result);
                        break;
                    case "Log":
                        result = Math.Log10((double)result);
                        break;
                    case "In":
                        result = Math.Log((double)result);
                        break;
                    case "%":
                        result = (double)result / ConstantContainer.percent;
                        break;
                    case "!":
                        result = CheckerActivity.CalculateRecursion(result);
                        break;
                }
            }
            else
            {
                switch (@function)
                {
                    case "Sin":
                        result = Math.Sin((double)first);
                        break;
                    case "Cos":
                        result = Math.Cos((double)first);
                        break;
                    case "Tan":
                        result = Math.Tan((double)first);
                        break;
                    case "Log":
                        result = Math.Log10((double)first);
                        break;
                    case "In":
                        result = Math.Log((double)first);
                        break;
                    case "%":
                        result = (double)first / ConstantContainer.percent;
                        break;
                    case "!":
                        result = CheckerActivity.CalculateRecursion(first);
                        break;
                }
            }

            // Check if the result null or not and then check if the firt number is valid or not and get the second instead
            if (result == null)
            {
                result = first ?? second;
            }

            if (result != null)
            {
                double? finalResult = Math.Round((double)result, 8);
                numbers[0] = finalResult.ToString();
                @operator = newOperatorFunction;
                @function = newOperatorFunction;
                numbers[1] = null;
                UpdateCalculatorText();
            }
        }

        // Method for clearing all the text displayed
        private void Clear()
        {
            @function = null;
            numbers[0] = numbers[1] = null;
            @operator = null;
            UpdateCalculatorText();
        }

        // This medthod will update all the operations and digits to the text view
        private void UpdateCalculatorText()
        {
            operatorText.Text = @operator != null ? $"{@operator}" : null;

            if (numbers[0] != null && numbers[1] == null)
            {
                calculatorText.Text = $"{numbers[0]}";

            }
            else if (numbers[0] != null && numbers[1] != null)
            {
                calculatorText.Text = $"{numbers[1]}";
            }
            else
            {
                calculatorText.Text = $"{numbers[0]}";
            }
        }
    }
}