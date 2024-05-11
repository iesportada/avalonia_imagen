using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaApplication1;

public partial class FechasWindow : Window
{
    public FechasWindow()
    {
        InitializeComponent();
        DtPicker1.MinYear = new DateTimeOffset(new DateTime(1980, 1, 1));
        DtPicker1.MaxYear = new DateTimeOffset(new DateTime(2040, 12, 31));
    }

    private void BtnHoy_OnClick(object? sender, RoutedEventArgs e)
    {
        Cld1.DisplayDate = DateTime.Now;
    }

    private void Cld1_OnSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (RbModoS.IsChecked == true)
        {
            TbFechaSimple.Text = (Cld1.SelectedDate != null)
                ? ((DateTime)Cld1.SelectedDate!).ToShortDateString()
                : "";
        }
        else
        {
            LbFechas.Items.Clear();
            foreach (var fecha in Cld1.SelectedDates)
                LbFechas.Items.Add(fecha.ToShortDateString());
        }
    }

    private void RbModoS_OnClick(object? sender, RoutedEventArgs e)
    {
        if (((RadioButton) sender).Name == "RbModoS")
            Cld1.SelectionMode = CalendarSelectionMode.SingleDate;
        else
            Cld1.SelectionMode = CalendarSelectionMode.MultipleRange;
    }
}