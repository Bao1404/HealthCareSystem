namespace BusinessObjects;

public partial class Aimessage
{
    public int AimessageId { get; set; }

    public int AiconversationId { get; set; }

    public string Sender { get; set; } = null!;

    public string? MessageType { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? SentAt { get; set; }

    public bool? IsRead { get; set; }

    public virtual Aiconversation Aiconversation { get; set; } = null!;
}
