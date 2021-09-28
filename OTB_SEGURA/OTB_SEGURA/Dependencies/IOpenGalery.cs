using System.IO;
using System.Threading.Tasks;

namespace OTB_SEGURA
{
    public interface IOpenGalery
    {
        Task<Stream> GetFotoAsync();
    }
}
