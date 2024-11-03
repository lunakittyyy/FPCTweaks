using System.Text.Json.Serialization;

namespace FPCTweaks;

public class Config {
    [JsonInclude] public bool AutoLook = true;
}
