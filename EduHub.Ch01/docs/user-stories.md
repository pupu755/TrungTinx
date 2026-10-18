# Phân Tích Yêu Cầu Người Dùng (User Stories) - Hệ Thống EduHub

- **Dự án**: EduHub - Nền tảng quản lý Câu lạc bộ & Sự kiện sinh viên
- **Mã sinh viên**: 0024416184
- **Hoạt động**: HĐ1.3 (CLO1, CLO4, CLO5)
- **Tài liệu**: Danh sách User Story và Tiêu chí nghiệm thu (Acceptance Criteria)

---

## Danh Sách Vai Trò Người Dùng (Actors)
1. **Thành viên (Member)**: Sinh viên tham gia vào các câu lạc bộ, đăng ký sự kiện và thực hiện điểm danh (check-in).
2. **Trưởng CLB (Club Leader)**: Sinh viên điều hành hoạt động câu lạc bộ, quản lý thành viên, tạo sự kiện, duyệt đăng ký tham dự.
3. **Quản trị viên (Administrator)**: Cán bộ quản lý hệ thống, kiểm soát việc thành lập câu lạc bộ, theo dõi các báo cáo và thống kê toàn trường.

---

## 1. Nhóm User Story: Vai Trò Thành Viên (Member)

### US-01: Đăng ký tham gia Câu lạc bộ
- **Là một**: Sinh viên (Thành viên tương lai)
- **Tôi muốn**: Gửi yêu cầu gia nhập vào một Câu lạc bộ học thuật / kỹ năng
- **Để**: Có thể tham gia sinh hoạt định kỳ và các sự kiện nội bộ do CLB tổ chức.

#### Tiêu chí nghiệm thu (Acceptance Criteria):
- **AC-01.1 (Đăng ký thành công)**:
  - *Given*: Sinh viên có mã số sinh viên (MSSV) hợp lệ, tài khoản đang hoạt động và chưa là thành viên của CLB "CodeCraft".
  - *When*: Sinh viên chọn CLB "CodeCraft" và nhấn gửi yêu cầu tham gia.
  - *Then*: Hệ thống ghi nhận thông tin thành viên vào danh sách của CLB với ngày gia nhập tương ứng.
- **AC-01.2 (Ngăn chặn trùng lặp)**:
  - *Given*: Sinh viên đã là thành viên chính thức của CLB "CodeCraft".
  - *When*: Sinh viên gửi lại yêu cầu gia nhập CLB "CodeCraft".
  - *Then*: Hệ thống từ chối yêu cầu và hiển thị thông báo lỗi rằng sinh viên đã là thành viên của câu lạc bộ này.

---

### US-02: Đăng ký tham gia Sự kiện
- **Là một**: Thành viên câu lạc bộ
- **Tôi muốn**: Đăng ký tham gia sự kiện ngoại khóa do CLB tổ chức
- **Để**: Đặt chỗ và nhận thông báo tham dự workshop/hội thảo.

#### Tiêu chí nghiệm thu (Acceptance Criteria):
- **AC-02.1 (Tạo phiếu đăng ký khi còn chỗ)**:
  - *Given*: Sự kiện "Workshop DDD" có sức chứa tối đa là 50 người, hiện mới có 30 người đăng ký.
  - *When*: Thành viên gửi yêu cầu đăng ký tham gia sự kiện.
  - *Then*: Hệ thống tạo một phiếu đăng ký (`Registration`) mới với trạng thái ban đầu là `Pending` (Chờ duyệt).
- **AC-02.2 (Ngăn chặn khi sự kiện đã đầy)**:
  - *Given*: Sự kiện "Workshop DDD" đã đạt giới hạn sức chứa tối đa (50/50).
  - *When*: Một thành viên mới cố gắng gửi đăng ký tham gia sự kiện.
  - *Then*: Hệ thống ném ngoại lệ hoặc thông báo từ chối đăng ký do sự kiện đã hết chỗ.
- **AC-02.3 (Ngăn chặn đăng ký trùng lặp)**:
  - *Given*: Thành viên đã có phiếu đăng ký ở trạng thái `Pending` hoặc `Confirmed` cho sự kiện.
  - *When*: Thành viên gửi tiếp một đăng ký khác cho cùng sự kiện đó.
  - *Then*: Hệ thống từ chối và báo lỗi rằng thành viên đã đăng ký sự kiện này trước đó.

---

### US-03: Điểm danh (Check-in) tại sự kiện
- **Là một**: Thành viên đã đăng ký sự kiện
- **Tôi muốn**: Thực hiện check-in điểm danh khi có mặt tại hội trường
- **Để**: Ghi nhận sự hiện diện thực tế và hoàn thành quyền lợi tham dự.

#### Tiêu chí nghiệm thu (Acceptance Criteria):
- **AC-03.1 (Check-in thành công cho phiếu đã duyệt)**:
  - *Given*: Thành viên sở hữu phiếu đăng ký ở trạng thái `Confirmed` (Đã xác nhận).
  - *When*: Thành viên quét mã QR hoặc check-in thủ công tại bàn lễ tân.
  - *Then*: Hệ thống tạo bản ghi `CheckIn` liên kết với phiếu đăng ký, lưu thời điểm check-in và phương thức check-in.
- **AC-03.2 (Từ chối check-in phiếu chưa xác nhận hoặc đã huỷ)**:
  - *Given*: Phiếu đăng ký của sinh viên đang ở trạng thái `Pending` hoặc `Cancelled`.
  - *When*: Sinh viên cố gắng check-in tại sự kiện.
  - *Then*: Hệ thống từ chối thao tác check-in và báo lỗi: "Chỉ phiếu đăng ký ở trạng thái Confirmed mới được phép Check-in".

