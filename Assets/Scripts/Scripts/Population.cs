using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Population : MonoBehaviour
{
    List<Agent> pop = new List<Agent>();
    /* vars */
    public int popSize = 200;
    public float cutOff = 0.3f;
    public int length;
    public int goodGene;
    public Transform start;
    public Transform end;
    public GameObject agentPrefab;
    public int currentGeneration;
    public Text genText;

    void initPopulation()
    {
        /* instantiate all agents for population size */
        for (int i = 0; i < popSize; i++)
        {
            /* instantiates the objects at designated position */
            GameObject obj = Instantiate(agentPrefab, start.position, Quaternion.identity);
            obj.GetComponent<Agent>().initAgent(new DNA(length), end.position);
            pop.Add(obj.GetComponent<Agent>());
        }
        currentGeneration = 1;
    }
    private void Start()
    {
        initPopulation();

    }
    private void Update()
    {
        if (!HasActive()) { NextGeneration(); }
        genText.text = "current gen: " + currentGeneration.ToString();
    }
    void NextGeneration()
    {
        int survivorCut = Mathf.RoundToInt(popSize * cutOff);
        List<Agent> survivors = new List<Agent>();
        for (int i = 0; i < survivorCut; i++)
        {
            survivors.Add(getFittest());
        }
        for (int i = 0; i < pop.Count; i++)
        {
            Destroy(pop[i].gameObject);
        }
        pop.Clear();
        for (int i = 0; i < goodGene; i++)
        {
            GameObject go = Instantiate(agentPrefab, start.position, Quaternion.identity);
            go.GetComponent<Agent>().initAgent(survivors[i].dna,end.position);
            pop.Add(go.GetComponent<Agent>());

        }
        while (pop.Count < popSize)
        {
            for (int i = 0; i < survivors.Count; i++)
            {
                GameObject go = Instantiate(agentPrefab, start.position, Quaternion.identity);
                go.GetComponent<Agent>().initAgent(new DNA(survivors[i].dna, survivors[Random.Range(0, 10)].dna), end.position);
                pop.Add(go.GetComponent<Agent>());
                if (pop.Count >= popSize)
                {
                    break;
                }
            }
        }
        for (int i = 0; i < survivors.Count; i++)
        {
            Destroy(survivors[i].gameObject);
        }

        currentGeneration++;
    }

    Agent getFittest()
    {
        /* sets max fit float */
        float maxFitness = float.MinValue;
        int index = 0;
        for (int i = 0; i < pop.Count; i++)
        {
            if (pop[i].fitness > maxFitness)
            {
                maxFitness = pop[i].fitness;
                index = i;
            }

        }
        Agent fittest = pop[index];
        /* to ensure we dont get the same result every time */
        pop.Remove(fittest);
        return fittest;
    }

    bool HasActive()
    {
        for (int i = 0; i < pop.Count; i++)
        {
            if (!pop[i].isFinished)
            {
                return true;
            }
        }
        return false;
    }

}
