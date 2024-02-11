using System;

namespace Celeste.Mod.ARandomizerMod
{
	public class VariantLists
	{
		public static readonly Variant[] great = {
			//Gravity
			//FallSpeed
			//JumpHeight
            new FloatVariant("JumpDuration", 3.0f, 100f, 1f, Variant.Level.GREAT),
			//WallBouncingSpeed
			//DisableWallJumping

			new IntegerVariant("JumpCount", int.MaxValue, int.MaxValue, 1, Variant.Level.GREAT),
			new IntegerVariant("DashCount", 5, 5, -1, Variant.Level.GREAT),
			new BooleanVariant("HeldDash", true, Variant.Level.GREAT),

        };

		public static readonly Variant[] good = {
			//Gravity
			//FallSpeed
			new FloatVariant("JumpHeight", 1.5f, 2.5f, 1.0f, Variant.Level.GOOD),
			new FloatVariant("JumpDuration", 2.0f, 2.9f, 1.0f, Variant.Level.GOOD),
			new FloatVariant("WallBouncingSpeed", 2.0f, 2.9f, 1.0f, Variant.Level.GOOD),

			new FloatVariant("HorizontalWallJumpDuration", 0.0f, 0.0f, 1.0f, Variant.Level.GOOD),
			new FloatVariant("HorizontalWallJumpDuration", 3.0f, 5.0f, 1.0f, Variant.Level.GOOD),

			new IntegerVariant("JumpCount", 2, 5, -1, Variant.Level.GOOD),
			new FloatVariant("CoyoteTime", 100f, 100f, 1.0f, Variant.Level.GOOD),

			new FloatVariant("DashLength", 2.0f, 3.0f, 1.0f, Variant.Level.GOOD),
			 
			new BooleanVariant("EverythingIsUnderwater", true, Variant.Level.GOOD),

			new IntegerVariant("Stamina", 230, 500, 110, Variant.Level.GOOD),
			new IntegerVariant("JellyfishEverywhere", 1, 1, 0, Variant.Level.GOOD),
        };

		public static readonly Variant[] nice = {
			new FloatVariant("Gravity", 0.3f, 0.9f, 1.0f, Variant.Level.NICE),
			new FloatVariant("FallSpeed", 0.4f, 1.0f, 1.0f, Variant.Level.NICE),
			//JumpHeight
			//JumpDuration
			//WallBouncingSpeed

			new FloatVariant("CoyoteTime", 2.0f, 5.0f, 1.0f, Variant.Level.NICE),
			new FloatVariant("SpeedX", 1.1f, 2.0f, 1.0f, Variant.Level.NICE),

			//new Variant("WindEverywhere", 6, 6, 0, Variant.Level.NICE),
			new IntegerVariant("WindEverywhere", 13, 13, 0, Variant.Level.NICE),

			new IntegerVariant("Stamina", 120, 220, 110, Variant.Level.NICE),
        };

		public static readonly Variant[] dubious = {
			new FloatVariant("Gravity", 0.0f, 0.2f, 1.0f, Variant.Level.DUBIOUS),
			new FloatVariant("FallSpeed", 0.0f, 0.3f, 1.0f, Variant.Level.DUBIOUS),
			new FloatVariant("JumpHeight", 3.0f, 100f,1.0f, Variant.Level.DUBIOUS),
			//JumpDuration
			new FloatVariant("WallBouncingSpeed", 3.0f, 100f, 1.0f, Variant.Level.DUBIOUS),

			new FloatVariant("HorizontalWallJumpDuration", 10.0f, 100f, 1.0f, Variant.Level.DUBIOUS),

			new FloatVariant("SpeedX", 3.0f, 100f, 1.0f, Variant.Level.DUBIOUS),
			new FloatVariant("AirFriction", 0.0f, 0.5f, 1.0f, Variant.Level.DUBIOUS),
			new FloatVariant("AirFriction", 100f, 100f, 1.0f, Variant.Level.DUBIOUS),
			new FloatVariant("WallSlidingSpeed", 100f, 100f, 1.0f, Variant.Level.DUBIOUS),

			new FloatVariant("GameSpeed", 0.1f, 0.9f, 1.0f, Variant.Level.DUBIOUS),
			new FloatVariant("GameSpeed", 1.1f, 5.0f, 1.0f, Variant.Level.DUBIOUS),

			//new Variant("HiccupStrength", 3.0f, 5.0f, 1.0f, Variant.Level.DUBIOUS),

			//new Variant("CorrectedMirrorMode", true, Variant.Level.DUBIOUS),

        };

