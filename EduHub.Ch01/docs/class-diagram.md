# Sơ Đồ Lớp Miền (Domain Class Diagram) - Hệ Thống EduHub

- **Dự án**: EduHub - Nền tảng quản lý Câu lạc bộ & Sự kiện sinh viên
- **Mã sinh viên**: 0024416184
- **Hoạt động**: HĐ1.3 (CLO1, CLO4, CLO5)
- **Tài liệu**: Sơ đồ lớp Mermaid thể hiện 5 thực thể cốt lõi, thuộc tính, khóa ngoại và quan hệ.

---

## 1. Sơ Đồ Lớp Tổng Quan (Mermaid Class Diagram)

```mermaid
classDiagram
    direction TB

    class RegistrationStatus {
        <<enumeration>>
        Pending
        Confirmed
        Cancelled
    }

    class Member {
        - Guid id
        - string studentId
        - string fullName
        - string email
        - DateTime joinedAt
        - bool isActive
        + Guid Id
        + string StudentId
        + string FullName
        + string Email
        + DateTime JoinedAt
        + bool IsActive
        - Member(Guid id, string studentId, string fullName, string email)
        + static Member Create(string studentId, string fullName, string email)$
        + void UpdateProfile(string fullName, string email)
        + void Deactivate()
        + void Activate()
    }

    class Club {
        - Guid id
        - string code
        - string name
        - string description
        - Guid leaderId
        - List~Guid~ memberIds
        - DateTime createdAt
        + Guid Id
        + string Code
        + string Name
        + string Description
        + Guid LeaderId
        + IReadOnlyCollection~Guid~ MemberIds
        + DateTime CreatedAt
        - Club(Guid id, string code, string name, string description, Guid leaderId)
        + static Club Create(string code, string name, string description, Guid leaderId)$
        + void AddMember(Guid memberId)
        + void RemoveMember(Guid memberId)
    }

    class Event {
        - Guid id
        - Guid clubId
        - string title
        - string description
        - DateTime startTime
        - DateTime endTime
        - string location
        - int maxCapacity
        - List~Registration~ registrations
        - List~CheckIn~ checkIns
        + Guid Id
        + Guid ClubId
        + string Title
        + string Description
        + DateTime StartTime
        + DateTime EndTime
        + string Location
        + int MaxCapacity
        + IReadOnlyCollection~Registration~ Registrations
        + IReadOnlyCollection~CheckIn~ CheckIns
        - Event(Guid id, Guid clubId, string title, string description, DateTime startTime, DateTime endTime, string location, int maxCapacity)
        + static Event Create(Guid clubId, string title, string description, DateTime startTime, DateTime endTime, string location, int maxCapacity)$
        + Registration RegisterMember(Guid memberId)
        + CheckIn CheckInMember(Registration registration, string method)
        + (int TotalRegistrations, int ConfirmedRegistrations, int TotalCheckIns, double AttendanceRate) GetAttendanceReport()
    }

    class Registration {
        - Guid id
        - Guid eventId
        - Guid memberId
        - DateTime registeredAt
        - RegistrationStatus status
        - DateTime? confirmedAt
        - DateTime? cancelledAt
        + Guid Id
        + Guid EventId
        + Guid MemberId
        + DateTime RegisteredAt
        + RegistrationStatus Status
        + DateTime? ConfirmedAt
        + DateTime? CancelledAt
        - Registration(Guid id, Guid eventId, Guid memberId)
        + static Registration Create(Guid eventId, Guid memberId)$
        + void Confirm()
        + void Cancel()
    }

    class CheckIn {
        - Guid id
        - Guid registrationId
        - Guid eventId
        - Guid memberId
        - DateTime checkInTime
        - string method
        + Guid Id
        + Guid RegistrationId
        + Guid EventId
        + Guid MemberId
        + DateTime CheckInTime
        + string Method
        - CheckIn(Guid id, Guid registrationId, Guid eventId, Guid memberId, string method)
        + static CheckIn Create(Registration registration, string method)$
        + bool IsLate(DateTime eventStartTime)
    }

    %% Quan hệ giữa các thực thể và Bội số quan hệ (Multiplicity)
    Member "1" <-- "0..*" Club : "LeaderId [FK]"
    Club "1" o-- "0..*" Member : "chứa các thành viên (MemberIds)"
    Club "1" *-- "0..*" Event : "quản lý / tổ chức (ClubId [FK])"
    
    Event "1" *-- "0..*" Registration : "tiếp nhận đăng ký (EventId [FK])"
    Member "1" <-- "0..*" Registration : "đăng ký bởi (MemberId [FK])"
    
    Registration "1" <-- "0..1" CheckIn : "xác thực điểm danh (RegistrationId [FK])"
    Event "1" *-- "0..*" CheckIn : "ghi nhận điểm danh (EventId [FK])"
    Member "1" <-- "0..*" CheckIn : "người điểm danh (MemberId [FK])"
    
    Registration ..> RegistrationStatus : "sử dụng trạng thái"
```

