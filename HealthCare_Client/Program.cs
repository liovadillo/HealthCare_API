using HealthCare_Client.DTOs.Patient;
using System.Net.Http.Json;

var client = new HttpClient();
client.BaseAddress = new Uri("https://healthcare-api-evelio-g9b9h7c6cdhybje5.centralus-01.azurewebsites.net");

var newPatient = new CreatePatientDTO
{
    Name = "Mariela",
    Age = 32,
    Diagnosis = "SOP",
    IsActive = true,
    DateOfBirth = new DateTime(1993, 6, 7)
};

var response = await client.PostAsJsonAsync("/api/patient", newPatient);

if (response.IsSuccessStatusCode)
{
    var created = await response.Content.ReadFromJsonAsync<PatientDTO>();
    Console.WriteLine($"Created: {created.Id} | {created.Name}");
}
else
{
    Console.WriteLine($"Error: {response.StatusCode}");
}

// GET todos los pacientes
var patients = await client.GetFromJsonAsync<List<PatientDTO>>("/api/patient");

foreach (var patient in patients)
{
    Console.WriteLine($"ID: {patient.Id} | Name: {patient.Name} | Diagnosis: {patient.Diagnosis}");
}