using NewsAPI.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace NewsAPI.Services
{
    public static class RedisHandler
    {
        private static string GetKeyPresentation(string key, int id) => $"{key}:{id}";
        public static Task<bool> AddToRedisAsync(IDatabase redisInstance, string key, DBEntry entity, int seconds = 600)
        {
            return redisInstance.StringSetAsync(GetKeyPresentation(key, entity.Id),
                JsonConvert.SerializeObject(entity), TimeSpan.FromSeconds(seconds));
        }

        public static Task<RedisValue> GetFromRedisAsync(IDatabase redisInstance, string key, int id)
        {
            return redisInstance.StringGetAsync(GetKeyPresentation(key, id));
        }

        public static Task<RedisValue> DeleteFromRedis(IDatabase redisInstance, string key, int id) 
        {
            return redisInstance.StringGetDeleteAsync(GetKeyPresentation(key, id));
        }
    }
}
