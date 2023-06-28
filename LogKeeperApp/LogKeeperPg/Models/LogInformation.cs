using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LogKeeperPg.Models;

[Index(nameof(LogInformation.Uuid), IsUnique = true)]
public class LogInformation
{
    public LogInformation(Guid uuid,
        string title,
        string author,
        string project,
        DateTime time,
        string contents)
    {
        Uuid = uuid;
        Title = title;
        Author = author;
        Project = project;
        Time = time;
        Contents = contents;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required] [Column(TypeName = "uuid")] public Guid Uuid { get; set; }

    [Required] [MaxLength(50)] public string Title { get; set; }

    [Required] [MaxLength(50)] public string Author { get; set; }

    [Required] [MaxLength(50)] public string Project { get; set; }
    
    [Required] public DateTime Time { get; set; }

    [Required] public string Contents { get; set; }
}