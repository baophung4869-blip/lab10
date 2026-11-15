/****************/
Mã sinh viên: 20237303
Họ tên: Phùng Ngọc Bảo
/****************/

namespace SmartHomeMini.Contracts;

/// <summary>Hợp đồng chung cho mọi thiết bị có thể bật và tắt.</summary>
public interface IDevice
{
    // Thuộc tính bổ sung phục vụ theo dõi trạng thái và kiểm thử tự động.
    string Name { get; }
    bool IsOn { get; }

    void TurnOn();
    void TurnOff();
}
