using Domain.Entities.Communication;

namespace Domain.Entities.Interfaces;

public interface ITaggable
{
    ICollection<FeedbackTag> FeedbackTags { get; set; }
}