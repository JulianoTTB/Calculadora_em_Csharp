using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq.Expressions;

namespace Calculadora
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // Variáveis
        private bool errorMode = false;
        private bool secondNumberOn = false;
        private bool decimalOn = false;
        private bool operationOn = false;
        private bool resultOccured = false;
        
        private double firstNumber = 0;
        private double secondNumber = 0;

        private string operation;

        private void BtnNumber_Click(object sender, RoutedEventArgs e)
        {
            Button buttonClicked = sender as Button;
            Update_Visor((string) buttonClicked.Content);
        }

        private void BtnOperation_Click(object sender, RoutedEventArgs e)
        {
            Button buttonClicked = sender as Button;
            string operationClicked = (string) buttonClicked.Content;
            if (buttonClicked == potencia)
            {
                operationClicked = "^";
            }
            Register_Operation(operationClicked);

        }

        private void Register_Operation(string operationClicked)
        {
            if (errorMode)
            {
                Clean_All();
                return;
            }
            
        
            switch (operationClicked)
            {
                case "C":
                    Clean();
                    return;

                case "+": case "-": case "x": case "/": case "√x": case "^":
                    if (!operationOn)
                    {
                        operation = operationClicked;
                        operationOn = true;
                        resultOccured = false;
                        break;
                    }
                    return;
                   
                case "+/-":
                    if (visor.Text[0] == '-') visor.Text = visor.Text.Remove(0,1);
                    else visor.Text = visor.Text.Insert(0, "-");
                    return;

                case "=":
                    if (!secondNumberOn) return;
                    secondNumber = double.Parse(visor.Text, System.Globalization.CultureInfo.InvariantCulture);
                    Update_VisorHistory(visor.Text, "");
                    Operations(firstNumber, secondNumber);
                    return;
            }
            visorHistory.Text = "";
            Update_VisorHistory(visor.Text, operation);
            secondNumberOn = true;
            firstNumber = double.Parse(visor.Text, System.Globalization.CultureInfo.InvariantCulture);
            Clean();
        }

        private void Operations(double num1, double num2)
        {
            operationOn = false;
            decimalOn = false;

            double result = 0;

            switch (operation)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "-":
                    result = firstNumber - secondNumber;
                    break;
                case "x":
                    result = firstNumber * secondNumber;
                    break;
                case "/":
                    if (secondNumber == 0)
                    {
                        Error_Occured("Não pode dividir por 0");
                        return;
                    }
                    else result = firstNumber / secondNumber;
                    break;
                case "√x":
                    result = Math.Pow(firstNumber, 1 / secondNumber);
                    break;
                case "^":
                    result = Math.Pow(firstNumber, secondNumber);
                    break;
            }


            firstNumber = result;
            visor.Text = result.ToString().Substring(0, Math.Min(result.ToString().Length, 13));
            resultOccured = true;
        }


        private void Error_Occured(string message)
        {
            errorMode = true;
            visor.Text = "Erro: " + message;
        }

        private void Update_VisorHistory(string number, string operation)
        {
            visorHistory.Text += number + " " + operation + " ";
        }

        private void Update_Visor(string numberString)
        {
            if (errorMode)
            {
                Clean_All();
                return;
            }
            if (resultOccured) return;
            //Limite de caracteres visíveis no visor 
            if(visor.Text.Length == 13) return;

            if (decimalOn && numberString == ".") return;

            if (numberString == ".") decimalOn = true;

            if (visor.Text == "0" && numberString != ".")
            {
                visor.Text = numberString;
            }
            else
            {
                visor.Text += numberString;
            }
        }

        private void Clean()
        {
            // se o mode de erro está ativado a única possibilidade é limpar tudo
            if (errorMode || visor.Text == "0")
            {
                Clean_All();
                return;
            }

            decimalOn = false;
            if (secondNumberOn)
            {
                secondNumber = 0;
            }
            else
            {
                firstNumber = 0;
            }

            visor.Text = "0";
        }

        private void Clean_All()
        {
            //Reseta a calculadora
            errorMode = false;
            secondNumberOn = false;
            decimalOn = false;
            operationOn = false;
            resultOccured = false;
            firstNumber = 0;
            secondNumber = 0;
            visor.Text = "0";
            visorHistory.Text = "";
        }

        // Cor original do botão
        private Brush originalBackground;
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            originalBackground = button.Background;
            button.Background = (Brush)new BrushConverter().ConvertFrom("#6f6f6f");
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Background = originalBackground;
        }
    }
}