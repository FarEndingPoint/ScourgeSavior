using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using DG.Tweening;

public class PlayerBeHitedInvincibleCommand : AbstractCommand
{
    SpriteRenderer spriteRenderer;
    float flashAlpha;
    bool canFade;

    public PlayerBeHitedInvincibleCommand(SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;
        flashAlpha = 0.2f;
        canFade = true;
    }

    protected override void OnExecute()
    {
        if (Main.Interface.GetModel<PlayerModel>().curBeHitedInvincibleTime > 0)
        {
            Main.Interface.GetModel<PlayerModel>().curBeHitedInvincibleTime -= Time.deltaTime;
            if (canFade)
            {
                AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "BeHited");
                canFade = false;
                spriteRenderer.material.DOFade(flashAlpha, 0.5f);
                ActionKit.Delay(0.5f, () =>
                {
                    spriteRenderer.material.DOFade(1, 0.5f);
                }).Start(PlayerController.Instance);
            }
        }
        else canFade = true;
    }
}
