using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager2 : MonoBehaviour
{
    public Action<Vector3Int> OnMouseClick, OnMouseHold;    //list action mouse
    public Action OnMouseUp;

    [SerializeField] Camera mainCamera;

    public LayerMask groundLayer;

    private int move = 0;

    private void Update() {
        CheckClickDownEvent();
        CheckClickUpEvent();
        CheckClickHoldEvent();
    }

    private Vector3Int? RaycastGround() { //buat mendeteksi ground yang dihover sama mouse
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity,groundLayer))
        {
            Vector3Int positionInt = Vector3Int.RoundToInt(hit.point);
            return positionInt;
        }
        return null;
    }   

    private void CheckClickHoldEvent() //buat cek kalau mouse dihold
    {
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            var position = RaycastGround();
            if (position != null)
            {
                OnMouseHold?.Invoke(position.Value);
            }
        }
    }

    private void CheckClickUpEvent() //buat check kalau mouse diangkat
    {
        if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            OnMouseUp?.Invoke();
        }
    }

    private void CheckClickDownEvent() //buat check kalau mouse diklik
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            var position = RaycastGround();
            if (position != null)
            {
                OnMouseClick?.Invoke(position.Value);
                
                move++; //buat menghitung langkah
            }
        }
    }

    public int GetNumberOfMoves() { //buat menghitung langkah
        return move;
    }
}
