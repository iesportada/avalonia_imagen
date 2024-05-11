using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AvaloniaApplication1;

public partial class CalculadoraWindow : Window
{
    private double _op1, _op2, _resultado = 0.0f;
    private char _operador;
    
    private bool _acumula = false;  // el display debe añadir, no empezar de cero
    private int _opActivo = 1;
    public CalculadoraWindow()
    {
        InitializeComponent();
    }
    private void BtnC_OnClick(object? sender, RoutedEventArgs e)
    {
        _resultado = 0;
        _acumula = false;
        _op1 = 0;
        _op2 = 0;
        _operador = '\0';
        TbDisplay.Text = "0";
    }

    private void BtnSigno_OnClick(object? sender, RoutedEventArgs e)
    {
        if (TbDisplay.Text != "0")
        {
            if (TbDisplay.Text!.Contains('-'))
                TbDisplay.Text = TbDisplay.Text.Remove(0, 1);
            else
                TbDisplay.Text = "-" + TbDisplay.Text;
            
            if (_opActivo == 1)
                double.TryParse(TbDisplay.Text, out _op1); 
            else if (_opActivo == 2)
                double.TryParse(TbDisplay.Text, out _op2);
        }
    }

    private void BtnDecimal_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!TbDisplay.Text!.Contains(','))
            TbDisplay.Text += ",";
    }
    // algún número ha sido pulsado
    private void Btn_OnClick(object? sender, RoutedEventArgs e)
    {
        var nuevoDigito = ((Button) sender!).Content!.ToString();
        
        if (_acumula)
            TbDisplay.Text += nuevoDigito;
        else
        {
            TbDisplay.Text = nuevoDigito;
            _acumula = true;
        }

        if (_opActivo == 1)
            double.TryParse(TbDisplay.Text, out _op1); 
        else if (_opActivo == 2)
            double.TryParse(TbDisplay.Text, out _op2);
    }
    // alguna operación ha sido pulsada
    private void BtnOperacion_OnClick(object? sender, RoutedEventArgs e)
    {
        _acumula = false;
        if (_opActivo == 1)
            _opActivo = 2;
        else
        {
            Opera();
            _op1 = _resultado;
        }
        _operador = ((Button)sender!).Content!.ToString()![0];
    }

    private void Opera()
    {
        switch (_operador)
        {
            case '+':
                _resultado = _op1 + _op2;
                break;
            case '-': 
                _resultado = _op1 - _op2;
                break;
            case '*':
                _resultado = _op1 * _op2;
                break;
            case '/':
                _resultado = _op1 / _op2;
                break;
        }
        TbDisplay.Text = _resultado.ToString();
    }
    private void BtnEnter_OnClick(object? sender, RoutedEventArgs e)
    {
        Opera();
        _opActivo = 1;
        _op1 = 0;
        _op2 = 0;
        _acumula = false;
    }

    private void MnCopiar_OnClick(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window }) {
            window.Clipboard?.SetTextAsync(TbDisplay.Text);
            //LblEstado.Content = await window.Clipboard!.GetTextAsync();
            MnPegar.IsEnabled = true;
        }
        //Clipboard.SetTextAsync();
    }

    private async void MnPegar_OnClick(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window }) {
            LblEstado.Content = await window.Clipboard!.GetTextAsync();
            MnPegar.IsEnabled = false;
        }
    }
}