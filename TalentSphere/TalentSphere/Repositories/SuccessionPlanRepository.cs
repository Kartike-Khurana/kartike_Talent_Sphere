using Microsoft.EntityFrameworkCore;
using TalentSphere.Config;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;

namespace TalentSphere.Repositories
{
	public class SuccessionPlanRepository : ISuccessionPlanRepository
	{
		private readonly AppDbContext _context;
		public SuccessionPlanRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<SuccessionPlan> AddAsync(SuccessionPlan successionPlan) {
			var entity = (await _context.Set<SuccessionPlan>().AddAsync(successionPlan)).Entity;
			return entity;

		}
		public async Task<SuccessionPlan> GetByIdAsync(int id) {
			return await _context.Set<SuccessionPlan>()
				.Include(sp => sp.Employee)
				.Include(sp => sp.Successor)
				.FirstOrDefaultAsync(sp => sp.SuccessionID == id && !sp.IsDeleted);
		}
		public async Task<List<SuccessionPlan>> GetAllAsync()
		{
			return await _context.SuccessionPlans
				.Include(sp => sp.Employee)
				.Include(sp => sp.Successor)
				.Where(sp => !sp.IsDeleted)
				.ToListAsync();
		}
		public async Task UpdateAsync(SuccessionPlan plan){
			_context.SuccessionPlans.Update(plan);
			await _context.SaveChangesAsync();
		}
		public async Task DeleteAsync(SuccessionPlan plan){
			plan.IsDeleted = true;
			plan.UpdatedAt = DateTime.UtcNow;
			_context.SuccessionPlans.Update(plan);
			await _context.SaveChangesAsync();
		}
		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();

		}
	}
}
