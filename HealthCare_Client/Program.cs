using HealthCare_Client.DTOs.Patient;
using System.Net.Http.Json;

var client = new HttpClient();
client.BaseAddress = new Uri("https://healthcare-api-evelio-g9b9h7c6cdhybje5.centralus-01.azurewebsites.net");

// GET todos los pacientes
var patients = await client.GetFromJsonAsync<List<PatientDTO>>("/api/patient");

foreach (var patient in patients)
{
    Console.WriteLine($"ID: {patient.Id} | Name: {patient.Name} | Diagnosis: {patient.Diagnosis}");
}