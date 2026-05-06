namespace Tests2sr_Module1
{
    [TestClass]
    public class RegistrationTests
    {
        private readonly RegistrationService _service = new();

        // Позитивные тесты

        [TestMethod]
        [DataRow("ivan_kireev", "Password123!", "ivana@example.com")]
        [DataRow("a", "Str0ng#Pass", "test@mail.ru")]
        [DataRow("verylongname155", "Valid1~Password", "valid@edu54.ru")]
        [DataRow("user", "A1b2c3d4e5_", "som@sub.domain.co.ru")]
        [DataRow("short", "Aa1!abcdefg", "name@gmail.com")]
        public void Register_ValidInput_ReturnsSuccess(string username, string password, string email)
        {
            // Act
            var result = _service.Register(username, password, email);

            // Assert
            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual("Регистрация прошла успешно", result.Message);
        }

        // Негативные тесты(ошибки валидации)

        [TestMethod]
        [DataRow("", "Password123!", "user@example.com", "Имя пользователя должно быть от 1 до 15 символов")]
        [DataRow(null, "Password123!", "user@example.com", "Имя пользователя должно быть от 1 до 15 символов")]
        [DataRow("   ", "Password123!", "user@example.com", "Имя пользователя должно быть от 1 до 15 символов")]
        [DataRow("thisusernameiswaytoolong16", "Password123!", "user@example.com", "Имя пользователя должно быть от 1 до 15 символов")]
        public void Register_InvalidUsername_ReturnsFailure(string username, string password, string email, string expectedMessage)
        {
            // Act
            var result = _service.Register(username, password, email);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedMessage, result.Message);
        }

        [TestMethod]
        [DataRow("user", "short", "user@example.com", "Пароль должен содержать не менее 8 символов, включая верхний и нижний регистр, цифру и спецсимвол")]
        [DataRow("user", "nouppercase1!", "user@example.com", "Пароль должен содержать не менее 8 символов, включая верхний и нижний регистр, цифру и спецсимвол")]
        [DataRow("user", "NOLOWERCASE1!", "user@example.com", "Пароль должен содержать не менее 8 символов, включая верхний и нижний регистр, цифру и спецсимвол")]
        [DataRow("user", "NoDigit_!", "user@example.com", "Пароль должен содержать не менее 8 символов, включая верхний и нижний регистр, цифру и спецсимвол")]
        [DataRow("user", "NoSpecial123", "user@example.com", "Пароль должен содержать не менее 8 символов, включая верхний и нижний регистр, цифру и спецсимвол")]
        [DataRow("user", "1234567890", "user@example.com", "Пароль должен содержать не менее 8 символов, включая верхний и нижний регистр, цифру и спецсимвол")] // нет букв
        [DataRow("user", "abcdefghij", "user@example.com", "Пароль должен содержать не менее 8 символов, включая верхний и нижний регистр, цифру и спецсимвол")] // нет цифр и спецсимволов
        public void Register_InvalidPassword_ReturnsFailure(string username, string password, string email, string expectedMessage)
        {
            // Act
            var result = _service.Register(username, password, email);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedMessage, result.Message);
        }

        [TestMethod]
        [DataRow("user", "Password123!", "invalid-email")]
        [DataRow("user", "Password123!", "missingatsign.com")]
        [DataRow("user", "Password123!", "user@")]
        [DataRow("user", "Password123!", "@domain.com")]
        [DataRow("user", "Password123!", "user with space@domain.com")]
        public void Register_InvalidEmail_ReturnsFailure(string username, string password, string email)
        {
            // Act
            var result = _service.Register(username, password, email);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Неверный формат электронной почты", result.Message);
        }

        // граничное значение длины пароля = 10, но с отсутствием спецсимвола
        [TestMethod]
        public void Register_PasswordLength10ButMissingSpecial_ReturnsFailure()
        {
            // Arrange
            string username = "user";
            string password = "Aa1bcdefgh"; // 10 символов, есть цифра, верхний и нижний регистр, но нет спецсимвола
            string email = "test@example.com";

            // Act
            var result = _service.Register(username, password, email);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Пароль должен содержать не менее 8 символов, включая верхний и нижний регистр, цифру и спецсимвол", result.Message);
        }
    }
}
