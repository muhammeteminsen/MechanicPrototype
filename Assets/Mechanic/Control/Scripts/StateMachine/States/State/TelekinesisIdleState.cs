using System.Collections.Generic;
using UnityEngine;

public class TelekinesisIdleState : StateAction<Telekinesis>
{
    public override void OnEnter(Telekinesis telekinesis)
    {
        telekinesis.TelekinesisAbility.Increase().Forget();
    }

    public override void OnUpdate(Telekinesis telekinesis)
    {
        if (telekinesis.TelekinesisAbility.IsUseAbility() && Input.GetKeyDown(KeyCode.E))
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
        if (telekinesis.Closest == null) return;
        if (telekinesis.Closest.TryGetComponent(out QuickOutline outline))
            outline.enabled = false;
    }
    private float _updateTimer;
    private void UpdateVisibleObject(Telekinesis telekinesis)
    {
        _updateTimer += Time.deltaTime;
        if (_updateTimer < 0.5f) return;
        _updateTimer = 0f;
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
        
        List<GameObject> unobstructedObjects = new List<GameObject>();

        foreach (var visibleObject in telekinesis.VisibleObjects)
        {
            if (visibleObject == null) continue;
            if (!visibleObject.TryGetComponent(out Collider col)) continue;

            Vector3 objPos = visibleObject.transform.position;
            
            if (Physics.Linecast(telekinesis.MainCamera.transform.position, objPos, out RaycastHit hit))
            {
                if (hit.collider != col)
                    continue;
            }

            unobstructedObjects.Add(visibleObject);
        }
        telekinesis.Closest = GetClosestObject(
            unobstructedObjects,
            Physics.Raycast(ray, out RaycastHit rayHit, telekinesis.maxDistance)
                ? rayHit.point
                : telekinesis.MainCamera.transform.position
        );
    }

    private void OutlineController(Telekinesis telekinesis)
    {
        if (telekinesis.Closest)
        {
            foreach (var target in telekinesis.VisibleObjects)
                if (target.TryGetComponent(out QuickOutline outline))
                    outline.enabled = false;
            if (telekinesis.Closest.TryGetComponent(out QuickOutline closesOutline))
                closesOutline.enabled = true;
        }
    }
}