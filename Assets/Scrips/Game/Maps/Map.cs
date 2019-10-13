using UnityEngine;

public interface Map {

    int GetChildCount();
    void SetChildCount(int childCount);

    int GetChildFinishCurver();
    void SetChildFinishCurver(int childFinishCurver);

    void ActionAllFinishPointCurver();

    Transform GetEggs();
    void SetEggs(Transform Eggs);
}
