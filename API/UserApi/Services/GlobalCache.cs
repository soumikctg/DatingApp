namespace UserAPI.Services;

public class GlobalCache : IGlobalCache
{
    private readonly Dictionary<string, object> _cache = new();

    public void SetValue(string key, object value)
    {
        _cache[key] = value;
    }

    public T GetValue<T>(string key)
    {
        _cache.TryGetValue(key, out var value);

        return (T)value;
    }
}