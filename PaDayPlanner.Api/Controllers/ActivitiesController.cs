using System;
using Microsoft.AspNetCore.Mvc;
using PaDayPlanner.Api.Repositories;
using PaDayPlanner.Api.Dtos;
using PaDayPlanner.Api.Entities;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PaDayPlanner.Api.Controllers
{
   // GET /activities

   [ApiController]
   [Route("activities")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivitiesRepository repository;
        private readonly ILogger<ActivitiesController> logger;

        public ActivitiesController(IActivitiesRepository repository, ILogger<ActivitiesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // GET /activities
        [HttpGet]
        public async Task<IEnumerable<ActivityDto>> GetActivitiesAsync(string name = null)
        {
            var activities = (await repository.GetActivitiesAsync()).Select( activity => activity.AsDto());

            if(!string.IsNullOrWhiteSpace(name)) {
                activities = activities.Where(item => item.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {activities.Count()} activities");
            
            return activities;
        }

       // GET /activities/{id}
       [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetActivityAsync(Guid id)
        {
            var activity = await repository.GetActivityAsync(id);
            if(activity is null)
            {
                return NotFound();
            }

            return activity.AsDto();
        }

        // POST /activities
       [HttpPost]
        public async Task<ActionResult<ActivityDto>> CreateActivityAsync(CreateActivityDto activityDto)
        {
            Activity activity = new() { 
                Id = Guid.NewGuid(),
                Name = activityDto.Name,
                Description = activityDto.Description,
                BusinessOwner = activityDto.BusinessOwner,
                Price = activityDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateActivityAsync(activity);

            return CreatedAtAction(nameof(GetActivityAsync), new { id = activity.Id }, activity.AsDto());
        }

        // PUT /activities/{id}
       [HttpPut("{id}")]
        public async Task<ActionResult> UpdateActivityAsync(Guid id, UpdateActivityDto activityDto)
        {
            var existingActivity = await repository.GetActivityAsync(id);

            if(existingActivity is null)
            {
                return NotFound();
            }
            
            existingActivity.Name = activityDto.Name;
            existingActivity.Description = activityDto.Description;
            existingActivity.BusinessOwner = activityDto.BusinessOwner;
            existingActivity.Price = activityDto.Price;
            existingActivity.UpdatedDate = DateTimeOffset.UtcNow;

            await repository.UpdateActivityAsync(existingActivity);

            return NoContent();
        }

         // DELETE /activities/{id}
       [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteActivityAsync(Guid id)
        {
            var existingActivity = await repository.GetActivityAsync(id);

            if(existingActivity is null)
            {
                return NotFound();
            }

            await repository.DeleteActivityAsync(id);

            return NoContent();
        }

    }
}