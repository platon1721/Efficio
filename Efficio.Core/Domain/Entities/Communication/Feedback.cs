using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Interfaces;
using Efficio.Core.Domain.Entities.Base;

namespace Efficio.Core.Domain.Entities.Communication;

public class Feedback: BaseEntity, ITaggable, IDescribable, ICommentable
{
    [Required]
    [MaxLength(120)]
    public string Title { get; set; } = default!;
    
    [Required]
    [MaxLength(5000)]
    public string Description { get; set; } = default!;
    
    // Picture
    public string? ImagePath { get; set; }
    [Range(0, 5 * 1024 * 1024, ErrorMessage = "Picture size must be between 0 and 5MB.")]
    public long? ImageSize { get; set; }
    
    // User id
    [Required]
    public Guid MadeBy { get; set; }

    // Tags
    public ICollection<FeedbackTag> FeedbackTags { get; set; } = new List<FeedbackTag>();
    [NotMapped]
    public IEnumerable<Tag> Tags => FeedbackTags.Select(ft => ft.Tag);
    
    // Comments
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}