---

## 2. Nhóm User Story: Vai Trò Trưởng CLB (Club Leader)

### US-04: Tạo mới Sự kiện Câu lạc bộ
- **Là một**: Trưởng câu lạc bộ
- **Tôi muốn**: Khởi tạo sự kiện mới với các thông tin chi tiết (tiêu đề, địa điểm, thời gian, giới hạn số người)
- **Để**: Mở đăng ký cho các thành viên tham gia.

#### Tiêu chí nghiệm thu (Acceptance Criteria):
- **AC-04.1 (Tạo sự kiện hợp lệ)**:
  - *Given*: Trưởng CLB cung cấp tiêu đề không rỗng, thời gian kết thúc sau thời gian bắt đầu, và sức chứa tối đa lớn hơn 0.
  - *When*: Trưởng CLB nhấn tạo sự kiện.
  - *Then*: Sự kiện được tạo thành công với mã định danh duy nhất và liên kết chính xác tới mã CLB chủ quản.
- **AC-04.2 (Ràng buộc dữ liệu thời gian và sức chứa)**:
  - *Given*: Thời gian kết thúc sự kiện được thiết lập trước thời gian bắt đầu, hoặc sức chứa <= 0.
  - *When*: Trưởng CLB gửi yêu cầu tạo sự kiện.
  - *Then*: Hệ thống báo lỗi dữ liệu đầu vào không hợp lệ và hủy bỏ việc tạo sự kiện.

---

### US-05: Duyệt và Xác nhận Phiếu đăng ký
- **Là một**: Trưởng câu lạc bộ (hoặc Ban tổ chức sự kiện)
- **Tôi muốn**: Xác nhận (`Confirm`) các phiếu đăng ký tham dự của thành viên
- **Để**: Chốt danh sách người tham gia chính thức trước giờ diễn ra sự kiện.

#### Tiêu chí nghiệm thu (Acceptance Criteria):
- **AC-05.1 (Xác nhận phiếu hợp lệ)**:
  - *Given*: Phiếu đăng ký đang ở trạng thái `Pending`.
  - *When*: Trưởng CLB thực hiện thao tác duyệt (`Confirm`).
  - *Then*: Trạng thái phiếu chuyển thành `Confirmed`, ghi nhận thời gian xác nhận (`ConfirmedAt`).
- **AC-05.2 (Bảo vệ quy tắc nghiệp vụ với phiếu đã huỷ)**:
  - *Given*: Phiếu đăng ký đã bị huỷ (`Cancelled`).
  - *When*: Người dùng cố tình gọi hành vi xác nhận (`Confirm()`).
  - *Then*: Hệ thống kiên quyết từ chối, ném ngoại lệ nghiệp vụ thông báo: "Không thể xác nhận phiếu đăng ký đã bị huỷ".

---

## 3. Nhóm User Story: Vai Trò Quản Trị Viên (Administrator)

### US-06: Phê duyệt thành lập Câu lạc bộ và Gán Trưởng CLB
- **Là một**: Quản trị viên hệ thống
- **Tôi muốn**: Khởi tạo thông tin Câu lạc bộ mới và chỉ định một sinh viên làm Trưởng CLB
- **Để**: Cho phép câu lạc bộ bắt đầu hoạt động chính thức trong khuôn khổ nhà trường.

#### Tiêu chí nghiệm thu (Acceptance Criteria):
- **AC-06.1 (Thành lập CLB thành công)**:
  - *Given*: Mã CLB (Code) chưa tồn tại trong hệ thống, thông tin tên CLB đầy đủ và ID của Trưởng CLB hợp lệ.
  - *When*: Quản trị viên phê duyệt thành lập CLB.
  - *Then*: Hệ thống lưu trữ CLB mới, thiết lập người đại diện là Trưởng CLB và tự động thêm Trưởng CLB vào danh sách thành viên.
- **AC-06.2 (Kiểm tra dữ liệu bắt buộc)**:
  - *Given*: Mã CLB hoặc Tên CLB bị để trống, hoặc ID Trưởng CLB rỗng.
  - *When*: Quản trị viên gửi form tạo CLB.
  - *Then*: Hệ thống ném ngoại lệ dữ liệu đầu vào và không tạo CLB.

---

### US-07: Xuất Báo cáo Thống kê Tham dự Sự kiện
- **Là một**: Quản trị viên hoặc Trưởng CLB
- **Tôi muốn**: Xem báo cáo tổng kết tham gia sự kiện gồm sức chứa, số đăng ký, số lượt check-in và tỷ lệ tham dự
- **Để**: Đánh giá mức độ quan tâm của sinh viên và hiệu quả tổ chức phong trào.

#### Tiêu chí nghiệm thu (Acceptance Criteria):
- **AC-07.1 (Tính toán tỷ lệ chính xác)**:
  - *Given*: Sự kiện có sức chứa 50 chỗ, 40 người đăng ký, trong đó 30 người có mặt check-in.
  - *When*: Hệ thống tổng hợp báo cáo tham dự.
  - *Then*: Báo cáo hiển thị: Tổng đăng ký = 40, Sức chứa = 50, Đã check-in = 30, Tỷ lệ tham dự = 75.0% (30/40) hoặc 60.0% (30/50 theo sức chứa).
- **AC-07.2 (Xử lý an toàn khi không có đăng ký)**:
  - *Given*: Sự kiện mới tạo chưa có lượt đăng ký hoặc check-in nào.
  - *When*: Xuất báo cáo tham dự.
  - *Then*: Tỷ lệ tham dự trả về 0.0% an toàn, không xảy ra lỗi chia cho 0 (DivideByZeroException).
