using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstarLibrary
{
    class Algorithm
    {
        //////////////////////////////           ALGORYTM       //////////////////////////////
        public static List<PoleModel> Algorytm(List<PoleModel> Pola)
        {
            PoleModel PoleS = null;
            PoleModel PoleK = null;
            for (int i = 0; i < Pola.Count; i++)
            {
                if (Pola[i].StartKon == 's')
                {
                    PoleS = Pola[i];
                }
                else if (Pola[i].StartKon == 'k')
                {
                    PoleK = Pola[i];
                }
            }
            // tablice przechowują pola
            List<PoleModel> PolaNieodwiedzoneSasiadujace = new(new[] { PoleS });  // Zbiór wierzchołków nieodwiedzonych, sąsiadujących z odwiedzonymi. 
            List<PoleModel> PolaPrzejrzane = new();
            PoleModel poleNajnizszeF = null; // przechowuje pole z najniższym F
            int DlugoscSiatki = (int)Math.Sqrt(Pola.Count);
            List<PoleModel> Sciezka = new();


            while (PolaNieodwiedzoneSasiadujace.Count != 0)
            {   // wybranie wierzchołka ze zbioru PolaNieodwiedzoneSasiadujace o najniższym F 
                for (int i = 0; i < PolaNieodwiedzoneSasiadujace.Count; i++)
                {
                    if (i == 0)
                    {
                        poleNajnizszeF = PolaNieodwiedzoneSasiadujace[0];
                    }
                    else if (PolaNieodwiedzoneSasiadujace[i].F < poleNajnizszeF.F)
                    {
                        poleNajnizszeF = PolaNieodwiedzoneSasiadujace[i];
                    }
                }



                //sprawdzenie czy to węzęł końcowy
                if (poleNajnizszeF.Id == PoleK.Id)
                {
                    return RekonstrukcjaSciezki(PoleS, PoleK);
                    //return View("Pole",Pola);

                }

                // dodanie nowo sprawdzonego elementu do tablicy PolaPrzejrzane
                PolaPrzejrzane.Add(poleNajnizszeF);//dodajemy obiekt

                // usunięcie nowo sprawdzonego elementu z tablicy PolaNieodwiedzoneSasiadujace 
                PolaNieodwiedzoneSasiadujace.Remove(poleNajnizszeF);

                //przeszukujemy pola sąsiadujące z polem "poleNajnizszeF"
                for (int j = 0; j < poleNajnizszeF.PolaSasiadujace.Count; j++)
                {
                    // sprawdzenie czy danego pola sąsiadującego nie ma w tablicy PolaPrzejrzanec
                    if (!(PolaPrzejrzane.Exists(x => x.Id == poleNajnizszeF.PolaSasiadujace[j])) && Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].Osiagalny != false)
                    {
                        int tempG = 0;        // prawo, lewo, góra, dół
                        if (poleNajnizszeF.Id + 1 == poleNajnizszeF.PolaSasiadujace[j] || poleNajnizszeF.Id - 1 == poleNajnizszeF.PolaSasiadujace[j] || poleNajnizszeF.Id + DlugoscSiatki == poleNajnizszeF.PolaSasiadujace[j] || poleNajnizszeF.Id - DlugoscSiatki == poleNajnizszeF.PolaSasiadujace[j])
                        {
                            tempG = poleNajnizszeF.G + 10;
                        }
                        else if (HaveNeighbourObstacle(poleNajnizszeF.Id, poleNajnizszeF.PolaSasiadujace[j], Pola) == true) // po skosie, czy nie ma sąsiada przeszkody
                        {
                            continue;
                        }
                        else
                        {
                            tempG = poleNajnizszeF.G + 14;
                        }

                        // jeżeli pole sąsiadujące jest w tablicy PolaNieodwiedzoneSasiadujace to sprawdź czy nie dostaniesz się tam szybciej      
                        if (!(PolaNieodwiedzoneSasiadujace.Count == 0))
                        {
                            bool zawiera = false;  // zmienna pomocnicza
                            for (int l = 0; l < PolaNieodwiedzoneSasiadujace.Count; l++)
                            {
                                if (PolaNieodwiedzoneSasiadujace[l].Id == poleNajnizszeF.PolaSasiadujace[j])
                                {   // jeżeli można się dostać szybciej to zapisz to
                                    if (tempG < Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].G)
                                    {
                                        Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].G = tempG;
                                        Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].Rodzic = poleNajnizszeF;
                                        // dodajemy heurystykę czyli oszacowaną odl z badanego pkt sąsiadującego do końca 
                                        Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].H = Heurystyka(Pola[poleNajnizszeF.PolaSasiadujace[j] - 1], PoleK);
                                        Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].F = Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].G + Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].H;
                                    }
                                    // jeżeli  pole sąsiadujące jest w tablicy PolaNieodwiedzoneSasiadujace to zapisz to do zmiennej pomocniczej
                                    zawiera = true;
                                }
                            }
                            if (zawiera != true)// jeżeli nie ma w tab PolaNieodwiedzoneSasiadujace to dodaj i przypisz tempG do G
                            {
                                Pola[(poleNajnizszeF.PolaSasiadujace[j]) - 1].G = tempG;
                                PolaNieodwiedzoneSasiadujace.Add(Pola[poleNajnizszeF.PolaSasiadujace[j] - 1]);
                                Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].Rodzic = poleNajnizszeF;
                                Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].H = Heurystyka(Pola[poleNajnizszeF.PolaSasiadujace[j] - 1], PoleK);
                                Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].F = Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].G + Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].H;
                            }

                        }
                        else // jeżeli nie ma w tab PolaNieodwiedzoneSasiadujace (nie na żadnych elementów w tablicy) to dodaj i przypisz tempG do G
                        {
                            Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].G = tempG;
                            PolaNieodwiedzoneSasiadujace.Add(Pola[poleNajnizszeF.PolaSasiadujace[j] - 1]);
                            Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].Rodzic = poleNajnizszeF;
                            Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].H = Heurystyka(Pola[poleNajnizszeF.PolaSasiadujace[j] - 1], PoleK);
                            Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].F = Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].G + Pola[poleNajnizszeF.PolaSasiadujace[j] - 1].H;
                        }






                    }

                }

            }
            return Sciezka;//Sciezka;
        }


        /////////////////////////////         FUNKCJE            ////////////////////

        public static int Heurystyka(PoleModel pkt, PoleModel koniec)
        {
            int a = koniec.X - pkt.X;
            int b = koniec.Y - pkt.Y;
            int H = (int)(10 * (Math.Sqrt((a * a) + (b * b))));
            return H;
        }


        public static List<PoleModel> RekonstrukcjaSciezki(PoleModel PunktStartowy, PoleModel PunktKoncowy)
        {
            List<PoleModel> Sciezka = new(new[] { PunktKoncowy });
            while (PunktKoncowy.Rodzic != PunktStartowy)
            {
                Sciezka.Add(PunktKoncowy.Rodzic);
                PunktKoncowy = PunktKoncowy.Rodzic;
            }
            Sciezka.Reverse();
            return Sciezka;
        }

        public static bool HaveNeighbourObstacle(int Id1, int Id2, List<PoleModel> Pola)
        {
            PoleModel PolePoSkosie = null;
            PoleModel PolePierwotne = null;
            PoleModel PoleWspolne = null;
            bool CzyPrzeszkoda = false;
            for (int i = 0; i < Pola.Count; i++)
            {

                if (Pola[i].Id == Id2)
                {
                    PolePoSkosie = Pola[i];
                }
                else if (Pola[i].Id == Id1)
                {
                    PolePierwotne = Pola[i];
                }
            }
            for (int i = 0; i < PolePierwotne.PolaSasiadujace.Count; i++)
            {
                for (int j = 0; j < PolePoSkosie.PolaSasiadujace.Count; j++)
                {


                    if (PolePierwotne.PolaSasiadujace[i] == PolePoSkosie.PolaSasiadujace[j])
                    {
                        for (int m = 0; m < Pola.Count; m++)
                        {
                            if (Pola[m].Id == PolePoSkosie.PolaSasiadujace[j])
                            {
                                PoleWspolne = Pola[m];
                                if (PoleWspolne.Osiagalny == false)
                                {
                                    CzyPrzeszkoda = true;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return CzyPrzeszkoda;
        }



    }
}
