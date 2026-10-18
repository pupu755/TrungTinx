namespace EduHub.Ch01.Domain;

/// <summary>
/// Thực thể Bản ghi điểm danh (Check-in) tại sự kiện.
/// Quy tắc nghiệp vụ: Chỉ phiếu đăng ký ở trạng thái Confirmed mới được phép Check-in.
/// </summary>
public class CheckIn
{
    public Guid Id { get; private set; }
    public Guid RegistrationId { get; private set; }
    public Guid EventId { get; private set; }
    public Guid MemberId { get; private set; }
    public DateTime CheckInTime { get; private set; }
    public string Method { get; private set; } = string.Empty;

    // Constructor private bảo vệ dữ liệu điểm danh
    private CheckIn(Guid id, Guid registrationId, Guid eventId, Guid memberId, DateTime checkInTime, string method)
    {
        Id = id;
        RegistrationId = registrationId;
        EventId = eventId;
        MemberId = memberId;
        CheckInTime = checkInTime;
        Method = method;
    }

    /// <summary>
    /// Factory method static tạo bản ghi check-in.
    /// Kiểm tra chặt chẽ: Chỉ phiếu Confirmed mới được phép CheckIn.
    /// </summary>
    public static CheckIn Create(Registration registration, string method = "QR_CODE")
    {
        if (registration == null)
            throw new ArgumentNullException(nameof(registration), "Phiếu đăng ký không được null.");

        if (registration.Status != RegistrationStatus.Confirmed)
            throw new InvalidOperationException($"Quy tắc nghiệp vụ: Chỉ phiếu đăng ký ở trạng thái Confirmed mới được Check-in. Trạng thái hiện tại: {registration.Status}.");

        if (string.IsNullOrWhiteSpace(method))
            throw new ArgumentException("Phương thức điểm danh không được để trống.", nameof(method));

        return new CheckIn(
            id: Guid.NewGuid(),
            registrationId: registration.Id,
            eventId: registration.EventId,
            memberId: registration.MemberId,
            checkInTime: DateTime.UtcNow,
            method: method.Trim()
        );
    }

    /// <summary>
    /// Phương thức hành vi: Kiểm tra xem thành viên check-in có bị muộn so với giờ bắt đầu sự kiện không.
    /// </summary>
    public bool IsLate(DateTime eventStartTime)
    {
        return CheckInTime > eventStartTime;
    }

    public override string ToString()
    {
        return $"[CheckIn] Mã: {Id.ToString()[..8]} | Phiếu: {RegistrationId.ToString()[..8]} | Phương thức: {Method} | Thời gian: {CheckInTime:yyyy-MM-dd HH:mm:ss}";
    }
}