		public static readonly Variant[] tame = {
			new FloatVariant("Gravity", 1.1f, 2.0f, 1.0f, Variant.Level.TAME),
			new FloatVariant("FallSpeed", 1.1f, 10f, 1.0f, Variant.Level.TAME),
			new FloatVariant("JumpHeight", 0.7f, 0.9f, 1.0f, Variant.Level.TAME),
			//JumpDuration
			//WallBouncingSpeed

			new BooleanVariant("DisableJumpingOutOfWater", true, Variant.Level.TAME),
			new BooleanVariant("DisableNeutralJumping", true, Variant.Level.TAME),

			new FloatVariant("DashLength", 0.5f, 0.8f, 1.0f, Variant.Level.TAME),

			new BooleanVariant("DontRefillStaminaOnGround", true, Variant.Level.TAME),

			new FloatVariant("Friction", 0.1f, 0.3f, 1.0f, Variant.Level.TAME),
			new IntegerVariant("DisableClimbingUpOrDown", 1, 3, 0, Variant.Level.TAME),

			new IntegerVariant("ChaserCount", 3, 10, 0, Variant.Level.TAME),

            new IntegerVariant("AddSeekers", 1, 1, 0, Variant.Level.TAME),
			new IntegerVariant("WindEverywhere", 2, 3, 0, Variant.Level.TAME),
            //new Variant("WindEverywhere", 7, 9, 0, Variant.Level.TAME),
            //new Variant("WindEverywhere", 12, 12, 0, Variant.Level.TAME),

			new IntegerVariant("Stamina", 50, 100, 110, Variant.Level.TAME),

			new FloatVariant("RegularHiccups", 1.0f, 5.0f, 0.0f, Variant.Level.TAME),
			new FloatVariant("RoomLighting", 0.0f, 0.0f, 1.0f, Variant.Level.TAME), 

			new FloatVariant("GlitchEffect", 0.05f, 0.1f, 0.0f, Variant.Level.TAME),

			new FloatVariant("BlurLevel", 0.1f, 0.7f, 0.0f, Variant.Level.TAME),

			new FloatVariant("ZoomLevel", 0.1f, 1.9f, 1.0f, Variant.Level.TAME),
        };

