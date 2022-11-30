using System.Collections.Generic;
using PaDayPlanner.Api.Entities;
using System.Threading.Tasks;

namespace PaDayPlanner.Api.Repositories
{
    
    public class InMemActivitiesRepository : IActivitiesRepository
    {
        private readonly List<Activity> activities = new()
        {
            new Activity { Id = Guid.NewGuid(), Name = "A", BusinessOwner = "Owner A", Price = 300 },
            new Activity { Id = Guid.NewGuid(), Name = "B", BusinessOwner = "Owner B", Price = 400 },
            new Activity { Id = Guid.NewGuid(), Name = "C", BusinessOwner = "Owner C", Price = 200 }
        };

        public async Task<IEnumerable<Activity>> GetActivitiesAsync()
        {
            return await Task.FromResult(activities);
        }

        public async Task<Activity> GetActivityAsync(Guid id)
        {
            var activity = activities.Where(activity => activity.Id == id).SingleOrDefault();
            return await Task.FromResult(activity);
        }

        public async Task CreateActivityAsync(Activity activity)
        {
            activities.Add(activity);
            await Task.CompletedTask;
        }

        public async Task UpdateActivityAsync(Activity activity)
        {
            var index = activities.FindIndex(existingActivity => existingActivity.Id == activity.Id);
            activities[index] = activity;
            await Task.CompletedTask;
        }

        public async Task DeleteActivityAsync(Guid id)
        {
            var index = activities.FindIndex(existingActivity => existingActivity.Id == id);
            activities.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}