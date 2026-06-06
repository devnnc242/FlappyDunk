using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Bottom buttons")]
    [SerializeField] private RectTransform menuBtn;
    [SerializeField] private RectTransform skinBtn;

    [Header("Settings")]
    [SerializeField] private float selectedScale = 1.2f;
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float animationDuration = 0.25f;

    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject skinPanel;

    [Header("Button Images")]
    [SerializeField] private Image menuImage;
    [SerializeField] private Image skinImage;

    [SerializeField] private Color selectedColor = Color.blue;
    [SerializeField] private Color normalColor = Color.white;

    private BottomTab currentTab;

    private enum BottomTab
    {
        Menu,
        Skin
    }

    private void Start()
    {
        SelectTab(BottomTab.Menu);
    }

    #region Bottom tabs
    public void OnClickMenuTab()
    {
        SelectTab(BottomTab.Menu);
    }

    public void OnClickSkinTab()
    {
        SelectTab(BottomTab.Skin);
    }

    private void SelectTab(BottomTab tab)
    {
        currentTab = tab;

        bool isMenu = tab == BottomTab.Menu;
        bool isSkin = tab == BottomTab.Skin;

        AnimateTab(menuBtn, isMenu);
        AnimateTab(skinBtn, isSkin);

        menuImage.color =
            isMenu ? selectedColor : normalColor;

        skinImage.color =
            isSkin ? selectedColor : normalColor;

        menuPanel.SetActive(isMenu);
        skinPanel.SetActive(isSkin);
    }

    private void AnimateTab(RectTransform target, bool isSelected)
    {
        float targetScale = isSelected ? selectedScale : normalScale;

        target.DOScale(targetScale, animationDuration).SetEase(Ease.OutBack).SetUpdate(true);
    }
    #endregion

    #region Play
    public void PlayGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(1);
    }
    #endregion
}
