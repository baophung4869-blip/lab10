/****************/
Mã sinh viên: 20237303
Họ tên: Phùng Ngọc Bảo
/****************/

using System.Threading;

namespace SmartHomeMini.Patterns;

/// <summary>Singleton: một đối tượng cấu hình dùng chung trong ứng dụng.</summary>
public sealed class HomeConfiguration
{
    // Lazy<T> với ExecutionAndPublication khởi tạo một lần, an toàn luồng.
    private static readonly Lazy<HomeConfiguration> LazyInstance =
        new(() => new HomeConfiguration(), LazyThreadSafetyMode.ExecutionAndPublication);

    public static HomeConfiguration Instance => LazyInstance.Value;

    public string HomeName { get; set; } = "My Home";
    public int NightLightLevel { get; set; } = 30;

    // Không cho client gọi trực tiếp new HomeConfiguration().
    private HomeConfiguration() { }
}
