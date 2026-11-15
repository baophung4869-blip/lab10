/****************/
Mã sinh viên: 20237303
Họ tên: Phùng Ngọc Bảo
/****************/

namespace SmartHomeMini.Contracts;

/// <summary>Hành vi khóa riêng, không áp đặt cho đèn hoặc quạt.</summary>
public interface ILockable
{
    bool IsLocked { get; }

    void Lock();
    void Unlock();
}
