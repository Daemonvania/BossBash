/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheatSystem {

    public event Action OnOverheatChanged;
    public event Action OnOverheat;

    private int overheatMax;
    private int _overheat;

    public OverheatSystem(int overheatMax) {
        this.overheatMax = overheatMax;
        _overheat = 0;
    }  

    public void SetOverheatAmount(int overHeat) {
        this._overheat = overHeat;
        if (OnOverheatChanged != null) OnOverheatChanged();
    }

    public float GetOverheatPercent() {
        return (float)_overheat / overheatMax;
    }

    public int GetOverheatAmount() {
        return _overheat;
    }

    public void AddOverheat(int amount) {
        _overheat += amount;
        _overheat = Mathf.Clamp(_overheat, 0, overheatMax);

        if (OnOverheatChanged != null) OnOverheatChanged();

        if (_overheat >= overheatMax) {
            Overheat();
        }
    }

    public void Overheat() {
        if (OnOverheat != null) OnOverheat();
    }

    public bool IsOverheating() {
        return _overheat >= overheatMax;
    }

}
