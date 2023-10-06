using System;

namespace Celeste.Mod.ARandomizerMod
{
	public class VariantLists
	{
		Variant[] great = {
			//Gravity
			//FallSpeed
			//JumpHeight
            new Variant("JumpDuration", 3.0f, 100f),
			//WallBouncingSpeed
			//DisableWallJumping

			new Variant("JumpCount", int.MaxValue, int.MaxValue),
			new Variant("DashCount", 5, 5),
			new Variant("HeldDash", true),

        };

		Variant[] good = {
			//Gravity
			//FallSpeed
			new Variant("JumpHeight", 1.5f, 2.5f),
			new Variant("JumpDuration", 2.0f, 2.9f),
			new Variant("WallBouncingSpeed", 2.0f, 2.9f),

			new Variant("HorizontalWallJumpDuration", 0.0f, 0.0f),
			new Variant("HorizontalWallJumpDuration", 3.0f, 5.0f),

			new Variant("JumpCount", 2, 5),
			new Variant("CoyoteTime", 100f, 100f),

			new Variant("DashCount", 2, 3),

        };

		Variant[] nice = {
			new Variant("Gravity", 0.3f, 0.9f),
			new Variant("FallSpeed", 0.4f, 1.0f),
			//JumpHeight
			//JumpDuration
			//WallBouncingSpeed

			new Variant("CoyoteTime", 2.0f, 5.0f),
			new Variant("SpeedX", 1.1f, 2.0f),


		};

		Variant[] dubious = {
			new Variant("Gravity", 0.0f, 0.2f),
			new Variant("FallSpeed", 0.0f, 0.3f),
			new Variant("JumpHeight", 3.0f, 100f),
			//JumpDuration
			new Variant("WallBouncingSpeed", 3.0f, 100f),

			new Variant("HorizontalWallJumpDuration", 10.0f, 100f),

			new Variant("SpeedX", 3.0f, 100f),
			new Variant("AirFriction", 0.0f, 0.5f),
			new Variant("AirFriction", 100f, 100f),
			new Variant("WallSlidingSpeed", 100f, 100f),

        };

		Variant[] tame = {
			new Variant("Gravity", 1.1f, 2.0f),
			new Variant("FallSpeed", 1.1f, 10f),
			new Variant("JumpHeight", 0.7f, 0.9f),
			//JumpDuration
			//WallBouncingSpeed

			new Variant("DisableJumpingOutOfWater", true),
			new Variant("DisableNeutralJumping", true),

			new Variant("DashDuration", 0.5f, 0.8f),

			new Variant("AddSeekers", 1, 1),
			new Variant("DontRefillStaminaOnGround", true),

			new Variant("Friction", 0.1f, 0.3f),
			new Variant("DisableClimbingUpOrDown", 1, 3),

			new Variant("ChaserCount", 3, 10),
		};

		Variant[] nasty = {
			new Variant("Gravity", 2.1f, 100f),
			new Variant("FallSpeed", 50f, 100f),
			new Variant("JumpHeight", 0.3f, 0.6f),
			//JumpDuration
			//WallBouncingSpeed

			new Variant("DisableWallJumping", true),
			new Variant("DisableClimbJumping", true),

			//Dash Direction would be cool

			new Variant("DontRefillDashOnGround", true),

			new Variant("BadelineChasersEverywhere", true),
			new Variant("BadelineBossesEverywhere", true),
			new Variant("OshiroEverywhere", true),

			new Variant("AddSeekers", 2, 2),

        };

		Variant[] FUCKED_UP = {
			//Gravity
			//FallSpeed
			new Variant("JumpHeight", 0.0f, 0.2f),
			//JumpDuration
			//WallBouncingSpeed

			new Variant("JumpCount", 0, 0),

			new Variant("DashDuration", 0.0f, 0.1f),
			new Variant("DashCount", 0, 0),

			new Variant("AddSeekers", 3, 3),
			new Variant("TheoCrystalsEverywhere", true),

		};


        Variant[] silly = { };
    }
}

