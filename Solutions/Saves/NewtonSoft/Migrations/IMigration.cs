#if INUI_NEWTONSOFT_JSON
using Newtonsoft.Json.Linq;

namespace DevKit.Saves.Migrations
{
    public interface IMigration
    {
        public int ToVersion { get; }
        public JObject Migrate(JObject dataObject);
    }
}
#endif