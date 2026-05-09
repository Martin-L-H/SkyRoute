
using System.ComponentModel.DataAnnotations;

namespace SkyRoute_Domain.Entities
{

    public class Airport
    {

        public int Id { get; set; }

        public int CityId { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string CodeIATA { get; set; }

        public string PublicName { get; set; }

        public City City { get; set; }

    }
}
