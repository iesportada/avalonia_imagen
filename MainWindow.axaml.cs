using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace AvaloniaApplication1;

public partial class MainWindow : Window
{
    private bool _salir = false;
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private void CargarListBox(string[] lista)
    {
        var lo = new List<string>(lista);
        lo.Sort();
        LblSuperior.Content = LbLista.SelectedIndex;
        LbLista.UnselectAll();
        LbLista.Items.Clear();
        foreach (string f in lo)
        {
            LbLista.Items.Add(Path.GetFileName(f));
        }    
    }
    private void CargarImagenAleatoria()
    {
        string ruta = "Imagenes";
        //string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes");
        string[] listado = Directory.GetFiles(ruta);
        var rnd = new Random();

        string img = listado[rnd.Next(listado.Length)];
        ImgPersonaje.Source = new Bitmap(img);
        LblSuperior.Content = Path.GetFileName(img);
        
        CargarListBox(listado);
    }
    //no funciona
    private Bitmap?[] GetResourceImages()
    {
        PropertyInfo[] props = typeof(ImageDrawing).GetProperties(BindingFlags.NonPublic | BindingFlags.Static);
        var images = props.Where(prop => prop.PropertyType == typeof(Bitmap)).Select(prop => prop.GetValue(null, null) as Bitmap).ToArray();

        return images;
    }
    
    private void CargarImagenAleatoriaAsset()
    { 
        Bitmap?[] listado = GetResourceImages();
        if (listado.Length > 0)
        {
            Random rnd = new Random();
            ImgPersonaje.Source = listado[rnd.Next(listado.Length)];
        }
    }
    private void BtnCargarImagen_OnClick(object? sender, RoutedEventArgs e)
    {
        LblEstado.Content = $"Botón pulsado {Environment.CurrentDirectory}";
        CargarImagenAleatoria();
    }

    private void BtnSalir_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();    
    }
#pragma warning disable 0618    
    private async void BtnAbrirDirectorio_OnClick(object? sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog();
        dlg.AllowMultiple = true;
        dlg.Filters.Add(new FileDialogFilter()
        {
            Name = "Archivos JPG|PNG",
            Extensions = {"jpg", "jpeg", "png"}
        });
        var resultado = await dlg.ShowAsync(this);
        if (resultado != null)
        {
            LblEstado.Content = $"Imagen cargada: {resultado[0]}";
            ImgPersonaje.Source = new Bitmap(resultado[0]);
        }
            
    }
    #pragma warning restore 0618
    private async void BtnAbrirDirectorio_OnClick_New(object? sender, RoutedEventArgs e)
    {
        /* En linux, hay que añadir al final del Program.cs
        .With(new X11PlatformOptions
        {
            UseDBusFilePicker = false // to disable FreeDesktop file picker
        });
        */
        
        var cwd = this.StorageProvider;
        var uri = Environment.CurrentDirectory + "/Imagenes";
        var ruta = await StorageProvider.TryGetFolderFromPathAsync(uri);
        
        var files = await cwd.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Abrir Archivo de imagen",
            AllowMultiple = false,
            FileTypeFilter = new[] { FilePickerFileTypes.ImageJpg, FilePickerFileTypes.ImagePng }, 
            SuggestedStartLocation = ruta
        });
        
        if (files.Count == 1)
        {
            var nuevaRuta = files[0].Path;
            LblEstado.Content = nuevaRuta;
            ImgPersonaje.Source = new Bitmap(nuevaRuta.LocalPath);
        }
    }

    private async void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (!_salir) //aún no hemos confirmado con la ventana informativa
        {
            e.Cancel = true;
            var box = MessageBoxManager.GetMessageBoxStandard("Confirmar fin aplicación",
                "¿Estás seguro/a de cerrar la aplicación?",
                ButtonEnum.OkCancel);
            if (await box.ShowWindowAsync() == ButtonResult.Ok)
            {
                _salir = true;
                Close();
            }
        }
    }

    private void ImgPersonaje_OnPointerEntered(object? sender, PointerEventArgs e)
    {
        LblSuperior.Opacity = 100;
    }

    private void ImgPersonaje_OnPointerExited(object? sender, PointerEventArgs e)
    {
        LblSuperior.Opacity = 0;
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        CargarImagenAleatoria();
    }

    private void LbLista_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (LbLista.SelectedIndex != -1)
        {
            string seleccion = (string) LbLista.SelectedItems[0];
            LblEstado.Content = seleccion;
            ImgPersonaje.Source = new Bitmap(Path.Combine("Imagenes", seleccion));
        }
    }

    private void BtnCargarTextBlock_OnClick(object? sender, RoutedEventArgs e)
    {
        var miText = new TextBlock();
        miText.Margin = new Thickness(5.0, 0.0);
        miText.FontSize = 12;
        miText.FontWeight = FontWeight.Bold;
        miText.FontStyle = FontStyle.Italic;
        miText.Text = "Saludos, Avalonios";
        StPanelLeft.Children.Add(miText);
    }

    private void MnCalculadora_OnClick(object? sender, RoutedEventArgs e)
    {
        new CalculadoraWindow().ShowDialog(this);
    }

    private void MnCalendario_OnClick(object? sender, RoutedEventArgs e)
    {
        new FechasWindow().ShowDialog(this);
    }

    private void MnEventos_OnClick(object? sender, RoutedEventArgs e)
    {
        new EventWindow().ShowDialog(this);
    }

    private void MnAcercaDe_OnClick(object? sender, RoutedEventArgs e)
    {
        new AcercaWindow().ShowDialog(this);
    }
}
