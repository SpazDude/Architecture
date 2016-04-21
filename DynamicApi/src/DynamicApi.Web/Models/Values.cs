namespace DynamicApi.Web.Models
{
    public class Values : IId
    {
        public Values()
        {
            Name = this.GetType().FullName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
