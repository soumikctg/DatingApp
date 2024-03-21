namespace UserAPI.Interfaces;

public interface IGlobalCache
{
    void SetValue(string key, object value);

    T GetValue<T>(string key);
}