using System.Threading.Tasks;

namespace nat.Services
{
    public interface IConvertToExcel
    { 
        public Task<byte[]> ConvertDbToExcel();
    }
}
