namespace EduHub.Ch01.Domain;

/// <summary>
/// Đại diện cho Câu lạc bộ trong trường đại học.
/// Quản lý danh sách thành viên tham gia và phân định Trưởng CLB.
/// </summary>
public class Club
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid LeaderId { get; private set; }

    private readonly List<Guid> _memberIds = new();
    public IReadOnlyCollection<Guid> MemberIds => _memberIds.AsReadOnly();

    public DateTime CreatedAt { get; private set; }

    // Constructor private bảo vệ tính toàn vẹn dữ liệu
    private Club(Guid id, string code, string name, string description, Guid leaderId, DateTime createdAt)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        LeaderId = leaderId;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Factory method static khởi tạo Câu lạc bộ mới.
    /// </summary>
    public static Club Create(string code, string name, string description, Guid leaderId)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Mã CLB (Code) không được để trống.", nameof(code));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên CLB (Name) không được để trống.", nameof(name));

        if (leaderId == Guid.Empty)
            throw new ArgumentException("ID Trưởng CLB (LeaderId) không hợp lệ.", nameof(leaderId));

        var club = new Club(
            id: Guid.NewGuid(),
            code: code.Trim().ToUpperInvariant(),
            name: name.Trim(),
            description: description?.Trim() ?? string.Empty,
            leaderId: leaderId,
            createdAt: DateTime.UtcNow
        );

        // Trưởng CLB mặc định là một thành viên của CLB
        club._memberIds.Add(leaderId);

        return club;
    }

    /// <summary>
    /// Phương thức hành vi: Thêm thành viên mới vào CLB.
    /// </summary>
    public void AddMember(Guid memberId)
    {
        if (memberId == Guid.Empty)
            throw new ArgumentException("Mã thành viên không hợp lệ.", nameof(memberId));

        if (_memberIds.Contains(memberId))
            throw new InvalidOperationException($"Thành viên với mã {memberId} đã tồn tại trong CLB.");

        _memberIds.Add(memberId);
    }

    /// <summary>
    /// Phương thức hành vi: Xóa thành viên khỏi CLB.
    /// Không cho phép xóa Trưởng CLB.
    /// </summary>
    public void RemoveMember(Guid memberId)
    {
        if (memberId == LeaderId)
            throw new InvalidOperationException("Không thể xóa Trưởng CLB khỏi danh sách thành viên.");

        if (!_memberIds.Remove(memberId))
            throw new InvalidOperationException($"Thành viên với mã {memberId} không thuộc CLB này.");
    }

    public override string ToString()
    {
        return $"[Club] Mã: {Code} | Tên: {Name} | Số thành viên: {_memberIds.Count} | Trưởng CLB ID: {LeaderId}";
    }
}
