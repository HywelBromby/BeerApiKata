namespace PubApi.Beer.Models
{
    public class GetBeersRequest
    {
        public decimal gtAlcoholByVolume { get; set; }
        public decimal ltAlcoholByVolume { get; set; }
    }
}