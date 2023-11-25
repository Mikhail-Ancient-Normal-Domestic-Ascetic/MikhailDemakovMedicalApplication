using System;

namespace MikhailDemakovMedicalApplication.Models;

public class Patients
{
    public int patient_id { get; set; }
    public string first_name  { get; set; }
    public string last_name  { get; set; }
    public string contact_information  { get; set; }
    public DateTime date_of_birth  { get; set; }
    public DateTime registration_date  { get; set; }
    public string medical_history  { get; set; }
    public string current_health_condition  { get; set; }
}