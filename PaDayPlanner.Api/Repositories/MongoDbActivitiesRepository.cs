using System.Collections.Generic;
using PaDayPlanner.Api.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace PaDayPlanner.Api.Repositories
{
    
    public class MongoDbActivitiesRepository : IActivitiesRepository
    {
        private const string databaseName = "padayplanner";
        private const string collectionName = "activities";

        private readonly IMongoCollection<Activity> activitiesCollection;
        private readonly FilterDefinitionBuilder<Activity> filterBuilder = Builders<Activity>.Filter;

        public MongoDbActivitiesRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            activitiesCollection = database.GetCollection<Activity>(collectionName);
        }

        public async Task<IEnumerable<Activity>> GetActivitiesAsync()
        {
            return await activitiesCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Activity> GetActivityAsync(Guid id)
        {
             var filter = filterBuilder.Eq(activity => activity.Id, id);
             return await activitiesCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task CreateActivityAsync(Activity activity)
        {
            await activitiesCollection.InsertOneAsync(activity);
        }

        public async Task UpdateActivityAsync(Activity activity)
        {
             var filter = filterBuilder.Eq(existingActivity => existingActivity.Id, activity.Id);
             await activitiesCollection.ReplaceOneAsync(filter, activity);
        }

        public async Task DeleteActivityAsync(Guid id)
        {
             var filter = filterBuilder.Eq(existingActivity => existingActivity.Id, id);
             await activitiesCollection.DeleteOneAsync(filter);
        }
    }
}