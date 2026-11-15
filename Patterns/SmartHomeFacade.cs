/****************/
Mã sinh viên: 20237303
Họ tên: Phùng Ngọc Bảo
/****************/

using SmartHomeMini.Contracts;

namespace SmartHomeMini.Patterns;

/// <summary>Facade: đơn giản hóa chuỗi thao tác thiết bị theo kịch bản.</summary>
public sealed class SmartHomeFacade
{
    // Facade phụ thuộc interface, không tự tạo các thiết bị cụ thể.
    private readonly IDevice light;
    private readonly IDevice fan;
    private readonly ILockable door;

    public SmartHomeFacade(IDevice light, IDevice fan, ILockable door)
    {
        this.light = light ?? throw new ArgumentNullException(nameof(light));
        this.fan = fan ?? throw new ArgumentNullException(nameof(fan));
        this.door = door ?? throw new ArgumentNullException(nameof(door));
    }

    public void GoToWork()
    {
        // Đi làm: tắt đèn, tắt quạt, khóa cửa.
        light.TurnOff();
        fan.TurnOff();
        door.Lock();
    }

    public void GoToSleep()
    {
        // Đi ngủ: tắt quạt, khóa cửa, bật đèn ngủ và công bố mức sáng.
        fan.TurnOff();
        door.Lock();
        light.TurnOn();

        int level = HomeConfiguration.Instance.NightLightLevel;
        Console.WriteLine($"Night light level: {level}%");
    }
}
