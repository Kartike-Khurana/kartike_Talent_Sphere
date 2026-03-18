using System.Threading.Tasks;
using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface ISelectionRepository
    {
        Task<Selection> AddAsync(Selection selection);
        Task<Selection> GetByIdAsync(int id);
        Task SaveChangesAsync();
    }
}
