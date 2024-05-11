using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace AvaloniaApplication1;

public partial class EventWindow : Window
{
    public EventWindow()
    {
        InitializeComponent();
        BtnInterno.Focus();
    }
    private async void BtnExterno_OnClick(object? sender, RoutedEventArgs e)
    {
        await MessageBoxManager.GetMessageBoxStandard("Atención","Botón externo", ButtonEnum.Ok).ShowAsync();
        LblEstado.Content += "externo ";
    }

    private async void BtnInterno_OnClick(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
        await MessageBoxManager.GetMessageBoxStandard("Atención","Botón interno",  ButtonEnum.Ok).ShowAsync();
        LblEstado.Content += "interno ";
    }

    private async void InputElement_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyModifiers == KeyModifiers.Control && e.Key==Key.C)
            await MessageBoxManager.GetMessageBoxStandard("Atención","Se ha pulsado Ctrl+C",  ButtonEnum.Ok).ShowAsync();
    }
    private async void BtnInterno_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        e.Handled = true;
        
        var point = e.GetCurrentPoint(sender as Control);
        var x = point.Position.X;
        var y = point.Position.Y;
        var msg = $"Ratón pulsado en posición {x}, {y} relativa.";
        
        if (point.Properties.IsMiddleButtonPressed)
        {
            msg += " Rueda central pulsada.";
        }
        if (point.Properties.IsRightButtonPressed)
        {
            msg += " Botón derecho pulsado.";
        }
        if (point.Properties.IsBarrelButtonPressed)
        {
            msg += " Botón de lápiz digitalizador pulsado.";
        }
        await MessageBoxManager.GetMessageBoxStandard("Interno",msg,  ButtonEnum.Ok).ShowAsync();
    }
}