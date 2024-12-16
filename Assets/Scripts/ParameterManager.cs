using UnityEngine;
using UnityEngine.UI;

public class ParameterManager : MonoBehaviour
{
    public Slider infectionRateSlider;
    public Slider misinformationRateSlider;
    public Slider infectionChanceSlider;
    public Slider spreadDelaySlider;

    public Text infectionRateText;
    public Text misinformationRateText;
    public Text infectionChanceText;
    public Text spreadDelayText;

    public SimulationManager simulationManager;

    void Start()
    {
      
        infectionRateSlider.value = simulationManager.initialInfectionRate;
        misinformationRateSlider.value = simulationManager.initialMisinformationRate;
        infectionChanceSlider.value = simulationManager.initialInfectionRate;
        spreadDelaySlider.value = simulationManager.defaultSpreadDelay;

        
        UpdateTexts();
    }

    void Update()
    {
      
        simulationManager.initialInfectionRate = infectionRateSlider.value;
        simulationManager.initialMisinformationRate = misinformationRateSlider.value;
        simulationManager.initialInfectionRate = infectionChanceSlider.value;
        simulationManager.defaultSpreadDelay = spreadDelaySlider.value;

     
        UpdateTexts();

     
        UpdateCitizenParameters();
    }

    void UpdateTexts()
    {
        infectionRateText.text = "Infection Rate: " + infectionRateSlider.value.ToString("F2");
        misinformationRateText.text = "Misinformation Rate: " + misinformationRateSlider.value.ToString("F2");
        infectionChanceText.text = "Infection Chance: " + infectionChanceSlider.value.ToString("F2");
        spreadDelayText.text = "Spread Delay: " + spreadDelaySlider.value.ToString("F2") + "s";
    }

    void UpdateCitizenParameters()
    {
     
        Citizen[] citizens = FindObjectsOfType<Citizen>();

        foreach (Citizen citizen in citizens)
        {
           
            if (citizen.state == CitizenState.Healthy || citizen.state == CitizenState.Desinformado)
            {
                citizen.infectionChance = simulationManager.initialInfectionRate;

               
                if (citizen.state == CitizenState.Infected)
                {
                    citizen.spreadDelay = simulationManager.defaultSpreadDelay;
                }
            }
        }
    }
}
