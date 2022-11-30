using System.Collections.Generic;
using PaDayPlanner.Api.Entities;
using System.Threading.Tasks;

namespace PaDayPlanner.Api.Repositories
{
    public interface IActivitiesRepository
    {
        Task<IEnumerable<Activity>> GetActivitiesAsync();
        Task<Activity> GetActivityAsync(Guid id);
        Task CreateActivityAsync(Activity activity);
        Task UpdateActivityAsync(Activity activity);
        Task DeleteActivityAsync(Guid id);
    }
}