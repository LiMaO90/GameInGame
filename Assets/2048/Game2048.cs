using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game2048 : MonoBehaviour
{
    private GameObject[,,] grilleObject;
    private GameObject canvas;
    private GameObject textFin;

    private int[,] grille;
    private bool estJoueur;
    private bool estFin;
    private int score;
    private int nbPGagne;


    public Color[] tabColor;
    public int size;
    public float multiplicator2048;
    public static int numDebut = 1;

    // Start is called before the first frame update
    void Start()
    {
        estJoueur = false;
        estFin = false;
        canvas = GameObject.Find("Canvas");
        textFin = GameObject.Find("textFin");
        initGrille();
        tourSuivant();
    }

    // Update is called once per frame
    void Update()
    {
        if (estFin)
        {
            canvas.SetActive(true);
            textFin.GetComponent<Text>().text = "Vous avez gagnez " + nbPGagne + " P";
        }
        else
        {
            canvas.SetActive(false);
            drawGrille();
            if (estJoueur)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow)) up();
                else if (Input.GetKeyDown(KeyCode.DownArrow)) down();
                else if (Input.GetKeyDown(KeyCode.LeftArrow)) left();
                else if (Input.GetKeyDown(KeyCode.RightArrow)) right();
            }
        }
    }

    private void drawGrille()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int val = grille[i, j];

                GameObject box = grilleObject[i, j, 0];
                GameObject text = grilleObject[i, j, 1];

                if (val == 0)
                {
                    box.SetActive(false);
                }
                else
                {
                    box.SetActive(true);
                    text.GetComponent<TextMesh>().text = val.ToString();

                    box.GetComponent<SpriteRenderer>().color = getColor(val);
                }
            }
        }
    }

    private Color getColor(int number)
    {
        int id = idTabColor(number);
        if (id > tabColor.Length - 1)
            id = tabColor.Length - 1;
        return tabColor[id];
    }

    private int idTabColor(int number)
    {
        int cpt = 0;
        while (number > numDebut)
        {
            number = number / 2;
            cpt++;
        }
        return cpt;
    }

    private void initGrille()
    {
        grille = new int[size, size];
        grilleObject = new GameObject[size, size, 2];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                grille[i, j] = 0;

                GameObject box = GameObject.Find(i + "," + j);
                GameObject text = GameObject.Find("text" + i + j);

                grilleObject[i, j, 0] = box;
                grilleObject[i, j, 1] = text;
            }
        }
    }

    private void fin()
    {
        estFin = true;
        score = calculScore();
        nbPGagne = calculP(score);
    }

    private int calculScore()
    {
        int sum = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                sum = sum + grille[i,j];
            }
        }
        return sum;
    }

    private int calculP(int score)
    {
        float scoreFloat = (float)score;
        int piece = (int) Mathf.Round(scoreFloat * multiplicator2048);
        if (piece <= 0) return 1;
        return piece;
    }

    private void tourSuivant()
    {
        List<int[]> freeCase = freeBox();
        int taille = freeCase.Count;

        if (taille <= 0)
        {
            Debug.Log("Fin de Partie");
            fin();
        }
        else
        {
            int id = Random.Range(0, taille);
            int[] firstElem = freeCase[id];
            grille[firstElem[0], firstElem[1]] = numDebut;

            estJoueur = true;
        }
    }

    private List<int[]> freeBox()
    {
        List<int[]> resultat = new List<int[]>();

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int val = grille[i, j];
                int[] tab = new int[2] { i, j };
                if (val == 0)
                    resultat.Add(tab);
            }
        }

        return resultat;
    }

    private void left()
    {
        estJoueur = false;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int courant = grille[i, j];
                if(courant != 0)
                {
                    bool stop = false;
                    bool fusion = false;
                    int jTmp = j - 1;
                    while(jTmp >= 0 && !stop)
                    {
                        int courantTmp = grille[i, jTmp];
                        if (courantTmp != 0 && courantTmp == courant)
                        {
                            fusion = true;
                            stop = true;
                        }
                        else if (courantTmp != 0)
                        {
                            stop = true;
                        }
                        else
                            jTmp--;
                    }

                    grille[i, j] = 0;
                    if (stop && fusion) grille[i, jTmp] = courant * 2;
                    else grille[i, jTmp + 1] = courant;
                }
            }
        }

        tourSuivant();
    }

    private void down()
    {
        estJoueur = false;

        for (int i = size-1; i >= 0; i--)
        {
            for (int j = 0; j < size; j++)
            {
                int courant = grille[i, j];
                if (courant != 0)
                {
                    bool stop = false;
                    bool fusion = false;
                    int iTmp = i + 1;
                    while (iTmp < size && !stop)
                    {
                        int courantTmp = grille[iTmp, j];
                        if (courantTmp != 0 && courantTmp == courant)
                        {
                            fusion = true;
                            stop = true;
                        }
                        else if (courantTmp != 0)
                        {
                            stop = true;
                        }
                        else
                            iTmp++;
                    }

                    grille[i, j] = 0;
                    if (stop && fusion) grille[iTmp, j] = courant * 2;
                    else grille[iTmp - 1, j] = courant;
                }
            }
        }

        tourSuivant();
    }

    private void up()
    {
        estJoueur = false;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int courant = grille[i, j];
                if (courant != 0)
                {
                    bool stop = false;
                    bool fusion = false;
                    int iTmp = i - 1;
                    while (iTmp >= 0 && !stop)
                    {
                        int courantTmp = grille[iTmp, j];
                        if (courantTmp != 0 && courantTmp == courant)
                        {
                            fusion = true;
                            stop = true;
                        }
                        else if (courantTmp != 0)
                        {
                            stop = true;
                        }
                        else
                            iTmp--;
                    }

                    grille[i, j] = 0;
                    if (stop && fusion) grille[iTmp, j] = courant * 2;
                    else grille[iTmp + 1, j] = courant;
                }
            }
        }

        tourSuivant();
    }

    private void right()
    {
        estJoueur = false;

        for (int i = 0; i < size; i++)
        {
            for (int j = size - 1; j >= 0; j--)
            {
                int courant = grille[i, j];
                if (courant != 0)
                {
                    bool stop = false;
                    bool fusion = false;
                    int jTmp = j + 1;
                    while (jTmp < size && !stop)
                    {
                        int courantTmp = grille[i, jTmp];
                        if (courantTmp != 0 && courantTmp == courant)
                        {
                            fusion = true;
                            stop = true;
                        }
                        else if (courantTmp != 0)
                        {
                            stop = true;
                        }
                        else
                            jTmp++;
                    }

                    grille[i, j] = 0;
                    if (stop && fusion) grille[i, jTmp] = courant * 2;
                    else grille[i, jTmp - 1] = courant;
                }
            }
        }

        tourSuivant();
    }
}
