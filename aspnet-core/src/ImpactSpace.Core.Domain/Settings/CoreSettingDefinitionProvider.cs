using Volo.Abp.Settings;

namespace ImpactSpace.Core.Settings;

public class CoreSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(CoreSettings.MySetting1));
    }
}
