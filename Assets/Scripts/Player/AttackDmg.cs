using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDmg : MonoBehaviour
{
    public List<Health> damaged = new List<Health>();
    PlayerAttack pa;
    public Animator Anim;
    PlayerHealth hp;
    BuffsManager bm;

    const string AttackLayer = "AttackLayer";
    int ind;
    private void Start()
    {
        if (hp == null) hp = FindObjectOfType<PlayerHealth>();
        if (bm == null) bm = FindObjectOfType<BuffsManager>();
        pa = transform.parent.GetComponent<PlayerAttack>();
        ind = Anim.GetLayerIndex(AttackLayer);
    }

    //Basically when we attack, the script will check what we CAN attack multiple times
    public void Attack_beforehands()
    {
        Anim.SetLayerWeight(ind, 1);
        foreach (var v in pa.Trigger.CurTargets)
        {
            if (!damaged.Contains(v.Hp)) { v.Hp.Take_Dmg(pa.Damage); SoundManager.Instance.PlaySound(Sound.Hurt, 0.2f, v.Hp.transform); damaged.Add(v.Hp); }

            
        }
    }

    public void Attack_last()
    {
        foreach (var v in pa.Trigger.CurTargets)
        {
            if (!damaged.Contains(v.Hp)) { v.Hp.Take_Dmg(pa.Damage); SoundManager.Instance.PlaySound(Sound.Hurt, 0.2f, v.Hp.transform); damaged.Add(v.Hp); }
        }

        //4 d landing attacks heals, attacks deal self damage
        if (bm.GetBuffAmount(4) > 0 && damaged.Count > 0) { hp.Take_Heal(bm.d_heal); }

        damaged.Clear();
        Anim.SetLayerWeight(ind, 0);
    }
}
