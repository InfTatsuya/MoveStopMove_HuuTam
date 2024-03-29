using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private Vector2 moveDirection;
    public Vector2 MoveDirection => moveDirection;

    
    private Enemy target;
    public Enemy Target
    {
        get { return target; }
        set
        {
            target = value;
        }
    }

    private Dictionary<EAbilityType, AbilityBooster> abilityDict = new Dictionary<EAbilityType, AbilityBooster>();
    private GameObject weaponOrbitAbility;
    private GameObject shieldAbility;
    private GameObject invincibleAbility;

    #region States

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDeathState DeathState { get; private set; }
    public PlayerWinState WinState { get; private set; }

    #endregion

    protected override void OnInit()
    {
        base.OnInit();

        IdleState = new PlayerIdleState(this, Anim, StringCollection.IdleAnim);
        MoveState = new PlayerMoveState(this, Anim, StringCollection.MoveAnim);
        AttackState = new PlayerAttackState(this, Anim, StringCollection.AttackAnim);
        DeathState = new PlayerDeathState(this, Anim, StringCollection.DeathAnim);
        WinState = new PlayerWinState(this, Anim, StringCollection.DanceAnim);

        stateMachine.Initialize(IdleState);

        Debug.Log("OnInit player chay");
    }

    private void ShopSystem_onPurchaseSkin(object sender, ShopSystem.OnEquipSkinArgs e)
    {
        characterSkin.ChangeSkin(e.skinData, this);
    }

    private void ShopSystem_onPurchaseWeapon(object sender, ShopSystem.OnEquipWeaponArgs e)
    {
        ChangeWeapon(e.weaponData.weaponType);
    }

    private void JoyStick_onStickInputValueUpdated(object sender, JoyStick.OnStickInputValueUpdatedArg e)
    {
        moveDirection = e.inputVector;
    }

    [SerializeField] AbilityBooster testAbility;

    protected override void Start()
    {
        base.Start();

        JoyStick.onStickInputValueUpdated += JoyStick_onStickInputValueUpdated;
        ShopSystem.Instance.onEquipWeapon += ShopSystem_onPurchaseWeapon;
        ShopSystem.Instance.onEquipSkin += ShopSystem_onPurchaseSkin;
        ChangeWeapon(EWeaponType.Hammer);

        //AddAbility(testAbility); // for testing, TODO: remove before build
    }

    // for testing, TODO: remove before build
    private void TestRemoveAbbility()
    {
        RemoveAbility(testAbility);
    }

    // for testing, TODO: remove before build
    private void TestApplyAbbility()
    {
        ApplyAbility(testAbility);
    }

    protected override void Update()
    {
        base.Update();

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.y) > 0.1f)
        {
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.y));
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 30f);
        }

        transform.position += new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.y * moveSpeed) * Time.deltaTime;
    }

    protected override void OnDead(Character damageDealer)
    {
        base.OnDead(damageDealer);

        stateMachine.ChangeState(DeathState);
        moveDirection = Vector2.zero;

        if(GameManager.Instance != null)
        {
            UIManager.Instance.SwitchToRevivePanel();
            //GameManager.Instance.PauseGame();
        }     
        else if(EndlessGameMode.Instance != null)
        {
            EndlessGameMode.Instance.PauseGame();
            EndlessMode_UIManager.Instance.OpenGameOverPanel();
        }
    }

    public bool CheckHaveTargetAndInRange()
    {
        if(target == null)
        {
            return false;
        }

        if(Vector3.Distance(target.transform.position, transform.position) > attackRange)
        {
            return false;
        }

        if(target.CharacterStateMachine.CurrentState == target.DeathState)
        {
            return false;
        }

        return true;
    }

    public void ClearPreviousTarget()
    {
        if (target == null) return;

        target.SetEnemyAsTarget(false);
        target = null;
    }

    public override void OnNewGame()
    {
        SetCharacterName();
        health = maxHeath;
        transform.position = Vector3.zero;
        stateMachine.ChangeState(IdleState);

        RemoveAllAbility();
    }

    public void AddAbility(AbilityBooster abilityBooster)
    {
        if (abilityDict.ContainsKey(abilityBooster.abilityType))
        {
            abilityDict.TryGetValue(abilityBooster.abilityType, out var oldAbility);
            RemoveAbility(oldAbility);
            abilityDict.Remove(abilityBooster.abilityType);
        }

        abilityDict.Add(abilityBooster.abilityType, abilityBooster);
        ApplyAbility(abilityBooster);
    }

    private void RemoveAllAbility()
    {
        foreach(KeyValuePair<EAbilityType, AbilityBooster> keyValuePair in abilityDict)
        {
            RemoveAbility(keyValuePair.Value);
        }
    }

    private void ApplyAbility(AbilityBooster abilityBooster)
    {
        switch (abilityBooster.abilityType)
        {
            case EAbilityType.WeaponOrbit:
                if(weaponOrbitAbility == null)
                {
                    weaponOrbitAbility = Instantiate(abilityBooster.abilityPrefab);
                }
                weaponOrbitAbility.SetActive(true);
                weaponOrbitAbility.GetComponent<WeaponOrbit>().SetUpSkill(damage, abilityBooster.amt, this);
                break;

            case EAbilityType.MultipleThrow:
                projectileAmount = abilityBooster.amt;
                break;

            case EAbilityType.Shield:
                if(shieldAbility == null)
                {
                    shieldAbility = Instantiate(abilityBooster.abilityPrefab, transform);
                }
                shieldAbility.SetActive(true);
                shield = abilityBooster.amt;
                break;

            case EAbilityType.Invincible:
                if (invincibleAbility == null)
                {
                    invincibleAbility = Instantiate(abilityBooster.abilityPrefab, transform);
                }
                invincibleAbility.SetActive(true);
                isInvicible = true;
                StartCoroutine(AbilityRoutine(abilityBooster));
                break;


            default:
                break;
        }
    }

    private IEnumerator AbilityRoutine(AbilityBooster abilityBooster)
    {
        yield return new WaitForSeconds(abilityBooster.duration);

        RemoveAbility(abilityBooster);
    }

    protected override void OnShieldDestroy()
    {
        base.OnShieldDestroy();

        RemoveAbility(abilityDict[EAbilityType.Shield]);
    }

    protected override void SetCharacterName(string name = "Character_00")
    {
        if (GameManager.Instance == null) return;
        name = GameManager.Instance.PlayerName;
        base.SetCharacterName(name);
    }

    private void RemoveAbility(AbilityBooster abilityBooster)
    {
        switch (abilityBooster.abilityType)
        {
            case EAbilityType.WeaponOrbit:
                if (weaponOrbitAbility != null)
                {
                    weaponOrbitAbility.SetActive(false);
                }
                break;

            case EAbilityType.MultipleThrow:
                projectileAmount = 1;
                break;

            case EAbilityType.Shield:
                shieldAbility.SetActive(false);
                shield = 0;
                break;

            case EAbilityType.Invincible:
                invincibleAbility.SetActive(false);
                isInvicible = false;
                break;

            default:
                break;
        }
    }

    public void RevivePlayer()
    {
        health = 100;
        stateMachine.ChangeState(IdleState);

        GameManager.Instance.ResumeGame();
    }

    public void EndlessMode_Equip(EWeaponType weaponType, List<SkinData> skinDataList)
    {
        ChangeWeapon(weaponType);

        foreach(var skin in skinDataList)
        {     
            if(skin == null) continue;  
            characterSkin.ChangeSkin(skin, this);
        }
    }

    private void OnDestroy()
    {
        ShopSystem.Instance.onEquipWeapon -= ShopSystem_onPurchaseWeapon;
        ShopSystem.Instance.onEquipSkin -= ShopSystem_onPurchaseSkin;
    }
}
