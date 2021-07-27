﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPicker : MonoBehaviour
{
    private BoardComposition[] _regularScenarios;
    private BoardComposition[] _testScenarios;

    public RectTransform regularScenariosBackground;
    public RectTransform regularScenariosContainer;
    public RectTransform testScenariosContainer;
    public RectTransform testScenariosBackground;
    public Button scenarioBtnPrefab;

    private void Awake()
    {
        _regularScenarios = Resources.LoadAll<BoardComposition>("BoardCompositions/Scenarios");
        _testScenarios = Resources.LoadAll<BoardComposition>("BoardCompositions/TestScenarios");
    }

    private void Start()
    {
        InitializeScenarioButtons();
    }

    private void InitializeScenarioButtons()
    {
        Common.Empty(regularScenariosContainer.transform);
        if (this._regularScenarios != null && this._regularScenarios.Length > 0)
        {
            this.ExtendContainer(regularScenariosBackground, _regularScenarios.Length);
            foreach (var scenario in this._regularScenarios)
            {
                this.AppendScenario(scenario, regularScenariosContainer);
            }
        }
        else
        {
            Debug.LogWarning("Cannot load regular scenarios!");
        }

        Common.Empty(testScenariosContainer.transform);
        if (this._testScenarios != null && this._testScenarios.Length > 0)
        {
            this.ExtendContainer(testScenariosBackground, _testScenarios.Length);
            foreach (var scenario in this._testScenarios)
            {
                this.AppendScenario(scenario, testScenariosContainer);
            }
        }
        else
        {
            Debug.LogWarning("Cannot load regular scenarios!");
        }
    }

    private void ExtendContainer(RectTransform container, int length)
    {
        container.sizeDelta = new Vector2(222.5f * length, 250);
    }

    private void AppendScenario(BoardComposition scenario, RectTransform container)
    {
        Button scenarioBtn = Instantiate<Button>(scenarioBtnPrefab, container);
        scenarioBtn.name = scenario.Name + "_Btn";
        scenarioBtn.onClick.AddListener(delegate { OnHeroButtonPressed(scenario); });

        var scenarioNameText = scenarioBtn.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        scenarioNameText.text = scenario.Name;

        if (scenario.PreviewImage != null)
        {
            Image scenarioPreviewImage = scenarioBtn.transform.Find("PreviewImage").GetComponent<Image>();
            scenarioPreviewImage.sprite = scenario.PreviewImage;
            scenarioPreviewImage.color = Color.white;
        }
    }

    private void OnHeroButtonPressed(BoardComposition scenario)
    {
        BoardManager.Instance.CurrentScenario = scenario;
        GameManager.Instance.PlayLevel();
    }
}