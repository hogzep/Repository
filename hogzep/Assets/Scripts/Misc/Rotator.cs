using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Rotator : MonoBehaviour 
{
	void Start () 
    {
        this.transform.DOMoveY(2, 2).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        this.transform.DORotate(new Vector3(0, 360, 0), 6, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);

	}
}
