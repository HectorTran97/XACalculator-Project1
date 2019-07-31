using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;

namespace XACalculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        // Variables
        private TextView calculatorText;

        private readonly string[] numbers = new string[2];
        private string @operator;
        
        //private IList<string> operatorList = new List<string>() { "÷", "×", "+", "-" };
        //private string inputText;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            calculatorText = FindViewById<TextView>(Resource.Id.calculator_text_view); // Define the TextView
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button)v;            

            if ("0123456789.".Contains(button.Text))
            {
                AddDigitOrDecimalPoint(button.Text);
            }
            else if ("÷×+-".Contains(button.Text))
            {
                AddOperator(button.Text);
            }
            else if ("=" == button.Text)
            {
                Calculate();
            }
            else
            {
                Clear();
            }
        }

        //private bool IsValid(string equationText, char input, int index)
        //{
        //    foreach (var item in operatorList)
        //    {
        //        if (inputText != item)
        //        {
        //            return false;
        //        }
        //    }
        //}

        private void AddDigitOrDecimalPoint(string value)
        {
            int index = (@operator == null) ? 0 : 1;

            if (value == "." && numbers[index].Contains("."))
            {
                return;
            }

            numbers[index] += value;

            UpdateCalculatorText();
        }

        private void AddOperator(string value)
        {
            if (numbers[1] != null)
            {
                Calculate(value);
                return;
            }

            @operator = value;

            UpdateCalculatorText();
        }

        private void Calculate(string newOperator = null)
        {
            double? result = null;
            double? first = numbers[0] == null ? null : (double?)double.Parse(numbers[0]);
            double? second = numbers[1] == null ? null : (double?)double.Parse(numbers[1]);

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

            if (result != null)
            {
                numbers[0] = result.ToString();
                @operator = newOperator;
                numbers[1] = null;
                UpdateCalculatorText();
            }
        }

        private void Clear()
        {
            numbers[0] = numbers[1] = null;
            @operator = null;
            UpdateCalculatorText();
        }

        //private void BackSpace()
        //{
        //    newText = string.Join("", numbers);

        //    for (int i = newText.Length - 1; i >= 0; i--)
        //    {
        //        if (newText == null || newText == " ")
        //        {
        //            Clear();
        //        }
        //        else
        //        {
        //            if (newText.Length != 0)
        //            {
        //                newText = newText.Remove(newText.Length - 1, 1);
        //                if (newText == null)
        //                {
        //                    calculatorText.Text = "0";
        //                }

        //                numbers[0] = newText;
        //                @operator = null;
        //                numbers[1] = null;
        //                UpdateCalculatorText();
        //            }
        //        }
        //    }
        //}

        private void UpdateCalculatorText() => calculatorText.Text = $"{numbers[0]} {@operator} {numbers[1]}";
    }
}
