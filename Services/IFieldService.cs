using projec.Dtos;
using projec.Models;

namespace projec.Services
{
    public interface IFieldService
    {
        Task<Field> AddField(FieldDto request);
        Task<List<Field>> GetAllFields();
    }
}
