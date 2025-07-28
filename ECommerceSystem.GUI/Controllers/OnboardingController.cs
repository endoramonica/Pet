using ECommerceSystem.GUI.Apis;
using ECommerceSystem.GUI.Services; // Giả sử AuthService ở đây
using ECommerceSystem.Shared.DTOs.OnBoarding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

// Bắt buộc người dùng phải đăng nhập để truy cập bất kỳ action nào trong controller này
[Authorize]
public class OnboardingController : Controller
{
    private readonly IOnboardingApi _onboardingApi;
    private readonly AuthService _authService; // Dùng để kiểm tra trạng thái đã hoàn thành hay chưa
    private readonly ILogger<OnboardingController> _logger;

    public OnboardingController(IOnboardingApi onboardingApi, AuthService authService, ILogger<OnboardingController> logger)
    {
        _onboardingApi = onboardingApi;
        _authService = authService;
        _logger = logger;
    }

    // === ACTION 1: Hiển thị trang Onboarding ban đầu ===
    // GET: /Onboarding/Index
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Kiểm tra xem người dùng có thực sự cần onboarding không.
        // Đây là lớp bảo vệ quan trọng để người dùng đã hoàn thành không thể vào lại.
        var isCompleted = await _authService.IsOnboardingCompletedAsync();
        if (isCompleted)
        {
            _logger.LogInformation("Người dùng đã hoàn thành onboarding, chuyển hướng về trang chủ.");
            return RedirectToAction("Index", "Home");
        }

        // Nếu bạn cần tải dữ liệu gì cho View (ví dụ: danh sách khu vực), hãy làm ở đây.
        // ViewBag.Locations = await _locationService.GetAllAsync();

        // Trả về View onboarding.cshtml
        return View();
    }

    // === ACTION 2: Endpoint cho JS để lấy dữ liệu đã lưu ===
    // GET: /Onboarding/GetData
    [HttpGet]
    public async Task<IActionResult> GetData()
    {
        var result = await _onboardingApi.GetOnboardingDataAsync();

        if (result.Success)
        {
            // Trả về dữ liệu dưới dạng JSON để JS có thể điền vào form
            return Ok(result.Data);
        }

        // Trả về lỗi nếu không lấy được dữ liệu
        return BadRequest(new { message = result.Message });
    }


    // === ACTION 3: Endpoint cho JS để lưu tiến trình của mỗi bước ===
    // POST: /Onboarding/Update
    // POST: /Onboarding/Update
    [HttpPost]
    public async Task<IActionResult> Update([FromBody] OnboardingRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Nhận được yêu cầu cập nhật onboarding cho bước: {Step}", request.CurrentStep);

        try
        {
            // Gọi đến API thực sự thông qua Refit
            var result = await _onboardingApi.UpdateOnboardingAsync(request);

            if (result.Success)
            {
                // Trả về kết quả thành công dưới dạng JSON
                return Ok(new { success = true, message = "Cập nhật thành công!" });
            }

            // Trả về lỗi nếu API báo lỗi nhưng không ném exception (lỗi logic nghiệp vụ)
            return BadRequest(new { success = false, message = result.Message ?? "Đã có lỗi xảy ra từ API." });
        }
        catch (Refit.ApiException ex)
        {
            // Bắt lỗi từ Refit (4xx, 5xx)
            _logger.LogError(ex, "Lỗi API khi cập nhật onboarding. Status: {StatusCode}", ex.StatusCode);

            // Cố gắng đọc nội dung lỗi từ API để trả về cho client
            var errorContent = await ex.GetContentAsAsync<object>(); // Đọc nội dung lỗi nếu có
            return BadRequest(new { success = false, message = $"Lỗi từ API: {ex.ReasonPhrase}", details = errorContent });
        }
        catch (Exception ex)
        {
            // Bắt các lỗi khác (ví dụ: mất kết nối mạng)
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật onboarding.");
            return StatusCode(500, new { success = false, message = "Lỗi hệ thống không mong muốn." });
        }
    }
}