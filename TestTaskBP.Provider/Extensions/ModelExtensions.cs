using Newtonsoft.Json;

namespace TestTaskBP.Provider.Extensions
{
    public static class ModelExtensions
    {
        public static string ToJson(this object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}