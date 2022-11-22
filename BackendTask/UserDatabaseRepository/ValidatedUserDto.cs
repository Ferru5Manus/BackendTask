using System.ComponentModel.DataAnnotations;
namespace BackendTask
{
    public class ValidatedUserDto
    {

        [Id]
        public int id { get; set; }
        [Required(ErrorMessage ="No username!")]
        [StringLength(50,MinimumLength = 5,ErrorMessage = "Invalid username length!")]
        [UserName]
        public string username { get; set; }
        [Required(ErrorMessage = "No password!")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Invalid password length!")]
        [Password]
        public string password { get; set; }
        [Date]
        public DateTime created_at { get; set; }
        
        [Date]
        public DateTime updated_at { get; set; }
    }
    /// <summary>
    /// Checks if name is not admin and does not contain invalid symbols.
    /// </summary>
    public class UserNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is string userName)
            {
                bool flag = false;
                foreach (var item in "\"';<>?/{}[]@#$%^&*()-+=_ ")
                {
                    if (userName.Contains(item))
                    {
                        flag = true;
                    }
                }
                int lettersUpperCount = 0;
                int lettersLowerCount = 0;
                int numbersCount = 0;
                foreach (char c in userName)
                {
                    if (char.IsUpper(c))
                    {
                        lettersUpperCount += 1;
                    }
                    if (char.IsLower(c))
                    {
                        lettersLowerCount += 1;
                    }
                    if (char.IsNumber(c))
                    {
                        numbersCount += 1;
                    }
                }
                if (userName.ToLower() != "admin" && flag == false) 
                {
                    return true;
                }
                else
                {
                    ErrorMessage = "Invalid username "+userName;
                }
                    
            }
            return false;
        }
    }
    /// <summary>
    /// Checks if password contains invalid symbols.
    /// </summary>
    public class PasswordAttribute : ValidationAttribute
    {
        
        public override bool IsValid(object? value)
        {
            if (value is string password)
            {
                bool flag = false;
                foreach (var item in "\"';<>?/{}[]@#$%^&*()-+=_ ")
                {
                    if (password.Contains(item))
                    {
                        flag = true;
                    }
                }
                int lettersUpperCount = 0;
                int lettersLowerCount = 0;
                int numbersCount = 0;
                foreach (char c in password)
                {
                    if (char.IsUpper(c))
                    {
                        lettersUpperCount += 1;
                    }
                    if (char.IsLower(c))
                    {
                        lettersLowerCount += 1;
                    }
                    if (char.IsNumber(c))
                    {
                        numbersCount += 1;
                    }
                }
                if (flag == false && numbersCount >= 1 && lettersLowerCount >= 1 && lettersUpperCount >= 1)
                {
                    return true;
                }
                else if (numbersCount < 1 || lettersUpperCount < 1 || lettersLowerCount < 1)
                {
                    if (flag == true)
                    {
                        ErrorMessage = "Password must have at least one number, one capital and one small letter!"+ System.Environment.NewLine+ "Invalid password!";

                    }
                    else
                    {
                        ErrorMessage = "Password must have at least one number, one capital and one small letter!";
                    }
                }
                else
                {
                    ErrorMessage = "Invalid password!";
                }

            }
            return false;
        }
    }
    /// <summary>
    /// Checks if id is null or less than zero.
    /// </summary>
    public class IdAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is int id)
            {
                if (id > 0)
                {
                    return true;
                }
                else
                {
                    ErrorMessage = "Invalid user id!";
                }
            }
            return false;
        }

    }
    /// <summary>
    /// Checks if DateTime is null.
    /// </summary>
    public class DateAttribute: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                if (date != DateTime.MinValue)
                {
                    return true;
                }
                else
                {
                    ErrorMessage = "Invalid date!";
                }
            }
            return false;
        }
    }
}
