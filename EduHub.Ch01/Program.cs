using System;
using System.Text;
using EduHub.Ch01.Domain;

namespace EduHub.Ch01;

internal class Program
{
    private static void Main(string[] args)
    {
        // Thiết lập bảng mã UTF-8 để hiển thị tiếng Việt trên Console
        Console.OutputEncoding = Encoding.UTF8;

        PrintHeader();

        try
        {
            // =========================================================================
            // BƯỚC 1: TẠO CÂU LẠC BỘ (CLB)
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("【BƯỚC 1】KHỞI TẠO CÂU LẠC BỘ (CLUB)");
            Console.WriteLine("--------------------------------------------------------------------------------");

            // Tạo Trưởng CLB (Sinh viên MSSV: 0024416184)
            var leader = Member.Create(
                studentId: "0024416184",
                fullName: "Bùi Trung Tín",
                email: "btn813@gmail.com"
            );

            // Khởi tạo CLB thông qua Factory Method Club.Create
            var club = Club.Create(
                code: "CODECRAFT",
                name: "CLB Lập Trình CodeCraft EduHub",
                description: "Nơi chia sẻ kiến thức C#, .NET, Domain-Driven Design và phần mềm.",
                leaderId: leader.Id
            );

            Console.WriteLine($"[✓] Khởi tạo thành công CLB:");
            Console.WriteLine($"    - Mã CLB        : {club.Code}");
            Console.WriteLine($"    - Tên CLB       : {club.Name}");
            Console.WriteLine($"    - Mô tả         : {club.Description}");
            Console.WriteLine($"    - Trưởng CLB ID : {club.LeaderId} ({leader.FullName} - MSSV: {leader.StudentId})");
            Console.WriteLine($"    - Ngày thành lập: {club.CreatedAt:yyyy-MM-dd HH:mm:ss} UTC\n");

            // =========================================================================
            // BƯỚC 2: THÊM 3 THÀNH VIÊN VÀO CÂU LẠC BỘ
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("【BƯỚC 2】THÊM 3 THÀNH VIÊN VÀO CÂU LẠC BỘ");
            Console.WriteLine("--------------------------------------------------------------------------------");

            // Thành viên 1: Trưởng CLB (đã được tự động thêm trong Factory Method)
            // Thành viên 2 & 3:
            var member2 = Member.Create(
                studentId: "0024416185",
                fullName: "Nguyễn Văn An",
                email: "an.nguyen@eduhub.edu.vn"
            );

            var member3 = Member.Create(
                studentId: "0024416186",
                fullName: "Trần Thị Bình",
                email: "binh.tran@eduhub.edu.vn"
            );

            // Thêm các thành viên vào CLB
            club.AddMember(member2.Id);
            club.AddMember(member3.Id);

            var memberList = new[] { leader, member2, member3 };

            Console.WriteLine($"[✓] Đã thêm đầy đủ 3 thành viên vào CLB '{club.Name}':");
            for (int i = 0; i < memberList.Length; i++)
            {
                var m = memberList[i];
                string role = m.Id == club.LeaderId ? "Trưởng CLB" : "Thành viên";
                Console.WriteLine($"    {i + 1}. [{role}] MSSV: {m.StudentId} | Họ tên: {m.FullName,-15} | Email: {m.Email,-26} | Trạng thái: {(m.IsActive ? "Hoạt động" : "Khóa")}");
            }
            Console.WriteLine($"    => Tổng số thành viên trong CLB hiện tại: {club.MemberIds.Count}\n");

            // =========================================================================
            // BƯỚC 3: TẠO SỰ KIỆN DO CLB TỔ CHỨC
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("【BƯỚC 3】TẠO SỰ KIỆN NGOẠI KHÓA (EVENT)");
            Console.WriteLine("--------------------------------------------------------------------------------");

            var eventStartTime = DateTime.UtcNow.AddDays(7);
            var eventEndTime = eventStartTime.AddHours(3);

            // Tạo sự kiện với giới hạn sức chứa = 2 để kiểm thử ràng buộc sức chứa
            var eduEvent = Event.Create(
                clubId: club.Id,
                title: "Workshop: Domain Modeling & C# 12 Architecture",
                description: "Thực hành thiết kế hướng đối tượng, Rich Domain Model và kiến trúc Clean Architecture.",
                startTime: eventStartTime,
                endTime: eventEndTime,
                location: "Hội trường Innovation Lab A1, EduHub",
                maxCapacity: 2
            );

            Console.WriteLine($"[✓] Khởi tạo thành công Sự kiện:");
            Console.WriteLine($"    - Tiêu đề      : {eduEvent.Title}");
            Console.WriteLine($"    - Đơn vị tổ chức: {club.Name} (Mã: {club.Code})");
            Console.WriteLine($"    - Địa điểm     : {eduEvent.Location}");
            Console.WriteLine($"    - Thời gian    : {eduEvent.StartTime:yyyy-MM-dd HH:mm} -> {eduEvent.EndTime:HH:mm} UTC");
            Console.WriteLine($"    - Sức chứa     : {eduEvent.MaxCapacity} người tham dự\n");

            // =========================================================================
            // BƯỚC 4: ĐĂNG KÝ THAM GIA SỰ KIỆN
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("【BƯỚC 4】THÀNH VIÊN ĐĂNG KÝ THAM GIA SỰ KIỆN");
            Console.WriteLine("--------------------------------------------------------------------------------");

            // Sinh viên 1 (Bùi Trung Tín) và Sinh viên 2 (Nguyễn Văn An) đăng ký
            var reg1 = eduEvent.RegisterMember(leader.Id);
            var reg2 = eduEvent.RegisterMember(member2.Id);

            Console.WriteLine($"[✓] Phiếu đăng ký 1: Thành viên '{leader.FullName}'");
            Console.WriteLine($"    - Mã phiếu : {reg1.Id}");
            Console.WriteLine($"    - Trạng thái: {reg1.Status} (Chờ ban tổ chức duyệt)");
            Console.WriteLine($"    - Ngày tạo  : {reg1.RegisteredAt:yyyy-MM-dd HH:mm:ss} UTC");

            Console.WriteLine($"[✓] Phiếu đăng ký 2: Thành viên '{member2.FullName}'");
            Console.WriteLine($"    - Mã phiếu : {reg2.Id}");
            Console.WriteLine($"    - Trạng thái: {reg2.Status} (Chờ ban tổ chức duyệt)");
            Console.WriteLine($"    - Ngày tạo  : {reg2.RegisteredAt:yyyy-MM-dd HH:mm:ss} UTC\n");

            // =========================================================================
            // BƯỚC 5: XÁC NHẬN PHIẾU ĐĂNG KÝ (CONFIRM)
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("【BƯỚC 5】BAN TỔ CHỨC XÁC NHẬN PHIẾU ĐĂNG KÝ (CONFIRM)");
            Console.WriteLine("--------------------------------------------------------------------------------");

            reg1.Confirm();
            reg2.Confirm();

            Console.WriteLine($"[✓] Đã duyệt phiếu 1 ({leader.FullName})  -> Trạng thái mới: {reg1.Status} (Xác nhận lúc: {reg1.ConfirmedAt:yyyy-MM-dd HH:mm:ss} UTC)");
            Console.WriteLine($"[✓] Đã duyệt phiếu 2 ({member2.FullName}) -> Trạng thái mới: {reg2.Status} (Xác nhận lúc: {reg2.ConfirmedAt:yyyy-MM-dd HH:mm:ss} UTC)\n");

            // =========================================================================
            // BƯỚC 6: ĐIỂM DANH CHECK-IN TẠI SỰ KIỆN
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("【BƯỚC 6】ĐIỂM DANH CHECK-IN THÀNH VIÊN THAM DỰ");
            Console.WriteLine("--------------------------------------------------------------------------------");

            var checkIn1 = eduEvent.CheckInMember(reg1, method: "QR_CODE");
            var checkIn2 = eduEvent.CheckInMember(reg2, method: "MANUAL_STAFF");

            Console.WriteLine($"[✓] Check-in thành công cho thành viên 1 ({leader.FullName}):");
            Console.WriteLine($"    - Mã CheckIn    : {checkIn1.Id}");
            Console.WriteLine($"    - Phương thức   : {checkIn1.Method}");
            Console.WriteLine($"    - Thời gian     : {checkIn1.CheckInTime:yyyy-MM-dd HH:mm:ss} UTC");

            Console.WriteLine($"[✓] Check-in thành công cho thành viên 2 ({member2.FullName}):");
            Console.WriteLine($"    - Mã CheckIn    : {checkIn2.Id}");
            Console.WriteLine($"    - Phương thức   : {checkIn2.Method}");
            Console.WriteLine($"    - Thời gian     : {checkIn2.CheckInTime:yyyy-MM-dd HH:mm:ss} UTC\n");

            // =========================================================================
            // BƯỚC 7: BÁO CÁO THAM DỰ SỰ KIỆN
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("【BƯỚC 7】BÁO CÁO TỔNG KẾT THAM DỰ SỰ KIỆN");
            Console.WriteLine("--------------------------------------------------------------------------------");

            var report = eduEvent.GetAttendanceReport();

            Console.WriteLine($"[📊] KẾT QUẢ THỐNG KÊ THAM DỰ:");
            Console.WriteLine($"    ┌──────────────────────────────────┬─────────────────┐");
            Console.WriteLine($"    │ Chỉ số thống kê                  │ Giá trị         │");
            Console.WriteLine($"    ├──────────────────────────────────┼─────────────────┤");
            Console.WriteLine($"    │ Sức chứa tối đa (Max Capacity)   │ {report.MaxCapacity,-15} │");
            Console.WriteLine($"    │ Tổng số đăng ký hợp lệ           │ {report.TotalRegistrations,-15} │");
            Console.WriteLine($"    │ Số phiếu đã duyệt (Confirmed)    │ {report.ConfirmedCount,-15} │");
            Console.WriteLine($"    │ Số lượt đã check-in thực tế      │ {report.TotalCheckIns,-15} │");
            Console.WriteLine($"    │ Tỷ lệ tham dự (Check-in/Đăng ký) │ {report.AttendanceRate,6:F1}%         │");
            Console.WriteLine($"    └──────────────────────────────────┴─────────────────┘\n");

            // =========================================================================
            // BƯỚC 8: KIỂM TRA CÁC QUY TẮC NGHIỆP VỤ (BUSINESS RULES)
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("【BƯỚC 8】KIỂM TRA CÁC QUY TẮC NGHIỆP VỤ BẮT BUỘC");
            Console.WriteLine("--------------------------------------------------------------------------------");

            // Quy tắc bắt buộc 1: Phiếu Cancelled KHÔNG THỂ Confirm
            Console.WriteLine("👉 Kiểm tra Quy tắc 1: Phiếu đã bị huỷ (Cancelled) KHÔNG ĐƯỢC phép Confirm:");
            var testReg = Registration.Create(eduEvent.Id, member3.Id);
            testReg.Cancel();
            Console.WriteLine($"    - Trạng thái phiếu thử nghiệm sau khi Cancel(): {testReg.Status}");

            try
            {
                testReg.Confirm();
                Console.WriteLine("    [!] THẤT BẠI: Lỗi vi phạm! Phiếu Cancelled lại được phép Confirm!");
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"    [✓ THÀNH CÔNG] Đã kích hoạt bảo vệ nghiệp vụ!");
                Console.WriteLine($"    [Xác nhận] Phiếu Cancelled không thể Confirm.");
                Console.WriteLine($"    [Thông điệp ngoại lệ]: \"{ex.Message}\"");
                Console.ResetColor();
            }

            // Quy tắc bắt buộc 2: Sự kiện hết chỗ không cho phép đăng ký
            Console.WriteLine("\n👉 Kiểm tra Quy tắc 2: Không cho phép đăng ký khi sự kiện hết chỗ (MaxCapacity = 2):");
            try
            {
                eduEvent.RegisterMember(member3.Id);
                Console.WriteLine("    [!] THẤT BẠI: Hệ thống vẫn cho đăng ký vượt quá sức chứa!");
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"    [✓ THÀNH CÔNG] Đã ngăn chặn đăng ký khi hết chỗ!");
                Console.WriteLine($"    [Thông điệp ngoại lệ]: \"{ex.Message}\"");
                Console.ResetColor();
            }

