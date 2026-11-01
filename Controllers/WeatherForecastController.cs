using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RequestsApi.Data;

namespace RequestsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Внедрение зависимости через конструктор
        public RequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/requests
        [HttpGet]
        public async Task<IActionResult> GetRequests(
            [FromQuery] string? format = null,
            [FromQuery] bool simulateError = false)
        {
            try
            {
                // 1. Симуляция внутренней ошибки сервера (тест 500)
                if (simulateError)
                {
                    throw new Exception("Имитация внутренней ошибки сервера");
                }

                // 2. Обработка некорректного запроса (тест 400)
                if (!string.IsNullOrEmpty(format) && format.ToLower() == "xml")
                {
                    return BadRequest(new { error = "Некорректный параметр format. Поддерживается только JSON." });
                }

                // 3. Проверка подключения к базе данных
                if (!await _context.Database.CanConnectAsync())
                {
                    return StatusCode(500, new { error = "Ошибка подключения к базе данных" });
                }

                // 4. Получение данных с JOIN таблиц Requests и Clients
                var requests = await _context.Requests
                    .Include(r => r.Client)
                    .ToListAsync();

                // 5. Если таблица пуста — возвращаем пустой массив (200 OK)
                if (requests == null || !requests.Any())
                {
                    return Ok(new List<object>());
                }

                // 6. Обработка данных согласно требованиям:
                //    - title_client = "{Title} - {Login}"
                //    - formatted_date = ДД.ММ.ГГГГ
                var result = requests.Select(r => new
                {
                    id = r.Id,
                    title_client = $"{r.Title} - {r.Client?.Login ?? "Неизвестно"}",
                    content = r.Content,
                    formatted_date = r.CreatedAt.ToString("dd.MM.yyyy")
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // 7. Обработка любых необработанных исключений (500)
                return StatusCode(500, new
                {
                    error = "Внутренняя ошибка сервера",
                    details = ex.Message
                });
            }
        }
    }
}
