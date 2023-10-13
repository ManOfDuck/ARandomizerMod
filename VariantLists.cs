using System;

namespace Celeste.Mod.ARandomizerMod
{
	public class VariantLists
	{
		public static Variant[] great = {
			//Gravity
			//FallSpeed
			//JumpHeight
            new Variant("JumpDuration", 3.0f, 100f, 1f, Variant.Level.GREAT),
			//WallBouncingSpeed
			//DisableWallJumping

			new Variant("JumpCount", int.MaxValue, int.MaxValue, 1, Variant.Level.GREAT),
			new Variant("DashCount", 5, 5, -1, Variant.Level.GREAT),
			new Variant("HeldDash", true, Variant.Level.GREAT),

        };

		public static Variant[] good = {
			//Gravity
			//FallSpeed
			new Variant("JumpHeight", 1.5f, 2.5f, 1.0f, Variant.Level.GOOD),
			new Variant("JumpDuration", 2.0f, 2.9f, 1.0f, Variant.Level.GOOD),
			new Variant("WallBouncingSpeed", 2.0f, 2.9f, 1.0f, Variant.Level.GOOD),

			new Variant("HorizontalWallJumpDuration", 0.0f, 0.0f, 1.0f, Variant.Level.GOOD),
			new Variant("HorizontalWallJumpDuration", 3.0f, 5.0f, 1.0f, Variant.Level.GOOD),

			new Variant("JumpCount", 2, 5, -1, Variant.Level.GOOD),
			new Variant("CoyoteTime", 100f, 100f, 1.0f, Variant.Level.GOOD),

			new Variant("DashLength", 2.0f, 3.0f, -1.0f, Variant.Level.GOOD),

			new Variant("EverythingIsUnderwater", true, Variant.Level.GOOD),

			new Variant("Stamina", 230, 500, 110, Variant.Level.GOOD),
        };

		public static Variant[] nice = {
			new Variant("Gravity", 0.3f, 0.9f, 1.0f, Variant.Level.NICE),
			new Variant("FallSpeed", 0.4f, 1.0f, 1.0f, Variant.Level.NICE),
			//JumpHeight
			//JumpDuration
			//WallBouncingSpeed

			new Variant("CoyoteTime", 2.0f, 5.0f, 1.0f, Variant.Level.NICE),
			new Variant("SpeedX", 1.1f, 2.0f, 1.0f, Variant.Level.NICE),

			new Variant("WindEverywhere", 6, 6, 0, Variant.Level.NICE),
			new Variant("WindEverywhere", 13, 13, 0, Variant.Level.NICE),

			new Variant("JellyfishEverywhere", 1, 3, 0, Variant.Level.NICE),

			new Variant("Stamina", 120, 220, 110, Variant.Level.NICE),
        };

		public static Variant[] dubious = {
			new Variant("Gravity", 0.0f, 0.2f, 1.0f,Variant.Level.DUBIOUS),
			new Variant("FallSpeed", 0.0f, 0.3f, 1.0f,Variant.Level.DUBIOUS),
			new Variant("JumpHeight", 3.0f, 100f,1.0f, Variant.Level.DUBIOUS),
			//JumpDuration
			new Variant("WallBouncingSpeed", 3.0f, 100f, 1.0f, Variant.Level.DUBIOUS),

			new Variant("HorizontalWallJumpDuration", 10.0f, 100f, 1.0f, Variant.Level.DUBIOUS),

			new Variant("SpeedX", 3.0f, 100f, 1.0f, Variant.Level.DUBIOUS),
			new Variant("AirFriction", 0.0f, 0.5f, 1.0f, Variant.Level.DUBIOUS),
			new Variant("AirFriction", 100f, 100f, 1.0f, Variant.Level.DUBIOUS),
			new Variant("WallSlidingSpeed", 100f, 100f, 1.0f, Variant.Level.DUBIOUS),

			new Variant("GameSpeed", 0.1f, 0.9f, 1.0f, Variant.Level.DUBIOUS),
			new Variant("GameSpeed", 1.1f, 5.0f, 1.0f, Variant.Level.DUBIOUS),

			//new Variant("HiccupStrength", 3.0f, 5.0f, 1.0f, Variant.Level.DUBIOUS),

			//new Variant("CorrectedMirrorMode", true, Variant.Level.DUBIOUS),

        };

