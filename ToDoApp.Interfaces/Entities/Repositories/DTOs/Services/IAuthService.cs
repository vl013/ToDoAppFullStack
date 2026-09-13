using ToDoApp.Interfaces.DTOs;

namespace ToDoApp.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserRegisterDto dto);
        Task<string> LoginAsync(UserLoginDto dto);
    }
}