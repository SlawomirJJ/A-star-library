using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace AstarLibrary
{
    public class PoleModel
    {
        public int Id { get; set; }
        public PoleModel Rodzic { get; set; }
        public int G { get; set; } //droga po między wierzchołkiem początkowym a x(tym)
        public int H { get; set; } //przewidywana przez heurystykę droga od x do wierzchołka docelowego
        public int F { get; set; } //funkcja F(x)=g(x)+h(x)
        public char StartKon { get; set; }// "s"-startowy "k"-koncowy "n" -nie

        public bool Osiagalny { get; set; } //"o"-osiągalny "n"-nie osiągalny (wokable)
        public int X { get; set; } //współrzędna x
        public int Y { get; set; } // współrzędna y


        public List<int> PolaSasiadujace = new List<int>();//Przechowuje Id pól sąsiadujących


        public PoleModel(int id, int g, int h, int DlugoscSiatki)
        {
            Id = id;
            G = g;
            H = h;

            if (id % DlugoscSiatki == 0) X = DlugoscSiatki;
            else X = id % DlugoscSiatki;

            for (int i = 1; i <= DlugoscSiatki; i++)
            {
                if (Id <= DlugoscSiatki * i && Id > DlugoscSiatki * (i - 1)) Y = i;
            }

            SetPolaSasiadujace(Id, DlugoscSiatki);
        }



        public void SetPolaSasiadujace(int Id, int DlugoscSiatki)
        {
            if (X % DlugoscSiatki == 1)/////////////// lewy bok /////////////////////
            {
                if (X == 1 && Y == DlugoscSiatki) // lewy górny róg
                {
                    PolaSasiadujace.Add(Id - DlugoscSiatki);
                    PolaSasiadujace.Add(Id - DlugoscSiatki + 1);
                    PolaSasiadujace.Add(Id + 1);
                }

                else if (X == 1 && Y == 1) // lewy dolny róg
                {
                    PolaSasiadujace.Add(Id + DlugoscSiatki);
                    PolaSasiadujace.Add(Id + DlugoscSiatki + 1);
                    PolaSasiadujace.Add(Id + 1);
                }

                else // pozostałe na lewej krawędzi
                {
                    PolaSasiadujace.Add(Id + DlugoscSiatki);
                    PolaSasiadujace.Add(Id + DlugoscSiatki + 1);
                    PolaSasiadujace.Add(Id + 1);
                    PolaSasiadujace.Add(Id - DlugoscSiatki);
                    PolaSasiadujace.Add(Id - DlugoscSiatki + 1);
                }
            }
            else if (X % DlugoscSiatki == 0)/////////////  prawy bok   /////////////////////
            {
                if (X == DlugoscSiatki && Y == DlugoscSiatki)// prawy górny róg
                {
                    PolaSasiadujace.Add(Id - 1);
                    PolaSasiadujace.Add(Id - DlugoscSiatki);
                    PolaSasiadujace.Add(Id - DlugoscSiatki - 1);
                }

                else if (X == DlugoscSiatki && Y == 1) // prawy dolny róg
                {
                    PolaSasiadujace.Add(Id + DlugoscSiatki);
                    PolaSasiadujace.Add(Id + DlugoscSiatki - 1);
                    PolaSasiadujace.Add(Id - 1);
                }

                else // pozostałe na prawej krawędzi
                {
                    PolaSasiadujace.Add(Id - DlugoscSiatki);
                    PolaSasiadujace.Add(Id - DlugoscSiatki - 1);
                    PolaSasiadujace.Add(Id - 1);
                    PolaSasiadujace.Add(Id + DlugoscSiatki);
                    PolaSasiadujace.Add(Id + DlugoscSiatki - 1);
                }
            }
            else if (Y == DlugoscSiatki)//////////////////  górna krawędź bez rogów   //////////////////
            {
                PolaSasiadujace.Add(Id + 1);
                PolaSasiadujace.Add(Id - DlugoscSiatki + 1);
                PolaSasiadujace.Add(Id - DlugoscSiatki);
                PolaSasiadujace.Add(Id - DlugoscSiatki - 1);
                PolaSasiadujace.Add(Id - 1);
            }
            else if (Y == 1)////////////////    dolna krawędź bez rogów    /////////////////
            {
                PolaSasiadujace.Add(Id - 1);
                PolaSasiadujace.Add(Id + DlugoscSiatki - 1);
                PolaSasiadujace.Add(Id + DlugoscSiatki);
                PolaSasiadujace.Add(Id + DlugoscSiatki + 1);
                PolaSasiadujace.Add(Id + 1);
            }
            else    ////////////////////////////    Pozostałe pola w środku     /////////////////////////////
            {
                PolaSasiadujace.Add(Id - DlugoscSiatki);
                PolaSasiadujace.Add(Id + DlugoscSiatki);
                PolaSasiadujace.Add(Id + 1);
                PolaSasiadujace.Add(Id - 1);
                PolaSasiadujace.Add(Id - DlugoscSiatki - 1);
                PolaSasiadujace.Add(Id - DlugoscSiatki + 1);
                PolaSasiadujace.Add(Id + DlugoscSiatki - 1);
                PolaSasiadujace.Add(Id + DlugoscSiatki + 1);
            }


        }

    }
}

