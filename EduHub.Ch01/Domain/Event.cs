namespace EduHub.Ch01.Domain;

/// <summary>
/// Đại diện cho Sự kiện (Workshop, Hội thảo, v.v.) do Câu lạc bộ tổ chức.
/// Đảm bảo các quy tắc nghiệp vụ: không cho đăng ký khi hết chỗ hoặc đăng ký trùng;
/// chỉ cho check-in khi phiếu đã Confirmed.
/// </summary>
public class Event
{
    public Guid Id { get; private set; }
    public Guid ClubId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string Location { get; private set; } = string.Empty;
    public int MaxCapacity { get; private set; }

    private readonly List<Registration> _registrations = new();
    public IReadOnlyCollection<Registration> Registrations => _registrations.AsReadOnly();

    private readonly List<CheckIn> _checkIns = new();
    public IReadOnlyCollection<CheckIn> CheckIns => _checkIns.AsReadOnly();

    // Constructor private bảo vệ tính toàn vẹn của sự kiện
    private Event(
        Guid id,
        Guid clubId,
        string title,
        string description,
        DateTime startTime,
        DateTime endTime,
        string location,
        int maxCapacity)
    {
        Id = id;
        ClubId = clubId;
        Title = title;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        Location = location;
        MaxCapacity = maxCapacity;
    }

    /// <summary>
    /// Factory method static tạo sự kiện mới với validation chặt chẽ.
    /// </summary>
    public static Event Create(
        Guid clubId,
        string title,
        string description,
        DateTime startTime,
        DateTime endTime,
        string location,
        int maxCapacity)
    {
        if (clubId == Guid.Empty)
            throw new ArgumentException("Mã CLB (ClubId) không hợp lệ.", nameof(clubId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề sự kiện (Title) không được để trống.", nameof(title));

        if (endTime <= startTime)
            throw new ArgumentException("Thời gian kết thúc (EndTime) phải sau thời gian bắt đầu (StartTime).", nameof(endTime));

        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Địa điểm tổ chức (Location) không được để trống.", nameof(location));

        if (maxCapacity <= 0)
            throw new ArgumentException("Sức chứa tối đa (MaxCapacity) phải lớn hơn 0.", nameof(maxCapacity));

        return new Event(
            id: Guid.NewGuid(),
            clubId: clubId,
            title: title.Trim(),
            description: description?.Trim() ?? string.Empty,
            startTime: startTime,
            endTime: endTime,
            location: location.Trim(),
            maxCapacity: maxCapacity
        );
    }

    /// <summary>
    /// Phương thức hành vi: Tiếp nhận đăng ký tham gia của thành viên.
    /// Quy tắc nghiệp vụ:
    /// 1. Không cho đăng ký trùng lặp (nếu đã có phiếu Pending hoặc Confirmed).
    /// 2. Không cho đăng ký khi sự kiện đã đầy sức chứa (MaxCapacity).
    /// </summary>
    public Registration RegisterMember(Guid memberId)
    {
        if (memberId == Guid.Empty)
            throw new ArgumentException("Mã thành viên không hợp lệ.", nameof(memberId));

        // 1. Kiểm tra đăng ký trùng
        bool isAlreadyRegistered = _registrations.Any(r => 
            r.MemberId == memberId && r.Status != RegistrationStatus.Cancelled);

        if (isAlreadyRegistered)
        {
            throw new InvalidOperationException(
                $"Quy tắc nghiệp vụ: Thành viên {memberId} đã đăng ký sự kiện '{Title}'. Không được phép đăng ký trùng lặp.");
        }

        // 2. Kiểm tra sức chứa
        int currentActiveCount = _registrations.Count(r => r.Status != RegistrationStatus.Cancelled);
        if (currentActiveCount >= MaxCapacity)
        {
            throw new InvalidOperationException(
                $"Quy tắc nghiệp vụ: Sự kiện '{Title}' đã hết chỗ (Sức chứa tối đa: {MaxCapacity}, hiện có: {currentActiveCount} lượt đăng ký).");
        }

        var registration = Registration.Create(Id, memberId);
        _registrations.Add(registration);
        return registration;
    }

    /// <summary>
    /// Phương thức hành vi: Điểm danh (Check-in) cho thành viên tham dự.
    /// Quy tắc nghiệp vụ:
    /// 1. Phiếu đăng ký phải thuộc sự kiện này.
    /// 2. Chỉ phiếu Confirmed mới được CheckIn (thực thi qua CheckIn.Create).
    /// 3. Không được check-in trùng lặp cho cùng một phiếu đăng ký.
    /// </summary>
    public CheckIn CheckInMember(Registration registration, string method = "QR_CODE")
    {
        if (registration == null)
            throw new ArgumentNullException(nameof(registration), "Phiếu đăng ký không được null.");

        if (registration.EventId != Id)
            throw new InvalidOperationException("Phiếu đăng ký không thuộc sự kiện này.");

        if (_checkIns.Any(c => c.RegistrationId == registration.Id))
            throw new InvalidOperationException($"Quy tắc nghiệp vụ: Phiếu đăng ký {registration.Id} đã được điểm danh trước đó.");

        var checkIn = CheckIn.Create(registration, method);
        _checkIns.Add(checkIn);
        return checkIn;
    }

    /// <summary>
    /// Phương thức hành vi: Tạo báo cáo thống kê tham dự sự kiện.
    /// </summary>
    public (int MaxCapacity, int TotalRegistrations, int ConfirmedCount, int TotalCheckIns, double AttendanceRate) GetAttendanceReport()
    {
        int totalReg = _registrations.Count(r => r.Status != RegistrationStatus.Cancelled);
        int confirmed = _registrations.Count(r => r.Status == RegistrationStatus.Confirmed);
        int checkIns = _checkIns.Count;
        double rate = totalReg == 0 ? 0.0 : Math.Round((double)checkIns / totalReg * 100, 2);

        return (MaxCapacity, totalReg, confirmed, checkIns, rate);
    }

    public override string ToString()
    {
        return $"[Event] '{Title}' | CLB: {ClubId.ToString()[..8]} | Sức chứa: {_registrations.Count}/{MaxCapacity} | Bắt đầu: {StartTime:yyyy-MM-dd HH:mm}";
    }
}
