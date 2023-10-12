using Buttons = Microsoft.Xna.Framework.Input.Buttons;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Celeste.Mod.ARandomizerMod {
    public class ARandomizerModModuleSettings : EverestModuleSettings {

        [DefaultButtonBinding(Buttons.LeftShoulder, Keys.Tab)]
        public ButtonBinding OpenVariantsMenu { get; set; }

        [DefaultButtonBinding(Buttons.LeftThumbstickUp, Keys.Up)]
        public ButtonConfigUI NavigateUp { get; set; }

        [DefaultButtonBinding(Buttons.LeftThumbstickDown, Keys.Down)]
        public ButtonConfigUI NavigateDown { get; set; }

        public enum DifficultyOptions { EASY, NORMAL, HARD, IMPOSSIBLE };
        public DifficultyOptions Difficulty { get; set; }
    }
}
 