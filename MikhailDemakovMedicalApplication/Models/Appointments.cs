using System;

namespace MikhailDemakovMedicalApplication.Models;

public class Appointments
{
    public int appointment_id  { get; set; }
    public string patient_id  { get; set; }
    public string personnel_id  { get; set; }
    public DateTime appointment_date  { get; set; }
    public string payment_status  { get; set; }
    public string consultation_notes  { get; set; }
}