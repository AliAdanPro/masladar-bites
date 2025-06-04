using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;

namespace BlazorApp23.Components.Services
{
    public class ClientAuthService
    {
        private readonly string _connectionString;

        public ClientAuthService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> ValidateClientLoginAsync(string username, string password)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT COUNT(1) FROM Clients WHERE Username = @Username AND Password = @Password";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        var result = await command.ExecuteScalarAsync();
                        return Convert.ToInt32(result) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"Login error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegisterClientAsync(ClientRegistrationModel model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"INSERT INTO Clients (Username, Password, Email, FullName, PhoneNumber) 
                              VALUES (@Username, @Password, @Email, @FullName, @PhoneNumber)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", model.Username);
                        command.Parameters.AddWithValue("@Password", model.Password);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@FullName", model.FullName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber ?? (object)DBNull.Value);

                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT COUNT(1) FROM Clients WHERE Username = @Username";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        var result = await command.ExecuteScalarAsync();
                        return Convert.ToInt32(result) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Username check error: {ex.Message}");
                return true;
            }
        }
    }

    public class ClientRegistrationModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}