public class SearchInitializationDTO
{
    public IEnumerable<AirportSearchResponseDTO?> Airports { get; set; }
    public List<EnumDisplayDTO?> CabinTypes { get; set; }
    public IEnumerable<string>? Providers { get; set; }
}