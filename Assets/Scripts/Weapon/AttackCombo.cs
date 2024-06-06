using UnityEngine;

public class AttackCombo : MonoBehaviour
{
    Animator animator;
    PlayerInputManager playerInputManager;
    WeaponSwitching weaponSwitching; // Reference to WeaponSwitching script
    AnimatorManager animatorManager; // Reference to AnimatorManager script
    public WeaponData weaponData;
    private int comboCount = 0;
    private float lastAttackTime = 0f;
    private float comboWindow;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerInputManager = GetComponent<PlayerInputManager>();

        // Find the WeaponSwitching component in the scene
        weaponSwitching = FindObjectOfType<WeaponSwitching>();
        if (weaponSwitching == null)
        {
            Debug.LogError("WeaponSwitching component not found in the scene.");
        }

        // Find the AnimatorManager component in the scene
        animatorManager = FindObjectOfType<AnimatorManager>();
        if (animatorManager == null)
        {
            Debug.LogError("AnimatorManager component not found in the scene.");
        }

        comboWindow = weaponData.comboWindow;
        Debug.Log("Combo window set to: " + comboWindow);
    }

    void Update()
    {
        // Ensure weaponSwitching and animatorManager are valid
        if (weaponSwitching == null || animatorManager == null) return;

        // Check if the Sword weapon is active
        if (weaponSwitching.selectedWeapon == 0) // Assuming Sword is at index 0
        {
            Debug.Log("Sword is active.");

            // Check for attack input
            if (playerInputManager.attackInput && !animator.GetBool("isInteracting"))
            {
                Debug.Log("Attack input detected.");

                // If within combo window and combo count is not maxed out, increment combo count
                if (Time.time - lastAttackTime < comboWindow && comboCount < 3)
                {
                    comboCount++;
                    Debug.Log("Combo count incremented to: " + comboCount);
                }
                else
                {
                    // Reset combo count if the combo window has passed or the max combo count is reached
                    comboCount = 1;  // Start a new combo
                    Debug.Log("Combo count reset to: " + comboCount);
                }

                // Update last attack time
                lastAttackTime = Time.time;

                // Update animator parameters
                UpdateAnimator();
            }
            // If combo window has passed and the combo count is maxed out, reset combo
            else if (Time.time - lastAttackTime > comboWindow && comboCount > 0)
            {
                comboCount = 0;
                Debug.Log("Combo window passed, combo count reset to 0.");
                UpdateAnimator();
            }
        }
        else
        {
            // Reset combo count if the weapon is switched
            if (comboCount > 0)
            {
                comboCount = 0;
                Debug.Log("Weapon switched, combo count reset to 0.");
                UpdateAnimator();
            }
        }
    }

    // Update animator parameters based on current combo state
    void UpdateAnimator()
    {
        Debug.Log("Updating Animator: isAttacking = " + (comboCount > 0) + ", ComboCount = " + comboCount);

        // Set the "isAttacking" parameter based on whether the combo count is greater than 0
        animator.SetBool("isAttacking", comboCount > 0);

        // Set the "ComboCount" parameter to the current combo count
        animator.SetInteger("ComboCount", comboCount);

        // If the combo count is 0, ensure that the "isAttacking" parameter is set to false
        if (comboCount == 0)
        {
            animator.SetBool("isAttacking", false);
        }
    }
}