            // Quy tắc bắt buộc 3: Không cho phép đăng ký trùng lặp
            Console.WriteLine("\n👉 Kiểm tra Quy tắc 3: Không cho phép cùng 1 thành viên đăng ký trùng lặp:");
            try
            {
                eduEvent.RegisterMember(leader.Id);
                Console.WriteLine("    [!] THẤT BẠI: Cho phép đăng ký trùng lặp!");
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"    [✓ THÀNH CÔNG] Đã ngăn chặn đăng ký trùng lặp!");
                Console.WriteLine($"    [Thông điệp ngoại lệ]: \"{ex.Message}\"");
                Console.ResetColor();
            }

            // Quy tắc bắt buộc 4: Chỉ phiếu Confirmed mới được Check-in
            Console.WriteLine("\n👉 Kiểm tra Quy tắc 4: Chỉ phiếu trạng thái Confirmed mới được phép Check-in:");
            var pendingReg = Registration.Create(eduEvent.Id, Guid.NewGuid());
            try
            {
                CheckIn.Create(pendingReg);
                Console.WriteLine("    [!] THẤT BẠI: Cho phép check-in phiếu Pending!");
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"    [✓ THÀNH CÔNG] Đã từ chối check-in phiếu chưa được duyệt Confirmed!");
                Console.WriteLine($"    [Thông điệp ngoại lệ]: \"{ex.Message}\"");
                Console.ResetColor();
            }

            Console.WriteLine("\n================================================================================");
            Console.WriteLine("🎉 HOÀN THÀNH TOÀN BỘ 8 BƯỚC DEMO VÀ KIỂM TRA ĐẠT TẤT CẢ QUY TẮC NGHIỆP VỤ!");
            Console.WriteLine("================================================================================");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[X] Đã xảy ra lỗi ngoài ý muốn: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Console.ResetColor();
        }
    }

    private static void PrintHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║               EDUHUB - DOMAIN MODELING & DEMO KIẾN TRÚC .NET 8               ║");
        Console.WriteLine("║                 HĐ1.3: Phân tích yêu cầu và Domain Modeling                  ║");
        Console.WriteLine("║                 Sinh viên thực hiện: Bùi Trung Tín (0024416184)              ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝\n");
        Console.ResetColor();
    }
}