		public static Variant[] tame = {
			new Variant("Gravity", 1.1f, 2.0f, 1.0f, Variant.Level.TAME),
			new Variant("FallSpeed", 1.1f, 10f, 1.0f, Variant.Level.TAME),
			new Variant("JumpHeight", 0.7f, 0.9f, 1.0f, Variant.Level.TAME),
			//JumpDuration
			//WallBouncingSpeed

			new Variant("DisableJumpingOutOfWater", true, Variant.Level.TAME),
			new Variant("DisableNeutralJumping", true, Variant.Level.TAME),

			new Variant("DashLength", 0.5f, 0.8f, 1.0f, Variant.Level.TAME),

			new Variant("DontRefillStaminaOnGround", true, Variant.Level.TAME),

			new Variant("Friction", 0.1f, 0.3f, 1.0f, Variant.Level.TAME),
			new Variant("DisableClimbingUpOrDown", 1, 3, 0, Variant.Level.TAME),

			new Variant("ChaserCount", 3, 10, 0, Variant.Level.TAME),

            new Variant("AddSeekers", 1, 1, 0, Variant.Level.TAME),
			new Variant("WindEverywhere", 2, 3, 0, Variant.Level.TAME),
            new Variant("WindEverywhere", 7, 9, 0, Variant.Level.TAME),
            new Variant("WindEverywhere", 12, 12, 0, Variant.Level.TAME),

			new Variant("Stamina", 50, 100, 110, Variant.Level.TAME),

			new Variant("RegularHiccups", 1.0f, 5.0f, 0.0f, Variant.Level.TAME),
			new Variant("RoomLighting", 0.0f, 0.0f, 1.0f, Variant.Level.TAME),

			new Variant("GlitchEffect", 0.05f, 0.49f, 0.0f, Variant.Level.TAME),

			new Variant("BlurLevel", 1.0f, 10.0f, 0.0f, Variant.Level.TAME),

			new Variant("ZoomLevel", 0.1f, 1.9f, 1.0f, Variant.Level.TAME),
        };

