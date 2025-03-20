using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;

namespace Domain.Entities.Communication;

public class FeedbackTag : BaseEntity
{
    [Required]
    public Guid FeedbackId { get; set; }
    public Feedback Feedback { get; set; } = default!;
    
    [Required]
    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = default!;
}