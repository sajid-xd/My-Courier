
namespace mycourier.Controllers
{
    internal class ApplicationDbContext
    {
        public IEnumerable<object> Deliveries { get; internal set; }
        public object Users { get; internal set; }
    }
}