---

## 2. Chi Tiết Thực Thể và Khóa Ngoại (Foreign Keys)

| Thực thể (Class) | Khóa chính (PK) | Khóa ngoại (FK) | Tham chiếu đến (References) | Mô tả quan hệ |
| :--- | :--- | :--- | :--- | :--- |
| **Member** | `Id (Guid)` | Không có | - | Đại diện cho sinh viên trong hệ thống EduHub. |
| **Club** | `Id (Guid)` | `LeaderId (Guid)` | `Member.Id` | Một CLB được dẫn dắt bởi đúng 1 Trưởng CLB (`1 - 1`). CLB chứa danh sách thành viên (`1 - 0..*`). |
| **Event** | `Id (Guid)` | `ClubId (Guid)` | `Club.Id` | Một sự kiện thuộc về duy nhất 1 CLB (`1 - 0..*`). |
| **Registration** | `Id (Guid)` | `EventId (Guid)`<br>`MemberId (Guid)` | `Event.Id`<br>`Member.Id` | Phiếu đăng ký kết nối 1 Member với 1 Event (`1 - 0..*`). Trạng thái được kiểm soát bởi `RegistrationStatus`. |
| **CheckIn** | `Id (Guid)` | `RegistrationId (Guid)`<br>`EventId (Guid)`<br>`MemberId (Guid)` | `Registration.Id`<br>`Event.Id`<br>`Member.Id` | Bản ghi điểm danh kiểm tra và liên kết với phiếu đăng ký hợp lệ (`1 - 0..1`). |

---

## 3. Bội Số Quan Hệ (Multiplicity) và Ý Nghĩa Nghiệp Vụ

1. **Club - Member (Quan hệ Trưởng CLB và Thành viên)**:
   - Một `Member` có thể là Trưởng CLB của `0` hoặc nhiều (`*`) CLB.
   - Một `Club` phải có đúng `1` Trưởng CLB (`LeaderId`).
   - Một `Club` kết tập (`Aggregation: o--`) nhiều `Member` qua danh sách `MemberIds`.

2. **Club - Event (Quan hệ Tổ chức)**:
   - Một `Club` có thể tổ chức từ `0` đến nhiều (`0..*`) `Event`.
   - Mỗi `Event` bắt buộc trực thuộc duy nhất `1` `Club` (`Composition: *--`).

3. **Event - Registration (Quan hệ Đăng ký)**:
   - Một `Event` có thể nhận từ `0` đến `MaxCapacity` phiếu `Registration`.
   - Mỗi `Registration` thuộc về duy nhất `1` `Event`.
   - Mỗi `Member` có thể tạo nhiều `Registration` nhưng không được đăng ký trùng lặp trong cùng một `Event`.

4. **Registration - CheckIn (Quan hệ Điểm danh)**:
   - Mỗi `Registration` hợp lệ (`Confirmed`) chỉ sinh ra tối đa `1` bản ghi `CheckIn` tương ứng (`0..1`).
   - Mỗi bản ghi `CheckIn` bắt buộc phải tham chiếu đến đúng `1` phiếu `Registration` đã xác nhận.

5. **Quy tắc bất biến (Invariants & Encapsulation)**:
   - Toàn bộ thuộc tính có private setter, bảo đảm không bị can thiệp trái phép ngoài Domain boundary.
   - Khởi tạo bắt buộc qua Factory Method static `Create(...)` có validation.
   - Phiếu `Cancelled` không được phép `Confirm`.
   - Chỉ phiếu `Confirmed` mới được phép `CheckIn`.
   - Không cho phép đăng ký khi đã đầy sức chứa (`MaxCapacity`) hoặc đăng ký trùng.
