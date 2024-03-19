namespace UserAPI.Services;

public interface IGlobalCache
{
    void SetValue(string key, object value);

    T GetValue<T>(string key);
}