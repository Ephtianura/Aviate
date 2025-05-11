namespace Aviate.Core.Models
{
    // ================= AIRPORT =================
    public class Airport
    {
        private Airport(string name, string code, string country, string city)
        {
            Id = Guid.NewGuid();
            Name = name.Trim();
            Code = NormalizeCode(code);
            Country = country.Trim();
            City = city.Trim();
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Country { get; private set; }
        public string City { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        public static Airport Create(string name, string code, string country, string city)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty");

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty");

            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Country cannot be empty");

            return new Airport(name, code, country, city);
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be empty");
            Name = newName.Trim();
            Touch();
        }
        public void ChangeCity(string newCity)
        {
            if (string.IsNullOrWhiteSpace(newCity))
                throw new ArgumentException("City cannot be empty");
            City = newCity.Trim();
            Touch();
        }
        public void ChangeCountry(string newCountry)
        {
            if (string.IsNullOrWhiteSpace(newCountry))
                throw new ArgumentException("Country cannot be empty");
            Country = newCountry.Trim();
            Touch();
        }

        private string NormalizeCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Code cannot be empty");

            code = code.Trim().ToUpperInvariant();

            if (code.Length != 3 || !code.All(char.IsLetter))
                throw new ArgumentException("Code must consist of exactly 3 letters (A-Z)");

            return code;
        }

        private void Touch() => UpdatedAt = DateTimeOffset.UtcNow;

        public override string ToString() => $"{Code} - {Name} ({City}, {Country})";
    }
}
