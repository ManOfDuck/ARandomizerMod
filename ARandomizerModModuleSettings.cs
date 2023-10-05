using Buttons = Microsoft.Xna.Framework.Input.Buttons;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Celeste.Mod.ARandomizerMod {
    public class ARandomizerModModuleSettings : EverestModuleSettings {

        [DefaultButtonBinding(Buttons.LeftShoulder, Keys.Tab)]
        public ButtonBinding OpenVariantsMenu { get; set; }

    }
}
 