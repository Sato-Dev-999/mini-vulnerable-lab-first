public class SqlInjectionResult
{
    public string? Username { get; set; }
    public string? Password { get; set; }

    public string? VulnerableSql { get; set; }
    public string? SafeSql { get; set; }

    public bool VulnerableLoginSuccess { get; set; }
    public bool SafeLoginSuccess { get; set; }

    public string? ErrorMessage { get; set; }
}