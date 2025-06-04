using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BlazorApp23.Components.Pages
{
    public class FeedbackService
    {
        private readonly string _connectionString;

        public FeedbackService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task SubmitFeedbackAsync(FeedbackModel feedback)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();

                var cmd = new SqlCommand("sp_SubmitFeedback", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Name", feedback.Name);
                cmd.Parameters.AddWithValue("@Email", feedback.Email);
                cmd.Parameters.AddWithValue("@Rating", feedback.Rating);
                cmd.Parameters.AddWithValue("@Message", feedback.Message);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<Feedback>> GetRecentFeedbackAsync(int count = 10)
        {
            var feedbackList = new List<Feedback>();

            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("sp_GetRecentFeedback", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Count", count);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        feedbackList.Add(new Feedback
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Rating = reader.GetInt32(3),
                            Message = reader.GetString(4),
                            SubmissionDate = reader.GetDateTime(5)
                        });
                    }
                }
            }
            return feedbackList;
        }

        public async Task<List<Feedback>> GetAllFeedbackAsync()
        {
            var feedbackList = new List<Feedback>();

            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT * FROM Feedback ORDER BY SubmissionDate DESC", conn);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        feedbackList.Add(new Feedback
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Rating = reader.GetInt32(3),
                            Message = reader.GetString(4),
                            SubmissionDate = reader.GetDateTime(5)
                        });
                    }
                }
            }
            return feedbackList;
        }

        public async Task DeleteFeedbackAsync(int id)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("DELETE FROM Feedback WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public partial class Feedback
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime SubmissionDate { get; set; }
    }

    public class FeedbackModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Feedback message is required")]
        [StringLength(1000, ErrorMessage = "Message cannot exceed 1000 characters")]
        public string Message { get; set; } = string.Empty;
    }
}