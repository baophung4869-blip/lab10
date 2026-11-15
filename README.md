# Lab W10 - Smart Home Mini (C#/.NET 8)

**Sinh viên:** Phùng Ngọc Bảo · **MSSV:** 20237303  
**Nguồn đề:** `lab w10-pattern.pdf` - Bài thực hành 10, Smart Home Mini.

> Trạng thái: **mã nguồn và test được chuẩn bị để chạy; chưa có kết quả build/run được xác nhận trong môi trường tạo tài liệu.** Khi chạy trên máy cá nhân, cập nhật báo cáo bằng ảnh console và kết quả thực tế.

## Yêu cầu môi trường
- .NET SDK 8.0 trở lên có khả năng build dự án target `net8.0`.

## Lệnh chạy

```bash
cd SmartHomeMini
dotnet restore
dotnet build
dotnet run -- --demo
dotnet run -- --test
dotnet run
```

`--demo`: chạy kịch bản cố định GoToWork, GoToSleep.  
`--test`: chạy 7 phép kiểm tra console.  
Không nhập tham số: chạy menu tương tác và tạo thiết bị theo loại người dùng nhập.

## Cấu trúc thư mục

```text
SmartHomeMini/
  Contracts/  IDevice.cs, ILockable.cs
  Devices/    SmartLight.cs, SmartFan.cs, SmartDoor.cs
  Patterns/   DeviceFactory.cs, HomeConfiguration.cs, SmartHomeFacade.cs
  Testing/    LabTests.cs
  docs/       UML.puml, UML.mmd, AI_SESSION_TEMPLATE.md
  Program.cs, SmartHomeMini.csproj, README.md
```

## Các mẫu thiết kế

| Pattern | Lớp | Vị trí, mục đích | Lợi ích |
|---|---|---|---|
| Factory | `DeviceFactory` | `Create(type,name)` | Client không `new` từng loại, chuẩn hóa đầu vào và ném lỗi rõ ràng |
| Singleton | `HomeConfiguration` | `Instance` tạo qua `Lazy<T>` | Một đối tượng cấu hình dùng chung, khởi tạo an toàn luồng |
| Facade | `SmartHomeFacade` | `GoToWork`, `GoToSleep` | Một lệnh cho nhiều thao tác, phụ thuộc `IDevice`/`ILockable` |

**Hạn chế thiết kế:** `DeviceFactory` dùng `switch`, nên khi thêm `SmartCamera` cần sửa Factory (phụ thuộc đăng ký loại). `SmartHomeFacade` đọc `HomeConfiguration.Instance` trực tiếp, khiến test độc lập khó hơn việc inject một interface cấu hình. 

**Điểm lưu ý về trạng thái:** việc `GoToSleep` báo `NightLightLevel` chỉ là mô phỏng thông báo, chưa có API điều chỉnh độ sáng phần cứng. Đèn và quạt triển khai `IDevice`; riêng cửa triển khai thêm `ILockable`.

## Kiểm thử

| Mã | Trường hợp | Điều kiện đỗ |
|---|---|---|
| T1 | Factory tạo light/fan/door | Kết quả đúng `SmartLight`/`SmartFan`/`SmartDoor` |
| T2 | Factory tạo camera không hỗ trợ | Ném `ArgumentException` |
| T3 | Lấy cấu hình hai lần | `ReferenceEquals` bằng `true` |
| T4 | `GoToWork` | `light.IsOn=false`, `fan.IsOn=false`, `door.IsLocked=true` |
| T5 | `GoToSleep` | đèn bật, quạt tắt, cửa khóa và in `Night light level: 35%` khi cấu hình là 35 |
| T6 | Factory chuẩn hóa đầu vào | `"  LiGhT  "` vẫn tạo `SmartLight` |
| T7 | Tách hợp đồng khóa | Chỉ `SmartDoor` là `ILockable`, thao tác unlock thành công |

Kiểm thử dùng bộ chạy console tùy chỉnh (`Testing/LabTests.cs`), không dùng framework test ngoài và không yêu cầu tải NuGet. Kết quả thực tế phải lấy từ việc chạy lệnh `dotnet run -- --test` trên máy có SDK.

## Phần G - Trả lời 4 câu hỏi

**1. Factory che giấu điều gì?** Che giấu quyết định chọn lớp cụ thể (`SmartLight`/`SmartFan`/`SmartDoor`) và cách khởi tạo theo chuỗi loại thiết bị; client chỉ nhận `IDevice`.

**2. Singleton đảm bảo gì, rủi ro gì?** Một thể hiện cấu hình dùng chung trong tiến trình và khởi tạo lazy an toàn luồng. Rủi ro là trạng thái toàn cục có thể bị các test khác chỉnh sửa, tăng liên kết ngầm, khó cô lập kiểm thử.

**3. Facade khác Factory?** Factory tạo và trả về đối tượng; Facade cung cấp giao diện mức cao để phối hợp những đối tượng đã được tạo.

**4. Bỏ Facade thì client phải biết gì?** Client phải biết thứ tự tắt/bật đèn, quạt, khóa cửa, phương thức nào áp dụng cho cửa, và tự đọc cấu hình mức sáng ban đêm.

## Tuân thủ nộp bài và AI

1. **Tự làm và commit trước** khi tham khảo AI theo quy định môn học; nếu chưa làm, cần thực hiện và ghi trung thực thời điểm áp dụng AI, không ghi ngược thời gian hoặc trình bày code AI như tự viết hoàn toàn.
2. Sau đó trao đổi và phản biện với AI; ghi prompt, phản hồi thật, thay đổi dựa trên kiểm thử vào `docs/AI_SESSION_TEMPLATE.md` và báo cáo.
3. Đẩy project lên GitHub của chính bạn, điền URL **thật** vào báo cáo, không dùng liên kết giả.
4. Chụp hình kết quả `dotnet build`, `dotnet run -- --demo`, `dotnet run -- --test` và lịch sử commit.

## GitHub - lệnh tham khảo

```bash
git init
git add .
git commit -m "Lab W10: Smart Home Mini implementation"
git branch -M main
git remote add origin https://github.com/<TEN_TAI_KHOAN>/<TEN_REPO>.git
git push -u origin main
```

**Lưu ý:** Các lệnh trên không thay thế bằng chứng commit độc lập trước khi dùng AI nếu quy định môn học yêu cầu.
