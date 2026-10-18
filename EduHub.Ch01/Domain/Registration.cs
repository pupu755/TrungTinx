namespace EduHub.Ch01.Domain;

/// <summary>
/// Trạng thái của phiếu đăng ký tham dự sự kiện.
/// </summary>
public enum RegistrationStatus
{
    Pending,
    Confirmed,
    Cancelled
}

/// <summary>
/// Thực thể Phiếu đăng ký sự kiện của thành viên.
/// Quy tắc nghiệp vụ: Phiếu Cancelled KHÔNG được phép chuyển sang Confirmed.
/// </summary>
public class Registration
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public Guid MemberId { get; private set; }
    public DateTime RegisteredAt { get; private set; }
    public RegistrationStatus Status { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    // Constructor private bảo vệ trạng thái nội bộ
    private Registration(Guid id, Guid eventId, Guid memberId, DateTime registeredAt, RegistrationStatus status)
    {
        Id = id;
        EventId = eventId;
        MemberId = memberId;
        RegisteredAt = registeredAt;
        Status = status;
    }

    /// <summary>
    /// Factory method static khởi tạo phiếu đăng ký mới (mặc định trạng thái Pending).
    /// </summary>
    public static Registration Create(Guid eventId, Guid memberId)
    {
        if (eventId == Guid.Empty)
            throw new ArgumentException("Mã sự kiện (EventId) không hợp lệ.", nameof(eventId));

        if (memberId == Guid.Empty)
            throw new ArgumentException("Mã thành viên (MemberId) không hợp lệ.", nameof(memberId));

        return new Registration(
            id: Guid.NewGuid(),
            eventId: eventId,
            memberId: memberId,
            registeredAt: DateTime.UtcNow,
            status: RegistrationStatus.Pending
        );
    }

    /// <summary>
    /// Phương thức hành vi: Xác nhận phiếu đăng ký.
    /// Quy tắc nghiệp vụ: Phiếu đã bị huỷ (Cancelled) KHÔNG thể Confirm.
    /// </summary>
    public void Confirm()
    {
        if (Status == RegistrationStatus.Cancelled)
            throw new InvalidOperationException("Quy tắc nghiệp vụ: Phiếu đăng ký đã bị huỷ (Cancelled) không thể xác nhận (Confirm).");

        Status = RegistrationStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Phương thức hành vi: Huỷ phiếu đăng ký.
    /// </summary>
    public void Cancel()
    {
        Status = RegistrationStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"[Registration] Mã: {Id.ToString()[..8]} | EventId: {EventId.ToString()[..8]} | MemberId: {MemberId.ToString()[..8]} | Trạng thái: {Status} | Ngày đăng ký: {RegisteredAt:yyyy-MM-dd HH:mm}";
    }
}