		public static readonly Variant[] nasty = {
			new FloatVariant("Gravity", 2.1f, 100f, 1.0f, Variant.Level.NASTY),
			new FloatVariant("FallSpeed", 50f, 100f, 1.0f, Variant.Level.NASTY),
			new FloatVariant("JumpHeight", 0.3f, 0.6f, 1.0f, Variant.Level.NASTY),
			//JumpDuration
			//WallBouncingSpeed

			new BooleanVariant("DisableWallJumping", true, Variant.Level.NASTY),
			new BooleanVariant("DisableClimbJumping", true, Variant.Level.NASTY),

			//TODO: Dash Direction would be cool

			new IntegerVariant("DontRefillDashOnGround", 1, 1, 0, Variant.Level.NASTY),

			new BooleanVariant("BadelineChasersEverywhere", true, Variant.Level.NASTY),
			new BooleanVariant("BadelineBossesEverywhere", true, Variant.Level.NASTY),
			new BooleanVariant("OshiroEverywhere", true, Variant.Level.NASTY,
				subVariants: new Variant[]
				{
				new IntegerVariant("OshiroCount", 1, 2, 0, Variant.Level.SUB),
				new IntegerVariant("ReverseOshiroCount", 0, 0, 0, Variant.Level.SUB)
				}),
            new BooleanVariant("OshiroEverywhere", true, Variant.Level.NASTY,
				subVariants: new Variant[]
				{
                new IntegerVariant("OshiroCount", 0, 0, 0, Variant.Level.SUB),
                new IntegerVariant("ReverseOshiroCount", 1, 2, 0, Variant.Level.SUB)
				}),
            new BooleanVariant("OshiroEverywhere", true, Variant.Level.NASTY,
				subVariants: new Variant[]
				{
                new IntegerVariant("OshiroCount", 1, 1, 0, Variant.Level.SUB),
                new IntegerVariant("ReverseOshiroCount", 1, 1, 0, Variant.Level.SUB)
				}),
            //new IntegerVariant("WindEverywhere", 4, 5, 0, Variant.Level.NASTY),
			new IntegerVariant("WindEverywhere", 10, 11, 0, Variant.Level.NASTY),
            //new IntegerVariant("WindEverywhere", 14, 14, 0, Variant.Level.NASTY),
			new BooleanVariant("SnowballsEverywhere", true, Variant.Level.NASTY,
				subVariants: new Variant[]
				{
                    new FloatVariant("SnowballDelay", 0.0f, 2.0f, 0.8f, Variant.Level.SUB)
                }),
            new FloatVariant("RegularHiccups", 0.3f, 0.6f, 0.0f, Variant.Level.DUBIOUS),

            new IntegerVariant("AddSeekers", 2, 2, 0, Variant.Level.NASTY),

			new BooleanVariant("RisingLavaEverywhere", true, Variant.Level.NASTY,
				subVariants: new Variant[]
				{
                    new FloatVariant("RisingLavaSpeed", 0.8f, 2.0f, 1.0f, Variant.Level.SUB)
                }),

			new IntegerVariant("Stamina", 0, 49, 110, Variant.Level.NASTY),
			new BooleanVariant("AllStrawberriesAreGoldens", true, Variant.Level.NASTY),
			new BooleanVariant("AlwaysInvisible", true, Variant.Level.NASTY),
			new BooleanVariant("ForceDuckOnGround", true, Variant.Level.NASTY),

			new BooleanVariant("UpsideDown", true, Variant.Level.NASTY),

			//new Variant("GlitchEffect", 0.1f, 0.5f, 0.0f, Variant.Level.NASTY),
			new FloatVariant("ZoomLevel", 2.0f, 2.9f, 1.0f, Variant.Level.NASTY),
            new BooleanVariant("BounceEverywhere", true, Variant.Level.FUCKED_UP),
        };

		public static readonly Variant[] FUCKED_UP = {
			//Gravity
			//FallSpeed
			new FloatVariant("JumpHeight", 0.0f, 0.2f, 1.0f, Variant.Level.FUCKED_UP),
			//JumpDuration
			//WallBouncingSpeed

			new IntegerVariant("JumpCount",  0,  0, -1, Variant.Level.FUCKED_UP),

			new FloatVariant("DashLength", 0.0f, 0.1f, 1.0f, Variant.Level.FUCKED_UP),
			new IntegerVariant("DashCount", 0, 0, -1, Variant.Level.FUCKED_UP),

			new FloatVariant("AddSeekers", 2, 2, 0, Variant.Level.FUCKED_UP),
			new BooleanVariant("TheoCrystalsEverywhere", true, Variant.Level.FUCKED_UP),
			new BooleanVariant("InvertDashes", true, Variant.Level.FUCKED_UP),
			new BooleanVariant("InvertVerticalControls", true, Variant.Level.FUCKED_UP),
			
			new FloatVariant("ZoomLevel", 3.0f, 5.0f, 1.0f, Variant.Level.FUCKED_UP),
		};


        public static readonly Variant[] silly = {
			new BooleanVariant("MadelineIsSilhouette", true, Variant.Level.SILLY),
			new BooleanVariant("DashTrailAllTheTime", true, Variant.Level.SILLY),
			new BooleanVariant("DisplayDashCount", true, Variant.Level.SILLY),
			new IntegerVariant("DisplaySpeedometer", 2, 2, 0, Variant.Level.SILLY),
			new FloatVariant("BackgroundBrightness", -1.0f, 5.0f, 1.0f, Variant.Level.SILLY),
			new FloatVariant("RoomBloom", -1.0f, 5.0f, 0.0f, Variant.Level.SILLY),

			new FloatVariant("AnxietyEffect", -1.0f, 10.0f, 0.0f,  Variant.Level.SILLY),
			new FloatVariant("BackgroundBlurLevel", -1.0f, 10.0f, 0.0f, Variant.Level.SILLY),

			//TODO: Color Grading would be awesome

			new IntegerVariant("SpinnerColor", 3, 3, 0, Variant.Level.SILLY),
			new BooleanVariant("FriendlyBadelineFollower", true, Variant.Level.SILLY)
		};
    }
}

