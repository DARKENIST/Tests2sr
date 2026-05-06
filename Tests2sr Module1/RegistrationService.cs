public class RegistrationService
{
    public RegistrationResult Register(string username, string password, string email)
    {
        // Валидации

        if (string.IsNullOrWhiteSpace(username) || username.Length > 15)
            return new RegistrationResult(false, "Имя пользователя должно быть от 1 до 15 символов");

        if (password.Length < 10 ||
            !password.Any(char.IsDigit) ||
            !password.Any(char.IsUpper) ||
            !password.Any(char.IsLower) ||
            !password.Any(c => "!#%*()_~".Contains(c)))
            return new RegistrationResult(false, "Пароль должен содержать не менее 8 символов, включая верхний и нижний регистр, цифру и спецсимвол");
        if (!IsValidEmail(email))
            return new RegistrationResult(false, "Неверный формат электронной почты");

        return new RegistrationResult(true, "Регистрация прошла успешно");
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

public record RegistrationResult(bool Success, string Message); public class RegistrationTests
{
}