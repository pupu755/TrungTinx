# Ch01 — Phân tích yêu cầu và Domain Modeling EduHub

Bài tập chương 1 của IN4211, chấm theo HĐ1.3 (bài tập chương). Liên quan CLO1, CLO4, CLO5.

Sinh viên thực hiện: MSSV 0024416184.

## Nội dung bài làm

Domain EduHub gồm 5 entity: `Member`, `Club`, `Event`, `Registration`, `CheckIn`. Bài làm gồm hai phần tài liệu và phần code C# (.NET 8):

1. `EduHub.Ch01/docs/user-stories.md`: các User Story kèm tiêu chí nghiệm thu, đủ 3 vai trò (Thành viên, Trưởng CLB, Quản trị viên).
2. `EduHub.Ch01/docs/class-diagram.md`: sơ đồ lớp sơ bộ bằng Mermaid, có thuộc tính, khoá ngoại và bội số quan hệ.
3. `EduHub.Ch01/Domain/`: 5 class với setter private, constructor private, factory `Create(...)` và các method hành vi.
4. `EduHub.Ch01/Program.cs`: chương trình demo 8 bước.

## Cấu trúc thư mục

```
.gitignore
PR_DESCRIPTION.md
README.md
EduHub.Ch01/
  EduHub.Ch01.csproj
  Program.cs
  Domain/
    Member.cs
    Club.cs
    Event.cs
    Registration.cs
    CheckIn.cs
  docs/
    user-stories.md
    class-diagram.md
```

## Chạy và build

Yêu cầu: .NET 8 SDK.

```bash
cd EduHub.Ch01
dotnet build
dotnet run
```

Chương trình chạy 8 bước: tạo CLB, thêm thành viên, tạo sự kiện, đăng ký, xác nhận, check-in, báo cáo tham dự, rồi kiểm tra quy tắc nghiệp vụ. Kết quả in ra thông tin thành viên, CLB, sự kiện (số đăng ký / sức chứa), check-in và báo cáo tham dự.

## Quy tắc nghiệp vụ chính

- Sự kiện không cho đăng ký khi đã hết chỗ hoặc khi thành viên đã đăng ký trùng.
- Phiếu đăng ký có ba trạng thái: `Pending`, `Confirmed`, `Cancelled`.
- Phiếu `Cancelled` không thể xác nhận lại.
- Chỉ phiếu `Confirmed` mới được check-in.

## Nộp bài

Nhánh nộp bài: `ch01/0024416184`. Pull Request về `main`, mô tả đủ what, why, how to test (xem `PR_DESCRIPTION.md`). Các commit theo Conventional Commits.
