using System.ComponentModel.DataAnnotations;

namespace WebApiTemplate.Infrastructure.Persistence.MongoDb
{
    public class DatabaseOptions 
    {
        public static string SectionName = nameof(DatabaseOptions);

        [Required]
        public string? ConnectionString { get; set; }

        [Required]
        public string? Database { get; set; }
    }
}
