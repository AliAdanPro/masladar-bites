using Microsoft.Data.SqlClient;

public class UserService
{
    private readonly string _connectionString;

    public UserService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<bool> ValidateUserLoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return false;

        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(
                "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Password = @Password",
                connection);

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Login error: {ex.Message}");
            return false;
        }
    }
}