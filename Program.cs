/****************/
Mã sinh viên: 20237303
Họ tên: Phùng Ngọc Bảo
/****************/

using System.Text;
using SmartHomeMini.Contracts;
using SmartHomeMini.Devices;
using SmartHomeMini.Patterns;
using SmartHomeMini.Testing;

namespace SmartHomeMini;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Chạy kiểm thử không cần gói NuGet: dotnet run -- --test
        if (args.Contains("--test", StringComparer.OrdinalIgnoreCase))
        {
            LabTests.RunAll();
            return;
        }

        // Màn hình demo không tương tác, thuận tiện chụp kết quả cho báo cáo.
        if (args.Contains("--demo", StringComparer.OrdinalIgnoreCase))
        {
            RunDemo();
            return;
        }

        RunInteractive();
    }

    private static (IDevice light, IDevice fan, IDevice door, SmartHomeFacade facade)
        CreateMainDevices()
    {
        // Client không dùng trực tiếp constructor của SmartLight/Fan/Door.
        IDevice light = DeviceFactory.Create("light", "Living room light");
        IDevice fan = DeviceFactory.Create("fan", "Living room fan");
        IDevice door = DeviceFactory.Create("door", "Main door");

        if (door is not ILockable lockableDoor)
            throw new InvalidOperationException("Main door must implement ILockable.");

        return (light, fan, door, new SmartHomeFacade(light, fan, lockableDoor));
    }

    private static void RunDemo()
    {
        var (light, fan, door, facade) = CreateMainDevices();
        Console.WriteLine("=== SMART HOME MINI | DEMO ===");
        Console.WriteLine($"Home: {HomeConfiguration.Instance.HomeName}");
        light.TurnOn();
        fan.TurnOn();
        ((ILockable)door).Unlock();

        Console.WriteLine("\n[GoToWork]");
        facade.GoToWork();
        Console.WriteLine("\n[GoToSleep]");
        facade.GoToSleep();
    }

    private static void RunInteractive()
    {
        var (light, fan, door, facade) = CreateMainDevices();
        var extraDevices = new List<IDevice>();

        while (true)
        {
            Console.WriteLine("\n======= SMART HOME MINI =======");
            Console.WriteLine("1. Bật đèn và quạt");
            Console.WriteLine("2. Chế độ Đi làm");
            Console.WriteLine("3. Chế độ Đi ngủ");
            Console.WriteLine("4. Tạo thiết bị mới theo loại người dùng nhập");
            Console.WriteLine("5. Xem trạng thái thiết bị chính");
            Console.WriteLine("6. Chạy kiểm thử T1-T7");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn: ");

            string? choice = Console.ReadLine();
            if (choice is null || choice == "0") return;

            switch (choice.Trim())
            {
                case "1":
                    light.TurnOn();
                    fan.TurnOn();
                    break;
                case "2":
                    facade.GoToWork();
                    break;
                case "3":
                    facade.GoToSleep();
                    break;
                case "4":
                    Console.Write("Loại (light/fan/door): ");
                    string? type = Console.ReadLine();
                    Console.Write("Tên thiết bị: ");
                    string? name = Console.ReadLine();
                    try
                    {
                        IDevice created = DeviceFactory.Create(type ?? "", name ?? "");
                        extraDevices.Add(created);
                        Console.WriteLine($"Đã tạo {created.GetType().Name}: {created.Name}");
                        Console.WriteLine($"Tổng thiết bị tạo thêm: {extraDevices.Count}");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Lỗi đầu vào: {ex.Message}");
                    }
                    break;
                case "5":
                    Console.WriteLine($"{light.Name}: {(light.IsOn ? "ON" : "OFF")}");
                    Console.WriteLine($"{fan.Name}: {(fan.IsOn ? "ON" : "OFF")}");
                    Console.WriteLine($"{door.Name}: {(door.IsOn ? "ON" : "OFF")}");
                    Console.WriteLine($"Main door lock: {(((ILockable)door).IsLocked ? "LOCKED" : "UNLOCKED")}");
                    break;
                case "6":
                    LabTests.RunAll();
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    break;
            }
        }
    }
}
