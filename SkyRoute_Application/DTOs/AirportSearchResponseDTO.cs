public class AirportSearchResponseDTO
{

    public int Id { get; set; }
    public required string Name { get; set; }

    public required string CodeIATA { get; set; }

    public required string CityName { get; set; }

    public int CityId { get; set; }

    public required string CountryName { get; set; }

    public int CountryId { get; set; }

}