using UnityEngine;
using UnityEngine.UI;

public class ACM_Enemy : MonoBehaviour, ACM_ISelectableEnemy
{
    public Image selectionIndicator;
    [SerializeField] private Transform assassinationTransform;
    private GameObject _highlightInstance;
    private void Awake()
    {
        selectionIndicator.enabled = false;
    }

    public void OnSelectable(ChainAssassination assassination)
    {
        selectionIndicator.transform.rotation = Quaternion.LookRotation(assassination.MainCamera.transform.forward);
        selectionIndicator.enabled = true;
        selectionIndicator.color = Color.green;
        assassination.crosshair.color = Color.red;
    }

    public void OnSelected(ChainAssassination assassination)
    {
        if(assassination.EnemyQueue.Contains(this)) return;
        assassination.EnemyQueue.Add(this);
        selectionIndicator.color = Color.red;
    }

    public void OnUnSelect(ChainAssassination assassination)
    {
        selectionIndicator.enabled = false;
        assassination.crosshair.color = Color.white;
    }
}