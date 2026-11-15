/****************/
Mã sinh viên: 20237303
Họ tên: Phùng Ngọc Bảo
/****************/

using SmartHomeMini.Contracts;
using SmartHomeMini.Devices;

namespace SmartHomeMini.Patterns;

/// <summary>Factory: tập trung quyết định lớp cụ thể cần khởi tạo.</summary>
public static class DeviceFactory
{
    public static IDevice Create(string type, string name)
    {
        // Giữ hợp đồng: loại không hợp lệ phải ném ArgumentException.
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Loại thiết bị không được để trống.", nameof(type));

        // Chuẩn hóa đầu vào để hỗ trợ chữ hoa/thường và khoảng trắng.
        return type.Trim().ToLowerInvariant() switch
        {
            "light" => new SmartLight(name),
            "fan"   => new SmartFan(name),
            "door"  => new SmartDoor(name),
            _ => throw new ArgumentException(
                $"Unsupported device type: {type}", nameof(type))
        };
    }
}
