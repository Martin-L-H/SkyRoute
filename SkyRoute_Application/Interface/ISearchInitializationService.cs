public interface ISearchInitializationService
{
    Task<ServiceResponse<SearchInitializationDTO>> GetMetadataAsync();
}