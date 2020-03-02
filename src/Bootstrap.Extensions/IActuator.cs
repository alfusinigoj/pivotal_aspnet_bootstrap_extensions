
namespace PivotalServices.AspNet.Bootstrap.Extensions
{
    public interface IActuator
    {
        void Configure();
        void Stop();
        void Start();
    }
}
