namespace EduHub.Ch01.Domain;

/// <summary>
/// Đại diện cho thành viên (sinh viên) trong hệ thống EduHub.
/// Áp dụng tính đóng gói cao: private constructor, private setters, static factory method.
/// </summary>
public class Member
{
    public Guid Id { get; private set; }
    public string StudentId { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime JoinedAt { get; private set; }
    public bool IsActive { get; private set; }

    // Constructor private đảm bảo việc khởi tạo phải thông qua Factory method Create
    private Member(Guid id, string studentId, string fullName, string email, DateTime joinedAt, bool isActive)
    {
        Id = id;
        StudentId = studentId;
        FullName = fullName;
        Email = email;
        JoinedAt = joinedAt;
        IsActive = isActive;
    }

    /// <summary>
    /// Factory method static tạo mới một Thành viên với dữ liệu hợp lệ.
    /// </summary>
    public static Member Create(string studentId, string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(studentId))
            throw new ArgumentException("Mã số sinh viên (StudentId) không được để trống.", nameof(studentId));

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Họ và tên (FullName) không được để trống.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Email không hợp lệ.", nameof(email));

        return new Member(
            id: Guid.NewGuid(),
            studentId: studentId.Trim(),
            fullName: fullName.Trim(),
            email: email.Trim().ToLowerInvariant(),
            joinedAt: DateTime.UtcNow,
            isActive: true
        );
    }

    /// <summary>
    /// Phương thức hành vi: Cập nhật thông tin cá nhân.
    /// </summary>
    public void UpdateProfile(string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Họ và tên không được để trống.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Email không hợp lệ.", nameof(email));

        FullName = fullName.Trim();
        Email = email.Trim().ToLowerInvariant();
    }

    /// <summary>
    /// Phương thức hành vi: Vô hiệu hóa tài khoản thành viên.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Phương thức hành vi: Kích hoạt lại tài khoản thành viên.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    public override string ToString()
    {
        return $"[Member] MSSV: {StudentId} | Họ tên: {FullName} | Email: {Email} | Trạng thái: {(IsActive ? "Hoạt động" : "Tạm khóa")}";
    }
}
