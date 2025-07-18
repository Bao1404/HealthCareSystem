public class UserDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Role { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AvatarUrl { get; set; }
}
