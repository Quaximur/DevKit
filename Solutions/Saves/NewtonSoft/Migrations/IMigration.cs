#if NEWTONSOFT_JSON
using Newtonsoft.Json.Linq;

namespace Inui.Core.Providers.SaveState.Migrations
{
    public interface IMigration
    {
        public int ToVersion { get; }
        public JObject Migrate(JObject dataObject);
    }
}
#endif