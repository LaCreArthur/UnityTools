using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModalWindowManager : MonoBehaviour
{
    public ModalWindow modalWindow;

    Stack<IModalWindowProvider> _modalStack = new Stack<IModalWindowProvider>();
    IModalWindowProvider _lastModal;
    
    void Start() => modalWindow.Close();
    void OnEnable() => ModalWindow.OnClose += OnModalClose;
    void OnDisable() => ModalWindow.OnClose -= OnModalClose;

    public void ShowModalFromTemplate(ModalWindowTemplate modalTemplate) => ShowModal(modalTemplate.provider);
    
    void ShowModal(IModalWindowProvider provider)
    {
        if (_lastModal != null) _modalStack.Push(_lastModal);

        Action onAccept = SetAction(provider.OnAccept());
        Action onDecline = SetAction(provider.OnDecline());
        Action onAlt = SetAction(provider.OnAlt());

        modalWindow.SetContent(provider.Title(), provider.Content(), provider.Sprite(),
            provider.AcceptBtnText(), provider.DeclineBtnText(), provider.AcceptBtnText(), provider.BodyOrientation(), onAccept,
            onDecline, onAlt);
        modalWindow.gameObject.SetActive(true);
        _lastModal = provider;
    }

    static Action SetAction(UnityEvent unityEvent) => unityEvent.GetPersistentEventCount() > 0 ? unityEvent.Invoke : null;

    void OnModalClose()
    {
        _lastModal = null;
        if (_modalStack.Count > 0) ShowModal(_modalStack.Pop());
    }
}