using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySqlConnector;
using MikhailDemakovMedicalApplication.Models;

namespace MikhailDemakovMedicalApplication;

public partial class MainWindow : Window
{
    private List<Appointments> Appointments_ { get; set; }
    private List<Personnel> Personnel_ { get; set; }
    private MySqlConnectionStringBuilder _connectionSb;

    public MainWindow()
    {
        InitializeComponent();
        
        _connectionSb = new MySqlConnectionStringBuilder
        {
            Server = "10.10.1.24",
            Database = "pro8",
            UserID = "user_01",
            Password = "user01pro"
            // Server = "localhost",
            // Database = "pr1",
            // UserID = "root",
            // Password = "123456"
        };
        UpdateDataGrid();
        
    }
    void UpdateDataGrid()
    {
        Appointments_ = new List<Appointments>();
        Personnel_ = new List<Personnel>();
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())  //Фильтрация по статусу
            {
                command.CommandText = "SELECT * From Personnel"; 
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Personnel_.Add(new Personnel()
                        {
                            personnel_id = reader.GetInt32("personnel_id"), 
                            last_name = reader.GetString("last_name")
                        });
                    }
                }
                    
            }
            connection.Close();
        }
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())  // Вывод основной таблицы
            {
                command.CommandText = "SELECT * From Appointments  " +
                                      "JOIN Patients " +
                                      "ON Appointments.patient_id = Patients.patient_id " +
                                      "JOIN Personnel ON Appointments.personnel_id = Personnel.personnel_id ";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Appointments_.Add(new Appointments()
                        {
                            appointment_id = reader.GetInt32("appointment_id"),
                            patient_id = reader.GetString ("last_name_patient"),
                            personnel_id = reader.GetString ("last_name"),
                            appointment_date = reader.GetDateTime("appointment_date"),
                            payment_status = reader.GetString("payment_status"),
                            consultation_notes = reader.GetString("consultation_notes"),
                        });
                    }
                }

            }

            connection.Close();
        }
        AppointmentsDataGrid.ItemsSource = Appointments_;
        StatusFilterBox.ItemsSource = Personnel_;
    }
    private void DeleteWindow_OnClick(object? sender,
        RoutedEventArgs e)
    {
        var patient = AppointmentsDataGrid.SelectedItem as Appointments;
        if (patient == null)
            return;
        using (var cnn = new MySqlConnection(_connectionSb.ConnectionString))
        using (var cmd = cnn.CreateCommand())
        {
            cmd.CommandText = "SELECT * FROM Appointments";
            cmd.Parameters.AddWithValue("@patient_id", patient.patient_id);
            cnn.Open();
            var deleted = cmd.ExecuteNonQuery();
        }
        Appointments_.Remove(patient);
        AppointmentsDataGrid.ItemsSource = Appointments_;
    }
    private void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                var datetime = Convert.ToDateTime(appointment_date_TextBox.Text).ToString("yyyy-M-d");
                command.CommandText = $"INSERT INTO Appointments (appointment_id, patient_id, personnel_id, appointment_date, payment_status, consultation_notes) " +
                                      $"VALUES ({Convert.ToInt32(appointment_id_TextBox.Text)}, '{patient_id_TextBox.Text}', '{personnel_id_TextBox.Text}', " +
                                      $"'{datetime}', '{payment_status_TextBox.Text}', '{consultation_notes_TextBox.Text}')";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
    private void ClientBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(TextClient.Text))
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка!", "Пустой поисковый запрос", ButtonEnum.Ok);
            box.ShowAsync();
            return;
        }

        AppointmentsDataGrid.SelectedItems.Clear();
        var foundItems = Appointments_.Where(s => s.patient_id.Contains(TextClient.Text));
        foreach (var found in foundItems) 
        {
            AppointmentsDataGrid.SelectedItems.Add(found);
        }
    }


    private void StatusFilterBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var selectedItem = StatusFilterBox.SelectedItem;
        if (selectedItem == null)
        {
            return;
        }

        var foundStatus = Appointments_.Where(s => s.personnel_id == (selectedItem as Personnel).last_name);
        AppointmentsDataGrid.ItemsSource = foundStatus;
    }

    private void BtReset_OnClick(object? sender, RoutedEventArgs e) //кнопка обновления
    {
        
        UpdateDataGrid();
    }
}