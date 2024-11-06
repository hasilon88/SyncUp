using System;
using System.Collections.Generic;
using System.IO;

public class ConfigManager
{
    private Dictionary<string, string> _configDict = new();

    public ConfigManager(string path)
    {
        LoadConfig(path);
    }

    /// <summary>
    /// Reads the configuration from the specified file path.
    /// </summary>
    /// <param name="filePath">The path of the config file.</param>
    public void LoadConfig(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Config file not found at {filePath}");
        }

        var configLines = File.ReadAllLines(filePath);
        _configDict = ParseConfig(configLines);
    }

    /// <summary>
    /// Retrieves a configuration value by key as a string.
    /// </summary>
    /// <param name="key">The key of the config value.</param>
    /// <returns>The value associated with the key.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the key doesn't exist.</exception>
    public string GetString(string key)
    {
        if (_configDict.TryGetValue(key, out string value))
        {
            return value;
        }
        throw new KeyNotFoundException($"Key '{key}' not found in the configuration.");
    }

    /// <summary>
    /// Retrieves a configuration value by key as an integer.
    /// </summary>
    /// <param name="key">The key of the config value.</param>
    /// <returns>The integer value associated with the key.</returns>
    /// <exception cref="InvalidCastException">Thrown when the value cannot be converted to an integer.</exception>
    public int GetInt(string key)
    {
        string value = GetString(key);
        if (int.TryParse(value, out int result))
        {
            return result;
        }
        throw new InvalidCastException($"Value '{value}' for key '{key}' cannot be converted to an integer.");
    }

    /// <summary>
    /// Retrieves a configuration value by key as a float.
    /// </summary>
    /// <param name="key">The key of the config value.</param>
    /// <returns>The float value associated with the key.</returns>
    /// <exception cref="InvalidCastException">Thrown when the value cannot be converted to a float.</exception>
    public float GetFloat(string key)
    {
        string value = GetString(key);
        if (float.TryParse(value, out float result))
        {
            return result;
        }
        throw new InvalidCastException($"Value '{value}' for key '{key}' cannot be converted to a float.");
    }

    /// <summary>
    /// Retrieves a configuration value by key as a double.
    /// </summary>
    /// <param name="key">The key of the config value.</param>
    /// <returns>The double value associated with the key.</returns>
    /// <exception cref="InvalidCastException">Thrown when the value cannot be converted to a double.</exception>
    public double GetDouble(string key)
    {
        string value = GetString(key);
        if (double.TryParse(value, out double result))
        {
            return result;
        }
        throw new InvalidCastException($"Value '{value}' for key '{key}' cannot be converted to a double.");
    }

    /// <summary>
    /// Retrieves a configuration value by key as a long.
    /// </summary>
    /// <param name="key">The key of the config value.</param>
    /// <returns>The long value associated with the key.</returns>
    /// <exception cref="InvalidCastException">Thrown when the value cannot be converted to a long.</exception>
    public long GetLong(string key)
    {
        string value = GetString(key);
        if (long.TryParse(value, out long result))
        {
            return result;
        }
        throw new InvalidCastException($"Value '{value}' for key '{key}' cannot be converted to a long.");
    }

    /// <summary>
    /// Retrieves a configuration value by key as a decimal.
    /// </summary>
    /// <param name="key">The key of the config value.</param>
    /// <returns>The decimal value associated with the key.</returns>
    /// <exception cref="InvalidCastException">Thrown when the value cannot be converted to a decimal.</exception>
    public decimal GetDecimal(string key)
    {
        string value = GetString(key);
        if (decimal.TryParse(value, out decimal result))
        {
            return result;
        }
        throw new InvalidCastException($"Value '{value}' for key '{key}' cannot be converted to a decimal.");
    }

    /// <summary>
    /// Retrieves a configuration value by key as a boolean.
    /// </summary>
    /// <param name="key">The key of the config value.</param>
    /// <returns>The boolean value associated with the key.</returns>
    /// <exception cref="InvalidCastException">Thrown when the value cannot be converted to a boolean.</exception>
    public bool GetBool(string key)
    {
        string value = GetString(key);
        if (bool.TryParse(value, out bool result))
        {
            return result;
        }
        throw new InvalidCastException($"Value '{value}' for key '{key}' cannot be converted to a boolean.");
    }

    /// <summary>
    /// Parses the lines of the configuration file.
    /// </summary>
    /// <param name="configLines">Lines from the config file.</param>
    /// <returns>A dictionary containing the config key-value pairs.</returns>
    private Dictionary<string, string> ParseConfig(string[] configLines)
    {
        var parsedConfig = new Dictionary<string, string>();

        foreach (var line in configLines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("//"))
                continue;

            var keyValue = line.Split('=', 2);
            if (keyValue.Length == 2)
            {
                var key = keyValue[0].Trim();
                var value = keyValue[1].Trim();
                parsedConfig[key] = value;
            }
        }

        return parsedConfig;
    }
}

