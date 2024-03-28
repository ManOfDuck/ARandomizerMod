using Buttons = Microsoft.Xna.Framework.Input.Buttons;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Celeste.Mod.ARandomizerMod {
    public class ARandomizerModModuleSettings : EverestModuleSettings {

        [DefaultButtonBinding(Buttons.LeftShoulder, Keys.V)]
        public ButtonBinding OpenVariantsMenu { get; set; }

        [DefaultButtonBinding(Buttons.LeftThumbstickUp, Keys.Up)]
        public ButtonBinding NavigateUp { get; set; }

        [DefaultButtonBinding(Buttons.LeftThumbstickDown, Keys.Down)]
        public ButtonBinding NavigateDown { get; set; }

        [DefaultButtonBinding(Buttons.A, Keys.C)]
        public ButtonBinding Select { get; set; }

        public enum DifficultyOptions { EASY };
        public DifficultyOptions Difficulty { get; set; }
    }
}
 