# Pull Request: feat(ch01): EduHub Domain Modeling - 0024416184

- **MSSV**: 0024416184
- **Họ và tên**: Bùi Trung Tín
- **Bài tập**: Ch01 - Phân tích yêu cầu và Domain Modeling EduHub (HĐ1.3; CLO1, CLO4, CLO5)
- **Nhánh nộp**: `ch01/0024416184` -> `main`

---

## 1. What (Đã làm gì)
- **Phân tích yêu cầu (User Stories)**:
  - Tạo tài liệu `docs/user-stories.md` chứa 7 User Stories chi tiết với ít nhất 2 Acceptance Criteria (theo chuẩn Given-When-Then), đáp ứng đầy đủ 3 vai trò:
    - *Thành viên (Member)*: Đăng ký tham gia CLB (US-01), Đăng ký sự kiện (US-02), Điểm danh Check-in (US-03).
    - *Trưởng CLB (Club Leader)*: Khởi tạo sự kiện mới (US-04), Duyệt và xác nhận phiếu đăng ký (US-05).
    - *Quản trị viên (Administrator)*: Phê duyệt thành lập CLB & gán Trưởng CLB (US-06), Xem báo cáo thống kê tham dự (US-07).
- **Mô hình hóa lớp miền (Domain Class Diagram)**:
  - Tạo tài liệu `docs/class-diagram.md` với sơ đồ lớp chuẩn Mermaid thể hiện rõ 5 lớp thực thể (`Member`, `Club`, `Event`, `Registration`, `CheckIn`) và 1 enum (`RegistrationStatus`).
  - Thể hiện đầy đủ thuộc tính, kiểu dữ liệu, phạm vi truy cập, các khóa ngoại (`[FK]`) và bội số quan hệ (Multiplicity: `1 - 1`, `1 - 0..*`, `1 - 0..1`).
- **Xây dựng Domain Model (Rich Domain Model)**:
  - Hoàn thiện 5 class trong thư mục `Domain/`:
    - Áp dụng nguyên lý Encapsulation triệt để: toàn bộ thuộc tính có private setter, toàn bộ constructor là private.
    - Cung cấp Factory Method static `Create(...)` có kiểm tra tính hợp lệ dữ liệu đầu vào.
    - Cung cấp các phương thức hành vi thực tế cho từng thực thể.
    - Quản lý trạng thái phiếu qua enum `RegistrationStatus` (`Pending`, `Confirmed`, `Cancelled`).
    - Thực thi các quy tắc nghiệp vụ bất biến:
      1. Phiếu đã huỷ (`Cancelled`) KHÔNG THỂ xác nhận (`Confirm`).
      2. Chỉ phiếu ở trạng thái `Confirmed` mới được phép điểm danh (`CheckIn`).
      3. Sự kiện (`Event`) ngăn chặn đăng ký khi hết chỗ (`MaxCapacity`) hoặc đăng ký trùng lặp.
- **Kịch bản Demo trong `Program.cs`**:
  - Triển khai kịch bản 8 bước tuần tự rõ ràng:
    - Bước 1: Khởi tạo CLB CodeCraft EduHub.
    - Bước 2: Thêm 3 thành viên vào CLB (gồm MSSV 0024416184 Bùi Trung Tín).
    - Bước 3: Tạo sự kiện ngoại khóa (sức chứa giới hạn = 2).
    - Bước 4: Thành viên thực hiện đăng ký tham gia sự kiện.
    - Bước 5: Ban tổ chức xác nhận phiếu đăng ký (`Confirm`).
    - Bước 6: Check-in điểm danh thành viên (hỗ trợ QR Code và thủ công).
    - Bước 7: Xuất bảng báo cáo tổng kết tham dự sự kiện.
    - Bước 8: Kiểm thử tự động các quy tắc nghiệp vụ (đặc biệt: xác nhận phiếu Cancelled không thể Confirm, ngăn chặn quá sức chứa, trùng lặp và check-in sai trạng thái).

---

## 2. Why (Vì sao)
- Nhằm hoàn thành mục tiêu học tập (CLO1, CLO4, CLO5) của môn học:
  - **CLO1**: Phân tích và chuyển giao yêu cầu bài toán thực tế thành các User Stories và Acceptance Criteria khả thi.
  - **CLO4**: Thiết kế mô hình lớp miền (Domain Model) có cấu trúc chuẩn, phân định rõ ranh giới trách nhiệm, thuộc tính, khóa ngoại và quan hệ.
  - **CLO5**: Lập trình hướng đối tượng chuyên sâu trong nền tảng .NET 8 / C# hiện đại, áp dụng mô hình Rich Domain Model thay vì Anemic Domain Model, bảo vệ bất biến dữ liệu ngay tại tầng lõi nghiệp vụ.

---

## 3. How to test (Cách kiểm thử & Kết quả mong đợi)

### Các bước thực hiện:
```bash
# 1. Di chuyển vào thư mục dự án
cd EduHub.Ch01

# 2. Biên dịch mã nguồn (.NET 8 SDK)
dotnet build

# 3. Chạy chương trình console
dotnet run
```

### Kết quả mong đợi:
- `dotnet build`: Build thành công, **0 Warning(s), 0 Error(s)**.
- `dotnet run`:
  - In tiêu đề sinh viên và đồ họa ASCII đẹp mắt.
  - Lần lượt in chi tiết kết quả từng bước từ Bước 1 đến Bước 7 với đầy đủ thông tin thành viên, CLB, sự kiện, thời gian UTC, và bảng báo cáo tỷ lệ tham dự.
  - Tại **Bước 8**: In dòng xác nhận quy tắc nghiệp vụ thành công:
    ```
    👉 Kiểm tra Quy tắc 1: Phiếu đã bị huỷ (Cancelled) KHÔNG ĐƯỢC phép Confirm:
        [✓ THÀNH CÔNG] Đã kích hoạt bảo vệ nghiệp vụ!
        [Xác nhận] Phiếu Cancelled không thể Confirm.
        [Thông điệp ngoại lệ]: "Quy tắc nghiệp vụ: Phiếu đăng ký đã bị huỷ (Cancelled) không thể xác nhận (Confirm)."
    ```
  - Các quy tắc bổ sung về sức chứa tối đa và đăng ký trùng lặp đều bắt được ngoại lệ hợp lệ.

---

## Giảng viên / Người phản biện (Reviewer)
- Giảng viên hướng dẫn: @____________________
- Đánh giá / Nhận xét: 
