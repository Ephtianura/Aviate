namespace Aviate.Application.Contracts
{
    public interface IDegenerateService
    {
        Task GenerateRandomFlightsAsync(int flightsPerAirplane = 200);
        Task GenerateRandomUkrFlightsAsync();
    }
}