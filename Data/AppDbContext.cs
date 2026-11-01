using Microsoft.EntityFrameworkCore;
using RequestsApi.Models;

namespace RequestsApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Таблицы
        public DbSet<Client> Clients { get; set; }
        public DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Client)
                .WithMany()
                .HasForeignKey(r => r.IdClient);

            // Тестовые данные
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, Login = "master07", Password = "12345", Role = "Пользователь", IsBlocked = false },
                new Client { Id = 2, Login = "admin", Password = "admin", Role = "Администратор", IsBlocked = false },
                new Client { Id = 3, Login = "manager", Password = "pass123", Role = "Менеджер", IsBlocked = true }
            );

            modelBuilder.Entity<Request>().HasData(
                new Request { Id = 1, Title = "Замена экрана", Content = "iPhone 12", IdClient = 1, CreatedAt = new DateTime(2027, 3, 15) },
                new Request { Id = 2, Title = "Чистка ноутбука", Content = "Lenovo", IdClient = 1, CreatedAt = new DateTime(2027, 3, 16) },
                new Request { Id = 3, Title = "Установка ПО", Content = "Windows 11", IdClient = 2, CreatedAt = new DateTime(2027, 3, 17) }
            );
        }
    }
}
