using Infrastructure.Common;
using Infrastructure.Interface;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Services;

public class DoctorService : IDoctorService
{
    public int AddDoctor(Doctor doctor)
    {
        using var connection = NpgsqlHelper.CreateConnection(SqlCommands.connectionString);
        using var command = new NpgsqlCommand(SqlCommands.createDoctor, connection);

        AddDoctorParameters(command, doctor);

        return (int)command.ExecuteScalar();
    }

    public bool DeleteDoctor(int id)
    {
        using var connection = NpgsqlHelper.CreateConnection(SqlCommands.connectionString);
        using var command = new NpgsqlCommand(SqlCommands.deleteDoctorByID, connection);

        command.Parameters.AddWithValue("@DoctorID", id);

        return command.ExecuteNonQuery() > 0;
    }

    public List<Doctor> GetAllDoctors()
    {
        var doctors = new List<Doctor>();
        using var connection = NpgsqlHelper.CreateConnection(SqlCommands.connectionString);
        using var command = new NpgsqlCommand(SqlCommands.selectAllDoctors, connection);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            doctors.Add(MapDoctor(reader));
        }

        return doctors;
    }

    public Doctor GetDoctorById(int id)
    {
        using var connection = NpgsqlHelper.CreateConnection(SqlCommands.connectionString);
        using var command = new NpgsqlCommand(SqlCommands.selectDoctorByID, connection);

        command.Parameters.AddWithValue("@DoctorID", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return MapDoctor(reader);
        }

        return null;
    }

    public bool UpdateDoctor(Doctor doctor)
    {
        using var connection = NpgsqlHelper.CreateConnection(SqlCommands.connectionString);
        using var command = new NpgsqlCommand(SqlCommands.updateDoctor, connection);

        AddDoctorParameters(command, doctor);
        command.Parameters.AddWithValue("@DoctorID", doctor.DoctorID);

        return command.ExecuteNonQuery() > 0;
    }

    private void AddDoctorParameters(NpgsqlCommand command, Doctor doctor)
    {
        command.Parameters.AddWithValue("@FirstName", doctor.FirstName);
        command.Parameters.AddWithValue("@LastName", doctor.LastName);
        command.Parameters.AddWithValue("@DateOfBirth", doctor.DateOfBirth);
        command.Parameters.AddWithValue("@IsActive" , doctor.IsActive);
        command.Parameters.AddWithValue("@Position",doctor.Position);
        command.Parameters.AddWithValue("@HireDate", doctor.HireDate);
        command.Parameters.AddWithValue("@Salary", doctor.Salary);
        command.Parameters.AddWithValue("@IsActive", doctor.IsActive);

    }

    private Doctor MapDoctor(NpgsqlDataReader reader)
    {
        return new Doctor
        {
            DoctorID = reader.GetInt32(reader.GetOrdinal("DoctorID")),
            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
            LastName = reader.GetString(reader.GetOrdinal("LastName")),
            DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
            Position = reader.GetString(reader.GetOrdinal("Position")),
            Department = reader.GetString(reader.GetOrdinal("Department")),
            HireDate = reader.GetDateTime(reader.GetOrdinal("HireDate")),
            Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))

        };
    }
}

public static class SqlCommands
{
    public static readonly string connectionString = @"Server=127.0.0.1;Port=5432;Database=doctordb;User Id=postgres;Password=832111;";
    
    public const string selectAllDoctors = "SELECT * FROM Doctor";
    public const string selectDoctorByID = "SELECT * FROM Doctor WHERE DoctorID = @DoctorID";
    public const string deleteDoctorByID = "DELETE FROM Doctor WHERE DoctorID = @DoctorID";
    public const string createDoctor = @"INSERT INTO Doctor (FirstName, LastName, Specialization)
                                         VALUES (@FirstName, @LastName, @Specialization)
                                         RETURNING DoctorID;";
    public const string updateDoctor = @"UPDATE Doctor
                                         SET FirstName = @FirstName,
                                             LastName = @LastName,
                                             Specialization = @Specialization
                                         WHERE DoctorID = @DoctorID";
}
