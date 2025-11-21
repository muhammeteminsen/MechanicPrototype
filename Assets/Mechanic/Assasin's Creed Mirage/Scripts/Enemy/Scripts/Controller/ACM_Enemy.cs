using UnityEngine;
using UnityEngine.UI;

public class ACM_Enemy : MonoBehaviour, ACM_ISelectableEnemy
{
    public Image selectionIndicator;
    public Transform effectPos;
    private QuickOutline _outline;
    public GameObject EffectInstance { get; set; }

    private void Start()
    {
        selectionIndicator.enabled = false;
        _outline = GetComponent<QuickOutline>();
        _outline.enabled = false;
    }

    public void OnSelectable(ChainAssassination context)
    {
        selectionIndicator.transform.rotation = Quaternion.LookRotation(context.MainCamera.transform.forward);
        selectionIndicator.enabled = true;
        selectionIndicator.color = Color.green;
        context.crosshair.color = Color.red;
    }

   
    public void OnSelected(ChainAssassination context)
    {
        if(context.EnemyQueue.Contains(this)) return;
        context.EnemyQueue.Add(this);
        selectionIndicator.color = Color.red;
        _outline.enabled = true;
        EffectInstance = Instantiate(context.effectPrefab, effectPos.position, Quaternion.identity);
        if (EffectInstance.TryGetComponent(out ACM_AnimationSwapper animationSwapper))
            animationSwapper.SetAnimationClip(context);
    }
    
    public void OnDeselected(ChainAssassination context)
    {
        OnUnSelect(context);
        _outline.enabled = false;
        context.EnemyQueue.Remove(this);
        Destroy(EffectInstance);
    }

    public void OnUnSelect(ChainAssassination context)
    {
        selectionIndicator.enabled = false;
        context.crosshair.color = Color.white;
    }
}