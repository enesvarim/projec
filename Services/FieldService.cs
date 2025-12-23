using Microsoft.EntityFrameworkCore;
using projec.Data;
using projec.Dtos;
using projec.Models;

namespace projec.Services
{
    public class FieldService : IFieldService
    {
        private readonly AppDbContext _context;

        public FieldService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Field> AddField(FieldDto request)
        {
            var field = new Field
            {
                Name = request.Name,
                Address = request.Address,
                HourlyRate = request.HourlyRate,
                IsActive = true
            };

            _context.Fields.Add(field);
            await _context.SaveChangesAsync();

            return field;
        }

        public async Task<List<Field>> GetAllFields()
        {
            return await _context.Fields.Where(f => f.IsActive).ToListAsync();
        }
    }
}
