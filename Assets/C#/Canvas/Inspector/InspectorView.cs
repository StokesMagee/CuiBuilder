﻿using Battlehub.UIControls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InspectorView : MonoBehaviour {

    private RectTransformChangeListener m_rtcListener;
    private ScrollRect m_scrollRect;

    [SerializeField] Transform m_PanelsContainer;

    private IEnumerable<ComponentEditor> m_Components;
    private IEnumerable<ComponentEditor> m_ActiveComponents { get { return m_Components.Where(p => p.IsActive()); } }

    private void Awake()
    {
        m_scrollRect = GetComponent<ScrollRect>();
        m_rtcListener = GetComponentInChildren<RectTransformChangeListener>();
        m_rtcListener.RectTransformChanged += OnViewportRectTransformChanged;
        HierarchyView.AddSelectionListener(OnSelectionChanged);
        m_Components = GetComponentsInChildren<ComponentEditor>();
    }

    private void OnSelectionChanged(object sender, SelectionChangedArgs e)
    {
        if (e.NewItems == null || e.NewItems.Length == 0)
        {
            HideAllPanels();
        }
        else
        {
            ShowAllPanels();
        }
    }

    private void ShowAllPanels()
    {
        foreach (Transform panel in m_PanelsContainer)
            panel.gameObject.SetActive(true);
    }

    private void HideAllPanels()
    {
        foreach (Transform panel in m_PanelsContainer)
            panel.gameObject.SetActive(false);
    }

    public void OnNameChanged(object name)
    {
        foreach(var item in HierarchyView.GetSelectedItems())
        {
            HierarchyView.ChangeName(item, name.ToString());
        }
    }

    public void OnFadeOutChanged(object FadeOut)
    {
        foreach (var item in CUIObject.Selection)
        {
            item.FadeOut = (float) FadeOut;
        }
    }

    private void OnViewportRectTransformChanged()
    {
        Rect viewportRect = m_scrollRect.viewport.rect;
        foreach(Transform panel in m_scrollRect.viewport.GetChild(0))
        {
            LayoutElement lElement = panel.GetComponent<LayoutElement>();
            if (lElement)
            {
                lElement.minWidth = viewportRect.width;
            }
        }
    }
}