namespace SkyRoute_Domain.Entities
{
    public class Country
    {

        public int Id { get; set; }

        public string Name { get; set; }

        //Navigation properties
        public List<City> Cities { get; set; }

    }
}
