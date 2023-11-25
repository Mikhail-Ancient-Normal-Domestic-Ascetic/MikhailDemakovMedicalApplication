using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace MikhailDemakovMedicalApplication;

public partial class Authorization : Window
{
    public Authorization()
    {
        InitializeComponent();
    }

    private void LoginButton_OnClick(object? sender, RoutedEventArgs e)
    {
        string username = Login.Text;
        string password = Password.Text;
        try 
        {
            if (username == "doctor" && password == "doctor")
            {
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
            else
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Ошибка входа", "Неверный логин или пароль", ButtonEnum.Ok);
                box.ShowAsync();
                return;
            }
        }
        catch (Exception ex)
        {
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Ошибка входа", "Неверный логин или пароль", ButtonEnum.Ok);
                box.ShowAsync();
                return;
            }
        }
        new MainWindow().Show(); 
        this.Close();
    }
}