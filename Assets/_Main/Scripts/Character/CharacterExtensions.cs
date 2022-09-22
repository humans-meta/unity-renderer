using _Main.Scripts;

public static class CharacterExtensions {
    public static void SetPhysics(this UltimateCharacterLocomotion loco, bool active) {
        // loco.DetectHorizontalCollisions = active;
        // loco.DetectVerticalCollisions = active;
        // loco.UseGravity = active;
    }

    public static void SetInput(this UltimateCharacterLocomotion loco, bool active) {
        EventHandler.ExecuteEvent(loco.gameObject, "OnEnableGameplayInput", active);
    }
}