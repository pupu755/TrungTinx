# EduHub - Hệ Thống Quản Lý Câu Lạc Bộ & Sự Kiện Sinh Viên

Bài tập Ch01: Phân tích yêu cầu và Domain Modeling EduHub (HĐ1.3; CLO1, CLO4, CLO5).

- **Sinh viên thực hiện**: Bùi Trung Tín
- **MSSV**: 0024416184
- **Lớp / Khóa**: K28 / CNTT
- **Nhánh nộp bài**: `ch01/0024416184`
- **Nền tảng**: .NET 8 (C# 12)

---

## 1. Giới thiệu dự án
EduHub là mô hình phần mềm quản lý hoạt động ngoại khóa dành cho các câu lạc bộ (CLB) và sự kiện sinh viên trong trường đại học. Dự án bao gồm các phân tích yêu cầu (User Stories), sơ đồ miền (Domain Class Diagram) và mã nguồn Domain Model áp dụng các nguyên lý hướng đối tượng chặt chẽ (Encapsulation, Factory Method, Domain Behaviors, Business Rule Enforcement).

---

## 2. Cấu trúc thư mục
```
EduHub.Ch01/
  ├── EduHub.Ch01.csproj          # Project file cấu hình .NET 8, Nullable & ImplicitUsings
  ├── Program.cs                  # Kịch bản chạy demo 8 bước thực thi và kiểm thử nghiệp vụ
  ├── Domain/                     # Các thực thể nghiệp vụ (Domain Entities & Enums)
  │   ├── Member.cs               # Thực thể Thành viên
  │   ├── Club.cs                 # Thực thể Câu lạc bộ
  │   ├── Event.cs                # Thực thể Sự kiện
  │   ├── Registration.cs         # Thực thể Đăng ký tham gia (Enum RegistrationStatus)
  │   └── CheckIn.cs              # Thực thể Check-in điểm danh
  ├── docs/                       # Tài liệu phân tích và thiết kế
  │   ├── user-stories.md         # Danh sách User Story và Tiêu chí nghiệm thu (Acceptance Criteria)
  │   └── class-diagram.md        # Sơ đồ lớp Mermaid thể hiện thuộc tính, khóa ngoại & quan hệ
  └── README.md                   # Hướng dẫn chạy chương trình & tài liệu kỹ thuật
.gitignore                        # Quy tắc bỏ qua file rác biên dịch (.vs, bin, obj, ...)
```

---

## 3. Hướng dẫn biên dịch và chạy chương trình

### Yêu cầu môi trường
- Đã cài đặt [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
- Git 2.x trở lên.

### Các bước thực thi
1. Mở cửa sổ dòng lệnh (Terminal / PowerShell / CMD) tại thư mục gốc của dự án.
2. Di chuyển vào thư mục dự án `EduHub.Ch01`:
   ```bash
   cd EduHub.Ch01
   ```
3. Khôi phục gói và biên dịch dự án:
   ```bash
   dotnet build
   ```
4. Chạy ứng dụng console:
   ```bash
   dotnet run
   ```

---

## 4. Kịch bản chạy mẫu (8 bước)
Chương trình `Program.cs` sẽ tự động thực hiện 8 bước minh họa:
1. **Bước 1**: Khởi tạo Câu lạc bộ (CLB Lập trình sinh viên - CodeCraft Club).
2. **Bước 2**: Thêm 3 thành viên vào câu lạc bộ (bao gồm MSSV: `0024416184`).
3. **Bước 3**: Tạo sự kiện ngoại khóa (Workshop kiến trúc Domain-Driven Design).
4. **Bước 4**: Thành viên thực hiện đăng ký tham gia sự kiện.
5. **Bước 5**: Trưởng CLB / Ban tổ chức xác nhận phiếu đăng ký.
6. **Bước 6**: Điểm danh (Check-in) người tham dự bằng mã QR / thủ công.
7. **Bước 7**: Xuất báo cáo tổng kết tham dự (Sức chứa, lượt đăng ký, số check-in, tỷ lệ tham gia).
8. **Bước 8**: Kiểm tra quy tắc nghiệp vụ:
   - Phiếu `Cancelled` không được phép `Confirm`.
   - Ngăn chặn đăng ký trùng lặp hoặc đăng ký khi sự kiện đã đầy sức chứa.
