namespace Server.Utils {
    public class Password {
        public static string HashPassword(string plainPassword) {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(plainPassword, BCrypt.Net.HashType.SHA512);
        }

        public static bool IsValidPassword(string plainPassword, string hashedPassword) {
            return BCrypt.Net.BCrypt.EnhancedVerify(plainPassword, hashedPassword, BCrypt.Net.HashType.SHA512);
        }
    }
}