using System.Collections.Generic;
using Control;
using UnityEngine;

public class TelekinesisIdleState : StateAction<Telekinesis>
{
    public override void OnEnter(Telekinesis telekinesis)
    {
        telekinesis.TelekinesisAbility.Increase().Forget();
    }

    public override void OnUpdate(Telekinesis telekinesis)
    {
        if (Input.GetKeyDown(KeyCode.E) && telekinesis.TelekinesisAbility.IsUseAbility())
        {
            telekinesis.ChangeState(new TelekinesisLiftState());
            return;
        }

        UpdateVisibleObject(telekinesis);
        RaycastController(telekinesis);
        OutlineController(telekinesis);
    }

    public override void OnExit(Telekinesis telekinesis)
    {
        if (telekinesis.Closest.TryGetComponent(out Outline outline))
            outline.enabled = false;
    }

    private void UpdateVisibleObject(Telekinesis telekinesis)
    {
        telekinesis.VisibleObjects.Clear();
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(telekinesis.MainCamera);
        TelekinesisObject[] telekinesisObjects = Object.FindObjectsByType<TelekinesisObject>(FindObjectsSortMode.None);
        foreach (var telekinesisObject in telekinesisObjects)
        {
            telekinesisObject.TryGetComponent(out Collider col);
            if (GeometryUtility.TestPlanesAABB(planes, col.bounds))
            {
                telekinesis.VisibleObjects.Add(col.gameObject);
            }
        }
    }

    private GameObject GetClosestObject(List<GameObject> targetList, Vector3 targetPosition)
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = targetPosition;
        foreach (var target in targetList)
        {
            Vector3 distance = target.transform.position - currentPosition;
            float distanceSqr = distance.sqrMagnitude;
            if (distanceSqr < minDistance)
            {
                minDistance = distanceSqr;
                closest = target;
            }
        }

        return closest;
    }

    private void RaycastController(Telekinesis telekinesis)
    {
        Ray ray = telekinesis.MainCamera.ScreenPointToRay(Input.mousePosition);
        telekinesis.Closest = GetClosestObject(telekinesis.VisibleObjects,
            Physics.Raycast(ray, out RaycastHit hit, telekinesis.maxDistance)
                ? hit.point
                : telekinesis.MainCamera.transform.position);
    }

    private void OutlineController(Telekinesis telekinesis)
    {
        if (telekinesis.Closest)
        {
            foreach (var target in telekinesis.VisibleObjects)
                if (target.TryGetComponent(out Outline outline))
                    outline.enabled = false;
            if (telekinesis.Closest.TryGetComponent(out Outline closesOutline))
                closesOutline.enabled = true;
        }
    }
}