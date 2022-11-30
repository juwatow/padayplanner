using PaDayPlanner.Api.Entities;
using PaDayPlanner.Api.Dtos;

namespace PaDayPlanner.Api
{
    public static class Extensions
    {
        public static ActivityDto AsDto(this Activity activity)
        {
            return new ActivityDto (activity.Id, activity.Name, activity.Description, activity.BusinessOwner, activity.Price, activity.CreatedDate, activity.UpdatedDate);
        }
    }
}