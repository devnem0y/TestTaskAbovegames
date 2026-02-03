using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BannerManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Transform _content;
    [SerializeField] private List<GameObject> _banners; //TODO: Баннеры можно было бы спавнить динамически, например из ScriptableObject. Но это не входит в ТЗ и я заморачиваться не стал.
    [SerializeField] private float _scrollInterval = 5f;
    [SerializeField] private float _scrollDuration = 0.5f;
    [SerializeField] private IndicatorPanel _indicatorPanel;

    private int _currentIndex;
    private Coroutine _autoScrollCoroutine;
    private bool _isUserScrolling;
    private Vector2 _startDragPosition;
    
    private const float _dragThreshold = 35f;

    private void Awake()
    {
        var eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger == null) eventTrigger = gameObject.AddComponent<EventTrigger>();

        eventTrigger.triggers.Clear();
        
        var beginDragEntry = new EventTrigger.Entry { eventID = EventTriggerType.BeginDrag };
        beginDragEntry.callback.AddListener((eventData) => { OnBeginDrag((PointerEventData)eventData); });
        eventTrigger.triggers.Add(beginDragEntry);
        
        var endDragEntry = new EventTrigger.Entry { eventID = EventTriggerType.EndDrag };
        endDragEntry.callback.AddListener((eventData) => { OnEndDrag((PointerEventData)eventData); });
        eventTrigger.triggers.Add(endDragEntry);
    }

    public void Init()
    {
        _indicatorPanel.CreateIndicators(_banners.Count);
        _indicatorPanel.SetActiveIndex(_currentIndex);
        
        _autoScrollCoroutine = StartCoroutine(AutoScrollLoop());
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _isUserScrolling = true;
        _startDragPosition = eventData.position;

        if (_autoScrollCoroutine == null) return;
        
        StopCoroutine(_autoScrollCoroutine);
        _autoScrollCoroutine = null;
    }

    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isUserScrolling) return;
        
        var endPosition = eventData.position;
        var swipeVector = endPosition - _startDragPosition;
        
        if (swipeVector.magnitude >= _dragThreshold)
        {
            if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
            {
                if (swipeVector.x > 0)
                {
                    _currentIndex = (_currentIndex - 1 + _banners.Count) % _banners.Count;
                    ScrollToIndex(_currentIndex);
                    _indicatorPanel.SetActiveIndex(_currentIndex);
                }
                else ScrollToNext();
            }
        }
        
        _isUserScrolling = false;
        StartCoroutine(RestartAutoScrollAfterDelay(1f));
    }
    
    private IEnumerator AutoScrollLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_scrollInterval);
            
            if (_banners.Count <= 1 || _isUserScrolling) continue;
            ScrollToNext();
        }
    }
    
    private void ScrollToNext()
    {
        _currentIndex = (_currentIndex + 1) % _banners.Count;
        ScrollToIndex(_currentIndex);
        _indicatorPanel.SetActiveIndex(_currentIndex);
    }
    
    private void ScrollToIndex(int index)
    {
        var contentRT = _content.GetComponent<RectTransform>();
        var layout = contentRT.GetComponent<HorizontalLayoutGroup>();
        var spacing = layout.spacing;
        float paddingLeft = layout.padding.left;

        float totalWidth = 0;
        for (var i = 0; i <= index; i++)
        {
            totalWidth += _banners[i].GetComponent<RectTransform>().rect.width;
            if (i < index) totalWidth += spacing;
        }

        var targetX = -(paddingLeft + totalWidth - _banners[index].GetComponent<RectTransform>().rect.width);
        targetX = Mathf.Clamp(targetX, -GetMaxScrollPosition(), 0);

        StartCoroutine(SmoothScroll(contentRT, targetX));
    }

    private float GetMaxScrollPosition()
    {
        var contentRT = _content.GetComponent<RectTransform>();
        var viewportRT = contentRT.parent as RectTransform;

        return Mathf.Max(0, contentRT.rect.width - viewportRT.rect.width);
    }
    
    private IEnumerator SmoothScroll(RectTransform content, float targetX)
    {
        var startPos = content.anchoredPosition;
        var endPos = new Vector2(targetX, content.anchoredPosition.y);


        var elapsedTime = 0f;
        while (elapsedTime < _scrollDuration)
        {
            elapsedTime += Time.deltaTime;
            var t = Mathf.Clamp01(elapsedTime / _scrollDuration);
            content.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
        
        content.anchoredPosition = endPos;
    }
    
    private IEnumerator RestartAutoScrollAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _autoScrollCoroutine ??= StartCoroutine(AutoScrollLoop());
    }
    
    private void OnDestroy()
    {
        if (_autoScrollCoroutine != null) StopCoroutine(_autoScrollCoroutine);
    }
}