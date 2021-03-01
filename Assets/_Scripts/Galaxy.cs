using Bodies;
using Bodies.Stars;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Galaxy : MonoBehaviour
{
    public Star[] Stars = null;
    private StarFactory StarFac = null;
    private SpriteRenderer rend = null;

    private Transform Tooltip;
    private Text TooltipNameText;
    private Text TooltipTypeText;
    private Text TooltipPositionText;

    private Transform TooltipMore;
    private Text TooltipMoreNameText;
    private Text TooltipMoreRadiusText;
    private Text TooltipMoreMassText;
    private Text TooltipMoreLuminosityText;
    private Text TooltipMoreLifetimeText;
    private Text TooltipMoreHabitableZoneText;
    private Text TooltipMoreTemperatureText;

    private Transform CursorHover;
    private Transform CursorLock;

    private bool UiInitDone = false;

    private int HoveredStar = -1;
    private int SelectedStar = -1;

    public void Generate(int amountStars)
    {
        StarFac = gameObject.AddComponent<StarFactory>();
        StarFac.InitStarFactory(GalaxyFactory.Seed);

        int am = amountStars;
        Stars = new Star[am];

        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i] = StarFac.GenerateStar();
        }
    }

    void Update()
    {
        if (rend == null)
        {
            rend = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        if (!UiInitDone)
        {
            InitUI();
        }

        Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2d v2 = new Vector2d(v.x, v.y);

        bool currentlyHovering = false;
        for (int i = 0; i < Stars.Length; i++)
        {
            if ((Stars[i].Position - v2).sqrMagnitude <= (0.5 * 0.5))
            {
                HoveredStar = i;
                currentlyHovering ^= true;

                TooltipNameText.text = Stars[i].Name;
                TooltipTypeText.text = StarFactory.ESpectralTypeToString(Stars[i].SpectralType) + StarFactory.ELuminosityTypeToString(Stars[i].LuminosityType);
                TooltipPositionText.text = Stars[i].Position.ToString(); 

                if (!Tooltip.gameObject.activeSelf)
                {
                    Tooltip.gameObject.SetActive(true);
                }

                if (!CursorHover.gameObject.activeSelf)
                {
                    CursorHover.gameObject.SetActive(true);
                }

                CursorHover.localPosition = new Vector2((float)Stars[HoveredStar].Position.x + 0.27f, (float)Stars[HoveredStar].Position.y + 0.25f);

                break;
            }
        }

        if (!currentlyHovering)
        {
            HoveredStar = -1;

            if (Tooltip.gameObject.activeSelf)
            {
                Tooltip.gameObject.SetActive(false);
            }

            if (CursorHover.gameObject.activeSelf)
            {
                CursorHover.gameObject.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0) && HoveredStar != -1)
        {
            SelectedStar = HoveredStar;
            TooltipMoreNameText.text = Stars[SelectedStar].Name;
            TooltipMoreRadiusText.text = Stars[SelectedStar].RadiusSuns.ToString();
            TooltipMoreMassText.text = Stars[SelectedStar].MassSuns.ToString();
            TooltipMoreLuminosityText.text = Stars[SelectedStar].LuminositySuns.ToString();
            TooltipMoreLifetimeText.text = Stars[SelectedStar].LifetimeMyears.ToString() == "-1" ? "Infinite" : Stars[SelectedStar].LifetimeMyears.ToString();
            TooltipMoreHabitableZoneText.text = Stars[SelectedStar].HabitableZoneAU.ToString();
            TooltipMoreTemperatureText.text = Stars[SelectedStar].TemperatureKelvin.ToString();

            if (!CursorLock.gameObject.activeSelf)
            {
                CursorLock.gameObject.SetActive(true);
            }

            CursorLock.localPosition = new Vector2((float)Stars[SelectedStar].Position.x + 0.27f, (float)Stars[SelectedStar].Position.y + 0.25f);

            if (!TooltipMore.gameObject.activeSelf)
            {
                TooltipMore.gameObject.SetActive(true);
            }
        }
        else if (Input.GetMouseButtonDown(0) && HoveredStar == -1)
        {
            if (TooltipMore.gameObject.activeSelf)
            {
                TooltipMore.gameObject.SetActive(false);
            }

            if (CursorLock.gameObject.activeSelf)
            {
                CursorLock.gameObject.SetActive(false);
            }
        }
    }

    void InitUI()
    {
        Tooltip = GameObject.Find("Canvas/Map UI/Tooltip").transform;
        TooltipNameText = Tooltip.Find("Values/NameValue").GetComponent<Text>();
        TooltipTypeText = Tooltip.Find("Values/TypeValue").GetComponent<Text>();
        TooltipPositionText = Tooltip.Find("Values/PositionValue").GetComponent<Text>();

        TooltipMore = GameObject.Find("Canvas/Map UI/TooltipMore").transform;
        TooltipMoreNameText = TooltipMore.Find("NamePanel/Name/NameValue").GetComponent<Text>();
        TooltipMoreRadiusText = TooltipMore.Find("Values/RadiusValue").GetComponent<Text>();
        TooltipMoreMassText = TooltipMore.Find("Values/MassValue").GetComponent<Text>();
        TooltipMoreLuminosityText = TooltipMore.Find("Values/LuminosityValue").GetComponent<Text>();
        TooltipMoreLifetimeText = TooltipMore.Find("Values2/LifetimeValue").GetComponent<Text>();
        TooltipMoreHabitableZoneText = TooltipMore.Find("Values2/HabitableZoneValue").GetComponent<Text>();
        TooltipMoreTemperatureText = TooltipMore.Find("Values2/TemperatureValue").GetComponent<Text>();

        CursorHover = transform.Find("StarOverlay/CursorHover").transform;
        CursorLock = transform.Find("StarOverlay/CursorLock").transform;

        UiInitDone = true;
    }
}