		public static Variant[] nasty = {
			new Variant("Gravity", 2.1f, 100f, 1.0f, Variant.Level.NASTY),
			new Variant("FallSpeed", 50f, 100f, 1.0f, Variant.Level.NASTY),
			new Variant("JumpHeight", 0.3f, 0.6f, 1.0f, Variant.Level.NASTY),
			//JumpDuration
			//WallBouncingSpeed

			new Variant("DisableWallJumping", true, Variant.Level.NASTY),
			new Variant("DisableClimbJumping", true, Variant.Level.NASTY),

			//TODO: Dash Direction would be cool

			new Variant("DontRefillDashOnGround", 1, 1, 0, Variant.Level.NASTY),

			new Variant("BadelineChasersEverywhere", true, Variant.Level.NASTY),
			new Variant("BadelineBossesEverywhere", true, Variant.Level.NASTY),
            new Variant("OshiroEverywhere", true,
				new Variant(
                new Variant("OshiroCount", 1, 2, 0, Variant.Level.SUB),
                new Variant("ReverseOshiroCount", 0, 0, 0, Variant.Level.SUB)), Variant.Level.NASTY),
            new Variant("OshiroEverywhere", true,
				new Variant(
				new Variant("OshiroCount", 0, 0, 0, Variant.Level.SUB),
				new Variant("ReverseOshiroCount", 1, 2, 0, Variant.Level.SUB)), Variant.Level.NASTY),
            new Variant("OshiroEverywhere", true,
				new Variant(
                new Variant("OshiroCount", 1, 1, 0, Variant.Level.SUB),
                new Variant("ReverseOshiroCount", 1, 1, 0, Variant.Level.SUB)), Variant.Level.NASTY),

            new Variant("WindEverywhere", 4, 5, 0, Variant.Level.NASTY),
			new Variant("WindEverywhere", 10, 11, 0, Variant.Level.NASTY),
            new Variant("WindEverywhere", 14, 14, 0, Variant.Level.NASTY),
			new Variant("SnowballsEverywhere", true,
				new Variant("SnowballDelay", 0.0f, 2.0f, 0.8f, Variant.Level.SUB), Variant.Level.NASTY),

            new Variant("RegularHiccups", 0.3f, 0.6f, 0.0f, Variant.Level.DUBIOUS),

            new Variant("AddSeekers", 2, 2, 0, Variant.Level.NASTY),

			new Variant("RisingLavaEverywhere", true,
				new Variant("RisingLavaSpeed", 0.8f, 2.0f, 1.0f, Variant.Level.SUB), Variant.Level.NASTY),

			new Variant("Stamina", 0, 49, 110, Variant.Level.NASTY),
			new Variant("AllStrawberriesAreGoldens", true, Variant.Level.NASTY),
			new Variant("AlwaysInvisible", true, Variant.Level.NASTY),
			new Variant("ForceDuckOnGround", true, Variant.Level.NASTY),

			new Variant("UpsideDown", true, Variant.Level.NASTY),

			new Variant("GlitchEffect", 0.5f, 1.0f, 0.0f, Variant.Level.NASTY),
			new Variant("ZoomLevel", 2.0f, 2.9f, 1.0f, Variant.Level.NASTY),

        };

		public static Variant[] FUCKED_UP = {
			//Gravity
			//FallSpeed
			new Variant("JumpHeight", 0.0f, 0.2f, 1.0f, Variant.Level.FUCKED_UP),
			//JumpDuration
			//WallBouncingSpeed

			new Variant("JumpCount",  0,  0, -1, Variant.Level.FUCKED_UP),

			new Variant("DashLength", 0.0f, 0.1f, 1.0f, Variant.Level.FUCKED_UP),
			new Variant("DashCount", 0, 0, -1, Variant.Level.FUCKED_UP),

			new Variant("AddSeekers", 3, 3, 0, Variant.Level.FUCKED_UP),
			new Variant("TheoCrystalsEverywhere", true, Variant.Level.FUCKED_UP),
			new Variant("InvertDashes", true, Variant.Level.FUCKED_UP),
			new Variant("InvertVerticalControls", true, Variant.Level.FUCKED_UP),
			new Variant("BounceEverywhere", true, Variant.Level.FUCKED_UP),
			new Variant("ZoomLevel", 3.0f, 5.0f, 1.0f, Variant.Level.FUCKED_UP),
		};


        public static Variant[] silly = {
			new Variant("MadelineIsSilhouette", true, Variant.Level.SILLY),
			new Variant("DashTrailAllTheTime", true, Variant.Level.SILLY),
			new Variant("DisplayDashCount", true, Variant.Level.SILLY),
			new Variant("DisplaySpeedometer", 2, 2, 0, Variant.Level.SILLY),
			new Variant("BackgroundBrightness", 0.0f, 9.0f, 1.0f, Variant.Level.SILLY),
			new Variant("RoomBloom", 0.0f, 50.0f, 0.0f, Variant.Level.SILLY),

			new Variant("AnxietyEffect", 0.1f, 10.0f, 0.0f,  Variant.Level.SILLY),
			new Variant("BackgroundBlurLevel", 0.1f, 10.0f, 0.0f, Variant.Level.SILLY),

			//TODO: Color Grading would be awesome

			new Variant("SpinnerColor", 3, 3, 0, Variant.Level.SILLY),
			new Variant("FriendlyBadelineFollower", true, Variant.Level.SILLY)
		};
    }
}

